using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzulGenerador : MonoBehaviour
{
    public float velocidad;
    public float velocidadHaciaAtras;

    public GameObject enemyAzul;

    public List<GameObject> reservaAzul;

    public List<GameObject> reservaDisparoRojo;

    // Start is called before the first frame update
    void Start()
    {
        velocidad = -300;
        velocidadHaciaAtras = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void inicializarPoolAzul()  //crea varios enemigos para guardarlos en la reserva
    {
        reservaAzul = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            GameObject instance = Instantiate(enemyAzul);
            instance.SetActive(false);
            reservaAzul.Add(instance);
        }
    }

    public void ifRellenaReserva(int cantidad) //Comprueba si hay suficientes enemigos en la reserva y los crea si es necesario
    {
        if (reservaAzul.Count < cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                GameObject instance = Instantiate(enemyAzul);
                instance.SetActive(false);
                reservaAzul.Add(instance);
            }
        }
    }

    //TIPOS DE ATAQUES

    public void generarAtaqueIndividualAleatorio()
    {
        ifRellenaReserva(1);
        logicaGenerarOleada(Random.Range(-4, 5), 1, 0, 0);
    }

    public void generarOleada_Doble()
    {
        int altura = Random.Range(1, 9);
        int aumento = Random.Range(0, altura);
        int cantidad = 2;

        ifRellenaReserva(cantidad);
        logicaGenerarOleada(altura-4, cantidad, aumento, -1);
    }

    public void generarOleada_Triple() //Genera una Oleada de 3 enemigos rojos a una altura determinada
    {
        int altura = Random.Range(0, 5);
        int aumento = 1;
        int cantidad = 3;

        ifRellenaReserva(cantidad);
        logicaGenerarOleada(altura, cantidad, aumento, 1);
    }


    private void logicaGenerarOleada(int altura, int cantidad, int aumento, int intervaloSeparacion)
    {
        int separacion = 0;
        int totalReserva = reservaAzul.Count - 1;
        for (int i = 0; i < cantidad; i++)
        {
            if (i == intervaloSeparacion)
            {
                separacion += aumento;
                intervaloSeparacion += intervaloSeparacion;
            }

            Vector3 SpawnPosition = new Vector3(transform.position.x, (transform.position.y + altura) - separacion, transform.position.z);
            reservaAzul[totalReserva - i].GetComponent<AzulEnemyControl>().modificarVelocidad(velocidad,velocidadHaciaAtras);
            reservaAzul[totalReserva - i].transform.position = SpawnPosition;
            reservaAzul[totalReserva - i].SetActive(true);
            reservaAzul.RemoveAt(totalReserva - i);
            altura--;

            if (intervaloSeparacion == -1)
            {
                separacion += aumento;
            }

        }
    }



    //UTILIDADES

    public void desactivarEnemigoAzul(GameObject enemigoAzul) //Usada para descativar enemigos (los propios enemigos llaman a esta función desde sus scripts)
    {
        enemigoAzul.SetActive(false);
        enemigoAzul.transform.position = new Vector3(10f, 0f, transform.position.z);
        reservaAzul.Add(enemigoAzul);
    }

    public void modificarVelocidad(float nuevaVelocidadHaciaDelante, float nuevaVelocidadHaciaAtras)
    {
        velocidad = nuevaVelocidadHaciaDelante;
        velocidadHaciaAtras = nuevaVelocidadHaciaAtras;
    }


    ///////////////////////////////////////////////////////////////////////////////////////
    public void instanciarTodaLaPoolAzul(List<GameObject> poolAzul)//sin uso
    {
        int aumento = 0;
        foreach (GameObject prefab in poolAzul)
        {
            Vector3 SpawnPosition = new Vector3(transform.position.x, transform.position.y + aumento, transform.position.z);
            Instantiate(prefab, SpawnPosition, Quaternion.identity);
            aumento++;
        }
    }
}
