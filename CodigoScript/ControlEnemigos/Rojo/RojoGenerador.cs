using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RojoGenerador : MonoBehaviour
{
    public float velocidad = -200f;

    public GameObject enemyRojo;

    public List<GameObject> reservaRoja;

    // Start is called before the first frame update
    void Start()
    {
        velocidad = -200f;
        //inicializarPoolRoja(); 
        //instanciarTodaLaPoolRoja(reservaRoja);
    }

    // Update is called once per frame
    void Update()
    {

    }


    void inicializarPoolRoja()  //crea varios enemigos para guardarlos en la reserva
    {
        reservaRoja = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            GameObject instance = Instantiate(enemyRojo);
            instance.SetActive(false);
            reservaRoja.Add(instance);
        }
    }

    public void ifRellenaReserva(int cantidad) //Comprueba si hay suficientes enemigos en la reserva y los crea si es necesario
    {
        if (reservaRoja.Count < cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                GameObject instance = Instantiate(enemyRojo);
                instance.SetActive(false);
                reservaRoja.Add(instance);
            }
        }
    }
    
    //TIPOS DE ATAQUES
    
    public void generarOleadaDe_3_Normal() //Genera una Oleada de 3 enemigos rojos a una altura determinada
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
        logicaGenerarOleada(altura, 3, 0, 0);
    }

    public void generarOleadaTipoBarrera()
    {

        int altura = 0; //la altura donde inicia a generar los enemigos
        int enemigosGenerados = 1; //cantidad de enemigos a generar
        int separacion = 0; //separacion entre enemigos
        int intervaloSeparacion = -1; //cuando se generar x cantidad de enemigos se realizara la separación

        switch (Random.Range(0, 3))
        {
            case 0:
                altura = 4;
                enemigosGenerados = 6;
                break;
            case 1:
                altura = 1;
                enemigosGenerados = 6;
                break;
            case 2:
                altura = 4;
                enemigosGenerados = 6;
                intervaloSeparacion = 3;
                separacion = 3;
                break;
        }

        ifRellenaReserva(enemigosGenerados);
        logicaGenerarOleada(altura, enemigosGenerados, separacion, intervaloSeparacion);
    }

    public void generarOleadaEspecialConHuecos()
    {

        int altura = 0; //la altura donde inicia a generar los enemigos
        int enemigosGenerados = 1; //cantidad de enemigos a generar
        int separacion = 0; //separacion entre enemigos
        int intervaloSeparacion = -1; //cuando se generar x cantidad de enemigos se realizara la separación
        switch (Random.Range(0, 2))
        {
            case 0:
                altura = 4;
                enemigosGenerados = 5;
                separacion = 1;
                break;
            case 1:
                altura = 3;
                enemigosGenerados = 4;
                separacion = 1;
                break;
        }
        ifRellenaReserva(enemigosGenerados);
        logicaGenerarOleada(altura, enemigosGenerados, separacion, intervaloSeparacion);
    }


    private void logicaGenerarOleada(int altura, int cantidad, int aumento, int intervaloSeparacion)
    {
        int separacion = 0;
        int totalReserva = reservaRoja.Count - 1;
        for (int i = 0; i < cantidad; i++)
        {
            if (i == intervaloSeparacion)
            {
                separacion += aumento;
                intervaloSeparacion += intervaloSeparacion;
            }

            Vector3 SpawnPosition = new Vector3(transform.position.x, (transform.position.y + altura)-separacion, transform.position.z);
            reservaRoja[totalReserva - i].GetComponent<RojoEnemyControl>().modificarVelocidad(velocidad);
            reservaRoja[totalReserva - i].transform.position = SpawnPosition;
            reservaRoja[totalReserva - i].SetActive(true);
            reservaRoja.RemoveAt(totalReserva - i);
            altura--;

            if(intervaloSeparacion == -1)
            {
                separacion += aumento;
            }
            
        }
    }

    

    //UTILIDADES

    public void desactivarEnemigoRojo(GameObject enemigoRojo) //Usada para descativar enemigos (los propios enemigos llaman a esta función desde sus scripts)
    {
        enemigoRojo.SetActive(false);
        enemigoRojo.transform.position = new Vector3(10f, 0f, transform.position.z);
        reservaRoja.Add(enemigoRojo);
    }

    public void modificarVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }


    ///////////////////////////////////////////////////////////////////////////////////////
    public void instanciarTodaLaPoolRoja(List<GameObject> poolRoja) //sin uso
    {
        int aumento = 0;
        foreach (GameObject prefab in poolRoja)
        {
            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + aumento, transform.position.z);
            Instantiate(prefab, SpawnPosition, Quaternion.identity);
            aumento++;
        }
    }
}
