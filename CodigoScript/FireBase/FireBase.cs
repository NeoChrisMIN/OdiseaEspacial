using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using System;
using UnityEngine.UI;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

public class FireBase : MonoBehaviour
{

    private FirebaseApp _app;
    FirebaseAuth auth;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                _app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                auth = FirebaseAuth.DefaultInstance;
                auth.SignOut();

                //SignInWithGoogle(false);
                //login();
            }
            else
            {
                UnityEngine.Debug.Log(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Método para comprobar el estado de la conexión a Internet
    public bool CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;
        else return true;
    }
    //---------------------------------------  Login --------------------------------------------
    private void loginAnonimo() //sin actualizar y sin uso (aunque funciona perfectamente, solo hay que activar el anonimo en la consola firebase)
    {
        auth = FirebaseAuth.DefaultInstance;
        if( auth.CurrentUser != null)
        {
            Debug.Log("este usuario ya esta identificado");
        }
        else
        {
            auth.SignInAnonymouslyAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.Log("SignInAnonymouslyAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.Log("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

            });
        }
        
    }



    public void registerEmailAndPassword(String email, String password, Text error, Action<string> callback)
    {
        /*if (!verifyEmailExists(email))
        {
            UnityMainThreadDispatcher.RunOnMainThread(() =>
            {
                // Código que quieres ejecutar en el hilo principal
                error.text = "the email is not real";
            });
            return;
        }*/
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                UnityMainThreadDispatcher.RunOnMainThread(() =>
                {
                    // Código que quieres ejecutar en el hilo principal
                    error.text = "CreateUserWithEmailAndPasswordAsync was canceled.";
                });
                
                Debug.Log("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                var exception = task.Exception.GetBaseException() as Firebase.FirebaseException;
                UnityMainThreadDispatcher.RunOnMainThread(() =>
                {
                    // Código que quieres ejecutar en el hilo principal
                    error.text = "The user already exists";
                });
                Debug.Log("task.IsFaulted");
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.LogFormat("displayname: " + result.User.DisplayName + " |||| User id: " + result.User.UserId);
            Debug.LogFormat("estoy aqui en register");

            UnityMainThreadDispatcher.RunOnMainThread(() =>
            {
                // Código que quieres ejecutar en el hilo principal
                callback.Invoke(result.User.Email);
            });
        }); 
    }


    public void loginEmailAndPassword(String email, String password, Text error, Action<string> callback)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                UnityMainThreadDispatcher.RunOnMainThread(() =>
                {
                    // Código que quieres ejecutar en el hilo principal
                    error.text = "unexpected error";
                });
                return;
            }
            if (task.IsFaulted)
            {
                UnityMainThreadDispatcher.RunOnMainThread(() =>
                {
                    // Código que quieres ejecutar en el hilo principal
                    error.text = "email or password not correct";
                });
                return;
            }

            AuthResult result = task.Result;
            Debug.Log("displayname: " + result.User.DisplayName + " |||| User id: " + result.User.UserId);
            Debug.Log("Email del usuario "+ result.User.Email);
            Debug.Log("estoy aqui en login");
            UnityMainThreadDispatcher.RunOnMainThread(() =>
            {
                // Código que quieres ejecutar en el hilo principal
                callback.Invoke(result.User.Email);
            });
        });
    }

    public void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log("Usuario deslogueado correctamente.");
    }

    public void usuarioYaIdentificado(Action<string> callback)
    {
        if (auth == null) return;
        if (auth.CurrentUser != null)
        {
            Debug.Log("este usuario ya esta identificado");
            Debug.Log(auth.CurrentUser.Email);
            callback.Invoke(auth.CurrentUser.Email);
            return;
        }
    }

    //--------------------------------------------------------  Records  ---------------------------------------------------------
    public void addRecord(float score)
    {
        Debug.Log("aqui addRecord");
        if (auth == null) return;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("lvl"+ FindObjectOfType<PasarParametros>().waveActual).Document(auth.CurrentUser.Email);
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "record", (int)score }
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added record in the users collection.");
        });

        //Debug.Log("Record en la base de datos : " + record);
    }

    public void getRecord(Action<int> callback)
    {
        /*FirebaseFirestore db = null;
        CollectionReference usersRef = null;
        DocumentReference documentRef = null;

        if (comprobarAntesDeHacerGet(db, usersRef, documentRef) == false) return;*/


        if (auth == null) return;
        
        Debug.Log("aqui GetRecord");
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        
        CollectionReference usersRef = db.Collection("lvl"+ FindObjectOfType<PasarParametros>().waveActual);
        if (usersRef == null)
        {
            Debug.LogError("GetRecord usersRef = null");
            return;
        }
        if(auth.CurrentUser == null)
        {
            Debug.LogError("GetRecord auth.CurrentUser = null");
            return;
        } 
        DocumentReference documentRef = usersRef.Document(auth.CurrentUser.Email);

        if (documentRef == null)
        {
            Debug.LogError("GetRecord documentRef = null");
            return;
        }

        documentRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            int record = -1;
             
            if (snapshot.Exists)
            {
                Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

                if (documentDictionary.ContainsKey("record"))
                {
                    record = Convert.ToInt32(documentDictionary["record"]);
                }
            }

            // Llamar a la función de devolución de llamada con el valor del registro
            callback.Invoke(record);
        });
        /*
        usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>  //esto viene a ser lo mismo que documentRef.GetSnapshotAsyn ... pero de otra manera que internamente es menos combeniente
        {
            QuerySnapshot snapshot = task.Result;
            int record = -1;

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Debug.Log($"User: {document.Id}");
                Dictionary<string, object> documentDictionary = document.ToDictionary();

                if (documentDictionary.ContainsKey("record"))
                {
                    record = Convert.ToInt32(documentDictionary["record"]);
                    break;
                }
            }

            // Llamar a la función de devolución de llamada con el valor del registro
            callback.Invoke(record);
        });*/
    }

    //-------------------------------------------------------- Best score --------------------------------------------------------


    public void addBestRecord(float score)
    {
        Debug.Log("aqui addBestRecord");

        if (auth == null) return;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("BestLvl" + FindObjectOfType<PasarParametros>().waveActual).Document("bestRecord");
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "bestRecord", (int)score },
                { "Email", auth.CurrentUser.Email}
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added record in the users collection.");
        });
    }

    public void getBestRecord(Action<int,string> callback)
    {
        int record = -1;
        string email = "anonimo";
        if (auth == null) return;

        Debug.Log("aqui getBestRecord");
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        CollectionReference usersRef = db.Collection("BestLvl" + FindObjectOfType<PasarParametros>().waveActual);
        if (usersRef == null)
        {
            Debug.LogError("getBestRecord usersRef = null");
            callback.Invoke(record, email);
            return;
        }
        if (auth.CurrentUser == null)
        {
            Debug.LogError("getBestRecord auth.CurrentUser = null");
            callback.Invoke(record, email);
            return;
        }
        DocumentReference documentRef = usersRef.Document("bestRecord");

        if (documentRef == null)
        {
            Debug.LogError("GetRecord documentRef = null");
            callback.Invoke(record, email);
            return;
        }

        documentRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

                if (documentDictionary.ContainsKey("bestRecord"))
                {
                    record = Convert.ToInt32(documentDictionary["bestRecord"]);
                    email = documentDictionary["Email"].ToString();
                }
            }

            // Llamar a la función de devolución de llamada con el valor del registro
            callback.Invoke(record,email);
        });
    }


    //--------------------------------------------------------  Mejoras  ---------------------------------------------------------


    public void addVidas(int vidasTotales)
    {
        Debug.Log("aqui addVidas");
        if (auth == null) return;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("capacidadDeVidas").Document(auth.CurrentUser.Email);
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "maximoDeVidas", vidasTotales }
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added record in the users collection.");
        });
    }
    public void getVidas(Action<int> callback)
    {
        if (auth == null) return;

        Debug.Log("aqui getVidas");
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        CollectionReference usersRef = db.Collection("capacidadDeVidas");
        if (usersRef == null)
        {
            Debug.LogError("getVidas usersRef = null");
            return;
        }
        if (auth.CurrentUser == null)
        {
            Debug.LogError("getVidas auth.CurrentUser = null");
            return;
        }
        DocumentReference documentRef = usersRef.Document(auth.CurrentUser.Email);

        if (documentRef == null)
        {
            Debug.LogError("getVidas documentRef = null");
            return;
        }

        documentRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            int record = -1;

            if (snapshot.Exists)
            {
                Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

                if (documentDictionary.ContainsKey("maximoDeVidas"))
                {
                    record = Convert.ToInt32(documentDictionary["maximoDeVidas"]);
                }
            }

            // Llamar a la función de devolución de llamada con el valor del registro
            callback.Invoke(record);
        });
    }

    //---------------------------------------------------------------------------------------------------------------
    public void addlvlshoot(int lvl)
    {
        Debug.Log("aqui addlvlshoot");
        if (auth == null) return;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("ShootLvl").Document(auth.CurrentUser.Email);
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "lvl", lvl }
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added record in the users collection.");
        });
    }

    public void getlvlshoot(Action<int> callback)
    {
        if (auth == null) return;

        Debug.Log("aqui getlvlshoot");
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        CollectionReference usersRef = db.Collection("ShootLvl");
        if (usersRef == null)
        {
            Debug.LogError("getlvlshoot usersRef = null");
            return;
        }
        if (auth.CurrentUser == null)
        {
            Debug.LogError("getlvlshoot auth.CurrentUser = null");
            return;
        }
        DocumentReference documentRef = usersRef.Document(auth.CurrentUser.Email);

        if (documentRef == null)
        {
            Debug.LogError("getlvlshoot documentRef = null");
            return;
        }

        documentRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            int record = -1;

            if (snapshot.Exists)
            {
                Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

                if (documentDictionary.ContainsKey("lvl"))
                {
                    record = Convert.ToInt32(documentDictionary["lvl"]);
                }
            }

            // Llamar a la función de devolución de llamada con el valor del registro
            callback.Invoke(record);
        });
    }

    //---------------------------------------------------------------------------------------------------------------

    public void addCadence(int cadence)
    {
        Debug.Log("aqui addVidas");
        if (auth == null) return;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cadence").Document(auth.CurrentUser.Email);
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "cadence", cadence }
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added record in the users collection.");
        });
    }

    public void getCadence(Action<int> callback)
    {
        if (auth == null) return;

        Debug.Log("aqui getCadence");
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        CollectionReference usersRef = db.Collection("Cadence");
        if (usersRef == null)
        {
            Debug.LogError("getCadence usersRef = null");
            return;
        }
        if (auth.CurrentUser == null)
        {
            Debug.LogError("getCadence auth.CurrentUser = null");
            return;
        }
        DocumentReference documentRef = usersRef.Document(auth.CurrentUser.Email);

        if (documentRef == null)
        {
            Debug.LogError("getCadence documentRef = null");
            return;
        }

        documentRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            int record = -1;

            if (snapshot.Exists)
            {
                Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

                if (documentDictionary.ContainsKey("cadence"))
                {
                    record = Convert.ToInt32(documentDictionary["cadence"]);
                }
            }

            // Llamar a la función de devolución de llamada con el valor del registro
            callback.Invoke(record);
        });
    }

    //---------------------------------------------------------------------------------------------------------------
    public void addCoins(int coins)
    {
        Debug.Log("aqui addVidas");
        if (auth == null) return;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Coins").Document(auth.CurrentUser.Email);
        Dictionary<string, object> user = new Dictionary<string, object>
        {
                { "coins", coins }
        };
        docRef.SetAsync(user).ContinueWithOnMainThread(task => {
            Debug.Log("Added record in the users collection.");
        });
    }

    public void getCoins(Action<int> callback)
    {
        if (auth == null) return;

        Debug.Log("aqui getCoins");
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;

        CollectionReference usersRef = db.Collection("Coins");
        if (usersRef == null)
        {
            Debug.LogError("getCoins usersRef = null");
            return;
        }
        if (auth.CurrentUser == null)
        {
            Debug.LogError("getCoins auth.CurrentUser = null");
            return;
        }
        DocumentReference documentRef = usersRef.Document(auth.CurrentUser.Email);

        if (documentRef == null)
        {
            Debug.LogError("getCoins documentRef = null");
            return;
        }

        documentRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            int record = -1;

            if (snapshot.Exists)
            {
                Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

                if (documentDictionary.ContainsKey("coins"))
                {
                    record = Convert.ToInt32(documentDictionary["coins"]);
                }
            }

            // Llamar a la función de devolución de llamada con el valor del registro
            callback.Invoke(record);
        });
    }


    //-------------------------------------------------------------------------------------------------------------------------------------------- 


}










//*********************************************************************************************************************************************************************

/*
public async Task<bool> IsUserLoggedInOnOtherDevices()
{
    FirebaseAuth auth = FirebaseAuth.DefaultInstance;
    FirebaseUser user = auth.CurrentUser;

    if (user == null)
    {
        // No hay usuario autenticado
        return false;
    }

    // Obtener el token de identificación del usuario
    string idToken = await user.GetIdTokenAsync();

    // Aquí debes enviar el token al servidor y verificar si está activo en otro dispositivo
    // Puedes implementar tu propia lógica en el servidor para realizar esta verificación

    // Ejemplo ficticio: supongamos que el servidor devuelve un valor booleano indicando si la sesión está activa en otro dispositivo
    bool isLoggedInOnOtherDevices = YourServerCheck(idToken);

    return isLoggedInOnOtherDevices;
}

// Función ficticia para verificar el token en el servidor
private bool YourServerCheck(string idToken)
{
    // Aquí debes implementar tu lógica para verificar el token en el servidor
    // Puedes usar Firebase Admin SDK o cualquier otra biblioteca para realizar esta verificación
    // Devuelve true si la sesión está activa en otro dispositivo, o false en caso contrario

    // Ejemplo ficticio: siempre se considera que la sesión está activa en otro dispositivo
    return true;
}
*/




/*
public static bool verifyEmailExists(string emailAddress)
{
    try
    {
        // Crear un cliente SMTP con los detalles de tu servidor de correo saliente
        SmtpClient client = new SmtpClient("tu.servidor.com", 587);
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential("tu_correo@dominio.com", "tu_contraseña");
        client.EnableSsl = true;

        // Intentar enviar un correo electrónico a la dirección proporcionada
        MailMessage message = new MailMessage();
        message.To.Add(emailAddress);
        message.Subject = "Verificación de correo electrónico";
        message.Body = "Este correo electrónico se utiliza para verificar la existencia de la dirección.";

        client.Send(message);

        return true; // El correo electrónico se envió correctamente, por lo tanto, la dirección existe
    }
    catch (Exception)
    {
        return false; // Ocurrió un error al enviar el correo electrónico, por lo tanto, la dirección no existe o es inválida
    }
}*/





/*private bool comprobarAntesDeHacerGet(FirebaseFirestore db, CollectionReference usersRef, DocumentReference documentRef)
    {
        if (auth == null) return false;

        Debug.Log("aqui GetRecord");
        db = FirebaseFirestore.DefaultInstance;

        usersRef = db.Collection("lvl" + FindObjectOfType<PasarParametros>().waveActual);
        if (usersRef == null)
        {
            Debug.LogError("GetRecord usersRef = null");
            return false;
        }
        if (auth.CurrentUser == null)
        {
            Debug.LogError("GetRecord auth.CurrentUser = null");
            return false;
        }
        documentRef = usersRef.Document(auth.CurrentUser.Email);

        if (documentRef == null)
        {
            Debug.LogError("GetRecord documentRef = null");
            return false;
        }

        return true;
    }*/





/* 
using System.Threading.Tasks;
//using UnityEngine.UT;
using Google;
using System.Net.Http;

    public void SignInWithGoogle(bool linkWithCurrentAnonUser)
    {
        Debug.Log("estoy aqui 1");

        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");


        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            // Copy this value from the google-service.json file.
            // oauth_client with type == 3
            WebClientId = "327599398520-3mhog7nkasipiui8idgsrfup0dar6alo.apps.googleusercontent.com"
        };

        Debug.Log("estoy aqui 2");

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();

        Debug.Log("estoy aqui 3");

        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWith(task =>
        {
            Debug.Log("estoy aqui 4");
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
                Debug.Log("estoy Auth Google canceled");
            }
            else if (task.IsFaulted)
            {
                Debug.Log("esto es lo que busco = "+task.Exception);
                signInCompleted.SetException(task.Exception);
                Debug.Log("estoy Auth Google Exception");
            }
            else
            {
                Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(((Task<GoogleSignInUser>)task).Result.IdToken, null);
                if (linkWithCurrentAnonUser)
                {
                    auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(HandleLoginResult2);
                }
                else
                {
                    SignInWithCredential(credential);
                }
            }
        });
    }
    private void SignInWithCredential(Credential credential)
    {
        if (auth != null)
        {
            auth.SignInWithCredentialAsync(credential).ContinueWith(HandleLoginResult1);
        }
    }


    private void HandleLoginResult1(Task<FirebaseUser> task)
    {
        Debug.Log("estoy aqui fin 1");
        if (task.IsCanceled)
        {
            UnityEngine.Debug.LogError("SignInWithCredentialAsync was canceled.");
            return;
        }
        if (task.IsFaulted)
        {
            UnityEngine.Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception.InnerException.Message);
            return;
        }
        else
        {
            FirebaseUser newUser = task.Result;
            UnityEngine.Debug.Log($"User signed in successfully1: {newUser.DisplayName} ({newUser.UserId})");
        }
    }
    private void HandleLoginResult2(Task<Firebase.Auth.AuthResult> task)
    {
        Debug.Log("estoy aqui fin 2");
        if (task.IsCanceled)
        {
            UnityEngine.Debug.LogError("SignInWithCredentialAsync was canceled.");
            return;
        }
        if (task.IsFaulted)
        {
            UnityEngine.Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception.InnerException.Message);
            return;
        }

        Firebase.Auth.AuthResult result = task.Result;
        FirebaseUser newUser = result.User;
        UnityEngine.Debug.Log($"User signed in successfully2: {newUser.DisplayName} ({newUser.UserId})");
    }
*/