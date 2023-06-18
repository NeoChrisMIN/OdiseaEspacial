using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasarParametros : MonoBehaviour
{
    public bool juegoEnCurso;
    public int waveActual;
    public bool juegoSilenciado;

    public int scoreLogrado;

    public string emailJugador;
    public int vidasTotales;
    public int lvlShoot;
    public float cadenceShoot;

    //--------------------------------------------------
    public int ataqueRojo1, ataqueRojo2, ataqueRojo3;
    //--------------------------------------------------
    public int ataqueVerde1, ataqueVerde2, ataqueVerde3;
    //--------------------------------------------------
    public int ataqueAzul1, ataqueAzul2, ataqueAzul3;
    //--------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindWithTag("mantenedorDatos") == null)
        {
            tag = "mantenedorDatos";
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        scoreLogrado = 0;
        juegoEnCurso = false;
        emailJugador = "Anonimo";
        juegoSilenciado = false;

        lvlShoot = 1;
        vidasTotales = 3;
        cadenceShoot = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
