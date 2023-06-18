using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerdeGenerador : MonoBehaviour
{
    public float velocidadX;
    public float velocidadY;

    public GameObject enemyVerde;

    public List<GameObject> reservaVerde;

    //
    private string UP = "arriba";
    private string BOTTOM = "abajo";
    //

    // Start is called before the first frame update
    void Start()
    {
        velocidadX = -200f;
        velocidadY = -200f;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void inicializarPoolVerde()  //crea varios enemigos para guardarlos en la reserva
    {
        reservaVerde = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            GameObject instance = Instantiate(enemyVerde);
            instance.SetActive(false);
            reservaVerde.Add(instance);
        }
    }

    public void ifRellenaReserva(int cantidad) //Comprueba si hay suficientes enemigos en la reserva y los crea si es necesario
    {
        if (reservaVerde.Count < cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                GameObject instance = Instantiate(enemyVerde);
                instance.SetActive(false);
                reservaVerde.Add(instance);
            }
        }
    }

    //TIPOS DE ATAQUES

    public void generarAtaqueIndividual()
    {
        ifRellenaReserva(1);
        logicaGenerarOleada(null, Random.Range(-4,5), 1, 0, -1, 0);
    }

    public void generarOleada_serpiente_de_3() //Genera una Oleada de 3 enemigos rojos a una altura determinada
    {

        int altura = 0;
        switch (Random.Range(0, 3)) 
        {
            case 0:
                altura = 4;
                break;
            case 1:
                altura = 1;
                break;
            case 2:
                altura = -2;
                break;
            
        }
        
        ifRellenaReserva(3);
        logicaGenerarOleada(null,altura, 3, 0, -1, 1);
    }

    public void generarOleada_serpiente_de_6()
    {

        float altura = 0; //la altura donde inicia a generar los enemigos
        int enemigosGenerados = 1; //cantidad de enemigos a generar
        int separacion = 0; //separacion entre enemigos
        int intervaloSeparacion = -1; //cuando se generar x cantidad de enemigos se realizara la separación

        switch (Random.Range(0, 2))
        {
            case 0:
                altura = 4;
                enemigosGenerados = 6;
                break;
            case 1:
                altura = 1;
                enemigosGenerados = 6;
                break;
            
        }

        ifRellenaReserva(enemigosGenerados);
        logicaGenerarOleada(null, altura, enemigosGenerados, separacion, intervaloSeparacion,1);
    }

    public void generarOleada_en_X(int enemigosGenerados)
    {
        int separacion = 0; //separacion entre enemigos
        int intervaloSeparacion = -1; //cuando se generar x cantidad de enemigos se realizara la separación

        float altura1 = 0; //la altura donde inicia a generar los enemigos
        float altura2 = 0; //la altura donde inicia a generar los enemigos

        string direccion1 = "";
        string direccion2 = "";
        switch (Random.Range(0, 2))
        {
            case 0:
                altura1 = enemigosGenerados / 2;
                altura2 = -1f;

                direccion1 = BOTTOM;
                direccion2 = UP;
                break;
            case 1:
                altura1 = 3f;
                altura2 = -1f;

                direccion1 = UP;
                direccion2 = BOTTOM;
                break;

        }

        ifRellenaReserva(enemigosGenerados);
        logicaGenerarOleada(direccion1, altura1, enemigosGenerados / 2, separacion, intervaloSeparacion, 1);
        logicaGenerarOleada(direccion2, altura2, enemigosGenerados / 2, separacion, intervaloSeparacion, 1);

    }

    public void generarOleada_serpiente_9()
    {

        float altura = 0; //la altura donde inicia a generar los enemigos
        int enemigosGenerados = 0; //cantidad de enemigos a generar
        int separacion = 0; //separacion entre enemigos
        int intervaloSeparacion = -1; //cuando se generar x cantidad de enemigos se realizara la separación
        string direccion = "";

        switch (Random.Range(0, 2))
        {
            case 0:
                altura = 4;
                enemigosGenerados = 9;
                direccion = BOTTOM;
                break;
            case 1:
                
                altura = 4;
                enemigosGenerados = 9;
                direccion = UP;
                break;

        }
        ifRellenaReserva(enemigosGenerados);
        logicaGenerarOleada(direccion, altura, enemigosGenerados, separacion, intervaloSeparacion, 1);
    }


    private void logicaGenerarOleada(string stringDireccion, float altura, int cantidad, int aumento, int intervaloSeparacionY, int intervaloseparacionX)
    {
        int direccion;
        if (stringDireccion == UP) direccion = 0;
        else if(stringDireccion == BOTTOM) direccion = 1;
        else direccion = Random.Range(0, 2);

        int separacionY = 0;
        int separacionX = 0;
        if (direccion == 1 && intervaloseparacionX != 0) separacionX = cantidad-1;
     

        int totalReserva = reservaVerde.Count - 1;
        for (int i = 0; i < cantidad; i++)
        {
            if (i == intervaloSeparacionY)
            {
                separacionY += aumento;
                intervaloSeparacionY += intervaloSeparacionY;
            }
            //cambio la velocidad
            reservaVerde[totalReserva - i].GetComponent<VerdeEnemyControl>().modificarVelocidad(velocidadX, velocidadY);

            //cambiar direccion del marciano hacia arriba o abajo
            if (direccion == 0 && reservaVerde[totalReserva - i].GetComponent<VerdeEnemyControl>().velocidadY < 0) reservaVerde[totalReserva - i].GetComponent<VerdeEnemyControl>().cambioDireccion();
            else if (direccion == 1 && reservaVerde[totalReserva - i].GetComponent<VerdeEnemyControl>().velocidadY > 0) reservaVerde[totalReserva - i].GetComponent<VerdeEnemyControl>().cambioDireccion();
            

            Vector3 SpawnPosition = new Vector3(transform.position.x + separacionX, (transform.position.y + altura) - separacionY, transform.position.z);
            reservaVerde[totalReserva - i].transform.position = SpawnPosition;
            reservaVerde[totalReserva - i].SetActive(true);
            reservaVerde.RemoveAt(totalReserva - i);
            altura--;

            //-----------
            if (direccion == 0)
            {
                separacionX += intervaloseparacionX;
            }
            else if ( direccion == 1) {
                separacionX -= intervaloseparacionX;
                
            }
            //------------

            if (intervaloSeparacionY == -1) separacionY += aumento;

        }
    }



    //UTILIDADES

    public void desactivarEnemigoVerde(GameObject enemigoVerde) //Usada para descativar enemigos (los propios enemigos llaman a esta función desde sus scripts)
    {
        enemigoVerde.SetActive(false);
        enemigoVerde.transform.position = new Vector3(10f, 0f, transform.position.z);
        reservaVerde.Add(enemigoVerde);
    }

    public void modificarVelocidad(float nuevaVelocidadX, float nuevaVelocidadY)
    {
        velocidadX = nuevaVelocidadX;
        velocidadY = nuevaVelocidadY;
    }




    ///////////////////////////////////////////////////////////////////////////////////////
    public void instanciarTodaLaPoolRoja(List<GameObject> poolVerde) //sin uso
    {
        int aumento = 0;
        foreach (GameObject prefab in poolVerde)
        {
            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + aumento, transform.position.z);
            Instantiate(prefab, SpawnPosition, Quaternion.identity);
            aumento++;
        }
    }
}
