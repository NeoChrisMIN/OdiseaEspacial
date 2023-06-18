using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalBotones : MonoBehaviour
{
    public GameObject menuLogin;
    public GameObject menuSeleccionNivel;

    public AudioMixer controladorMusica;
    private bool silenciar;

    public bool modoSignIn; //false = modo registro   true == modo login

    public InputField emailInputField;
    public InputField passwordInputField;
    public Text error;

    //------------------------------
    public Button btnRegister;
    public Texture2D imgenResgisterA;
    public Texture2D imgenResgisterB;
    public Button btnLogin;
    public Texture2D imageLoginA;
    public Texture2D imageLoginB;
    public Button btnSilenciar;
    public Texture2D imageSilenciarA;
    public Texture2D imageSilenciarB;
    //------------------------------

    private bool logeado = false;

    // Start is called before the first frame update
    void Start()
    {
        modoSignIn = false;
        silenciar = FindObjectOfType<PasarParametros>().juegoSilenciado;
        if(silenciar == true) btnSilenciar.image.sprite = Sprite.Create(imageSilenciarB, new Rect(0, 0, imageSilenciarB.width, imageSilenciarB.height), Vector2.one * 0.5f);
    }
    private void Awake()
    {
        if (!FindObjectOfType<FireBase>().CheckInternetConnection())
        {
            error.text = "No Internet connection";
            return;
        }
        if (FindObjectOfType<PasarParametros>().juegoEnCurso == true)
        {
            cambioAPantallaLvls();
            return;
        }
    }

    private void FixedUpdate()
    {
        if (logeado == false)
        {
            FindObjectOfType<FireBase>().usuarioYaIdentificado(signInEcho);
        }
    }

    public void modoRegister()
    {
        btnRegister.image.sprite = Sprite.Create(imgenResgisterB, new Rect(0, 0, imgenResgisterB.width, imgenResgisterB.height), Vector2.one * 0.5f);
        btnLogin.image.sprite = Sprite.Create(imageLoginA, new Rect(0, 0, imageLoginA.width, imageLoginA.height), Vector2.one * 0.5f);
        modoSignIn = false;
    }
    public void modoLogin()
    {
        btnRegister.image.sprite = Sprite.Create(imgenResgisterA, new Rect(0, 0, imgenResgisterA.width, imgenResgisterA.height), Vector2.one * 0.5f);
        btnLogin.image.sprite = Sprite.Create(imageLoginB, new Rect(0, 0, imageLoginB.width, imageLoginB.height), Vector2.one * 0.5f);
        modoSignIn = true;
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    public void cambioAPantallaLvls()
    {
        Debug.LogFormat("estoy aqui en PantallaLvl");

        FindObjectOfType<PasarParametros>().juegoEnCurso = true;
        menuLogin.SetActive(false);
        menuSeleccionNivel.SetActive(true);
    }
    public void nivel1()
    {
        FindObjectOfType<PasarParametros>().waveActual = 1;
        SceneManager.LoadScene(1);
    }

    public void nivel2()
    {
        FindObjectOfType<PasarParametros>().waveActual = 2;
        SceneManager.LoadScene(1);
    }

    public void nivel3()
    {
        FindObjectOfType<PasarParametros>().waveActual = 3;
        SceneManager.LoadScene(1);
    }
    
    public void silenciarOdesinleciar()
    {
        if (!FindObjectOfType<PasarParametros>().juegoSilenciado)
        {
            btnSilenciar.image.sprite = Sprite.Create(imageSilenciarB, new Rect(0, 0, imageSilenciarB.width, imageSilenciarB.height), Vector2.one * 0.5f);
            controladorMusica.SetFloat("musica", -80f);
            FindObjectOfType<PasarParametros>().juegoSilenciado = true;
        }
        else
        {
            btnSilenciar.image.sprite = Sprite.Create(imageSilenciarA, new Rect(0, 0, imageSilenciarA.width, imageSilenciarA.height), Vector2.one * 0.5f);
            controladorMusica.SetFloat("musica", 0f);
            FindObjectOfType<PasarParametros>().juegoSilenciado = false;
        }

    }

    public void SignOut()
    {
        FindObjectOfType<FireBase>().SignOut();
        menuLogin.SetActive(true);
        menuSeleccionNivel.SetActive(false);
    }



    public void signInEmailPassword()
    {
        Debug.Log("estoy signInEmailPassword");

        if (!FindObjectOfType<FireBase>().CheckInternetConnection())
        {
            error.text = "No Internet connection";
            return;
        }

        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (verificarFormatoEmail(email) == false) return;
        else if (VerificarPassword(password) == false) return;

        if (modoSignIn == false)
        {
            FindObjectOfType<FireBase>().registerEmailAndPassword(email, password, error, signInEcho);
        }
        else
        {
            FindObjectOfType<FireBase>().loginEmailAndPassword(email, password, error, signInEcho);
        }

        Debug.Log("fin loginEmailPassword");
    }

    private bool verificarFormatoEmail(string email)
    {
        if (email == "")
        {
            error.text = "empty email field";
            return false;
        }
        // Patrón de expresión regular para validar el formato del correo electrónico
        string patronEmail = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        // Verificar si el correo electrónico coincide con el patrón
        bool formatoValido = Regex.IsMatch(email, patronEmail);
        if(formatoValido == false) error.text = "wrong email format";

        return formatoValido;
    }


    public bool VerificarPassword(string password)
    {
        if (password == "")
        {
            error.text = "empty password field";
            return false;
        }

        int longitudMinima = 6;

        bool formatoValido = password.Length >= longitudMinima;
        if(formatoValido == false) error.text = "password must have at least 6 characters";
        return formatoValido;
    }


    private void signInEcho(string email)
    {
        FindObjectOfType<PasarParametros>().emailJugador = email;
        logeado = true;
        Debug.LogFormat("estoy aqui en signInEcho");
        cambioAPantallaLvls();
    }
}
