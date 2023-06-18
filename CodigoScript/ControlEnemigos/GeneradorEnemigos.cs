using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    //private Transform transform;
    //public GameObject enemyRojo;
    //public float cadenciaRojo = 3f;

    private int waveActual;

    private float dificultadPlus = 30f;
    private float tiempoActualdificultadPlus;

    private float cadenciaRojo = 2f;
    private float tiempoActualRojo;

    private float cadenciaVerde = 2.6f;
    private float tiempoActualVerde;

    private float cadenciaAzul = 3.3f;
    private float tiempoActualAzul;

    private float cadenciaPowerUpVida = 22.4f;
    private float tiempoActualVida;

    private float cadenciaPowerUpShoot = 15.2f;
    private float tiempoActualPowerUpShoot;

    private RojoGenerador rojoGenerador;
    private VerdeGenerador verdeGenerador;
    private AzulGenerador azulGenerador;
    private GeneradorPowerUpVida generadorPowerUpVida;
    private GeneradorShootPowerUp generadorShootPowerUp;

    private int ataqueRojo1 = 40, ataqueRojo2 = 40, ataqueRojo3 = 15;
    private int ataqueVerde1 = 100, ataqueVerde2 = 2, ataqueVerde3 = 1;
    private int ataqueAzul1 = 100, ataqueAzul2 = 2, ataqueAzul3 = 1;

    // Start is called before the first frame update
    void Start()
    {
        tiempoActualRojo = cadenciaRojo;
        tiempoActualVerde = cadenciaVerde;
        tiempoActualAzul = cadenciaAzul;
        tiempoActualVida = cadenciaPowerUpVida;
        tiempoActualPowerUpShoot = cadenciaPowerUpShoot;

        tiempoActualdificultadPlus = dificultadPlus;

        rojoGenerador = GetComponent<RojoGenerador>();
        verdeGenerador = GetComponent<VerdeGenerador>();
        azulGenerador = GetComponent<AzulGenerador>();
        generadorPowerUpVida = GetComponent<GeneradorPowerUpVida>();
        generadorShootPowerUp = GetComponent<GeneradorShootPowerUp>();

        if (FindObjectOfType<PasarParametros>() != null)
        {
            waveActual = FindObjectOfType<PasarParametros>().waveActual;
        }
        

        //transform= GetComponent<Transform>();
        //InvokeRepeating("crearEnemyRojo", 0.0f, cadenciaRojo);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (waveActual == 1) wave1();
        if (waveActual == 2) wave2();
        if (waveActual == 3) wave3();
    }

    public void wave1()
    {
        controlPowerUpVida();
        controlPowerUpShootLvl();

        tiempoActualdificultadPlus -= Time.deltaTime;

        if (tiempoActualdificultadPlus > 3f)
        {
            controlAtaqueRojo();
        }
        
        if(tiempoActualdificultadPlus <= 0)
        {
            generadorPowerUpVida.generarPowerUp();

            tiempoActualdificultadPlus = dificultadPlus;
            rojoGenerador.modificarVelocidad(rojoGenerador.velocidad * 1.2f);
            if(cadenciaRojo > 0.5) cadenciaRojo -= 0.1f;


        }
    }

    public void wave2()
    {
        controlPowerUpVida();
        controlPowerUpShootLvl();

        tiempoActualdificultadPlus -= Time.deltaTime;
        if (tiempoActualdificultadPlus > 4f)
        {
            controlAtaqueRojo();
            controlDeTiempoVerde();
            controlDeTiempoAzul();
        }
        if (tiempoActualdificultadPlus <= 0)
        {
            generadorPowerUpVida.generarPowerUp();

            tiempoActualdificultadPlus = dificultadPlus;
            //Rojo
            rojoGenerador.modificarVelocidad(rojoGenerador.velocidad * 1.1f);
            if (cadenciaRojo > 1) cadenciaRojo -= 0.1f;

            //Verde
            verdeGenerador.modificarVelocidad(verdeGenerador.velocidadX * 1.1f, verdeGenerador.velocidadY * 1.1f);
            if (cadenciaVerde > 1.3) cadenciaVerde -= 0.1f;
            ataqueVerde2 = 30;
            ataqueVerde3 = 15;

            //Azul
            azulGenerador.modificarVelocidad(azulGenerador.velocidad * 1.1f, azulGenerador.velocidadHaciaAtras);
            if (cadenciaAzul > 1.8) cadenciaAzul -= 0.1f;
            ataqueAzul2 = 30;
            ataqueAzul3 = 15;
        }

    }

    public void wave3()
    {
        controlPowerUpVida();
        controlPowerUpShootLvl();

        tiempoActualdificultadPlus -= Time.deltaTime;
        if (tiempoActualdificultadPlus > 4f)
        {
            controlAtaqueRojo();
            controlDeTiempoVerde();
            controlDeTiempoAzul();
        }
        if (tiempoActualdificultadPlus <= 0)
        {
            generadorPowerUpVida.generarPowerUp();

            tiempoActualdificultadPlus = dificultadPlus;
            //Rojo
            rojoGenerador.modificarVelocidad(rojoGenerador.velocidad * 1.2f);
            if (cadenciaRojo > 1) cadenciaRojo -= 0.2f;

            //Verde
            verdeGenerador.modificarVelocidad(verdeGenerador.velocidadX * 1.2f, verdeGenerador.velocidadY * 1.2f);
            if (cadenciaVerde > 1.3) cadenciaVerde -= 0.2f;
            ataqueVerde2 = 30;
            ataqueVerde3 = 15;

            //Azul
            azulGenerador.modificarVelocidad(azulGenerador.velocidad * 1.2f, azulGenerador.velocidadHaciaAtras);
            if (cadenciaAzul > 1.8) cadenciaAzul -= 0.2f;
            ataqueAzul2= 30;
            ataqueAzul3= 15;
        }

    }


    private void controlPowerUpVida()
    {
        tiempoActualVida -= Time.deltaTime;

        if (tiempoActualVida <= 0)
        {
            tiempoActualVida = cadenciaPowerUpVida;

            generadorPowerUpVida.generarPowerUp();

        }
    }

    private void controlPowerUpShootLvl()
    {
        tiempoActualPowerUpShoot -= Time.deltaTime;

        if (tiempoActualPowerUpShoot <= 0)
        {
            tiempoActualPowerUpShoot = cadenciaPowerUpShoot;

            generadorShootPowerUp.generarPowerUp();

        }
    }

    private void controlAtaqueRojo()//Va generando enemigos rojos segun un temporizador
    {
        tiempoActualRojo -= Time.deltaTime;

        if (tiempoActualRojo <= 0)
        {
            tiempoActualRojo = cadenciaRojo;

            switch (Random.Range(0, ataqueRojo1 + ataqueRojo2 + ataqueRojo3)) //eligue de forma aleatoria el tipo de la siguiente oleada
            {
                case int n when n >= 0 && n < ataqueRojo1:
                    rojoGenerador.generarOleadaDe_3_Normal();
                    break;
                case int n when n >= ataqueRojo1 && n < ataqueRojo1 + ataqueRojo2:
                    rojoGenerador.generarOleadaTipoBarrera();
                    break;
                case int n when n >= ataqueRojo1 + ataqueRojo2 && n <= ataqueRojo1 + ataqueRojo2 + ataqueRojo3:
                    rojoGenerador.generarOleadaEspecialConHuecos();
                    break;
            }
            
        }
    }
    private void controlDeTiempoVerde()
    {
        tiempoActualVerde -= Time.deltaTime;

        if (tiempoActualVerde <= 0)
        {
            tiempoActualVerde = cadenciaVerde;

            switch (Random.Range(0, ataqueVerde1 + ataqueVerde2 + ataqueVerde3)) //eligue de forma aleatoria el tipo de la siguiente oleada
            {
                case int n when n >= 0 && n < ataqueVerde1:
                    verdeGenerador.generarAtaqueIndividual();
                    break;
                case int n when n >= ataqueVerde1 && n < ataqueVerde1 + ataqueVerde2:
                    verdeGenerador.generarOleada_serpiente_de_6();
                    break;
                case int n when n >= ataqueVerde1 + ataqueVerde2 && n <= ataqueVerde1 + ataqueVerde2 + ataqueVerde3:
                    verdeGenerador.generarOleada_en_X(8);
                    break;
            }

        }
    }

    private void controlDeTiempoAzul()
    {
        tiempoActualAzul -= Time.deltaTime;

        if (tiempoActualAzul <= 0)
        {
            tiempoActualAzul = cadenciaAzul;

            switch (Random.Range(0, ataqueAzul1 + ataqueAzul2 + ataqueAzul3)) //eligue de forma aleatoria el tipo de la siguiente oleada
            {
                case int n when n >= 0 && n < ataqueAzul1:
                    azulGenerador.generarAtaqueIndividualAleatorio();
                    break;
                case int n when n >= ataqueAzul1 && n < ataqueAzul1 + ataqueAzul2:
                    azulGenerador.generarOleada_Doble ();
                    break;
                case int n when n >= ataqueAzul1 + ataqueAzul2 && n <= ataqueAzul1 + ataqueAzul2 + ataqueAzul3:
                    azulGenerador.generarOleada_Triple();
                    break;
            }

        }
    }



    private void crearEnemyRojo()
    {
        //Vector3 SpawnPosition = transform.position;
        //GameObject EnemyRojo = Instantiate(enemyRojo,SpawnPosition,Quaternion.identity);
    }
}
