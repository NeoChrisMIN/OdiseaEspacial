using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorPowerUpVida : MonoBehaviour
{
    public float velocidad = -200f;

    public GameObject powerUpVida;

    public List<GameObject> reservaPowerUpVida;

    // Start is called before the first frame update
    void Start()
    {
        velocidad = -200f;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void inicializarPoolRoja()  //crea varios enemigos para guardarlos en la reserva
    {
        reservaPowerUpVida = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            GameObject instance = Instantiate(powerUpVida);
            instance.SetActive(false);
            reservaPowerUpVida.Add(instance);
        }
    }

    public void ifRellenaReserva(int cantidad) //Comprueba si hay suficientes enemigos en la reserva y los crea si es necesario
    {
        if (reservaPowerUpVida.Count < cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                GameObject instance = Instantiate(powerUpVida);
                instance.SetActive(false);
                reservaPowerUpVida.Add(instance);
            }
        }
    }

    //TIPOS DE ATAQUES

    public void generarPowerUp() //Genera una Oleada de 3 enemigos rojos a una altura determinada
    {

        int altura = Random.Range(-4, 5);

        ifRellenaReserva(1);
        logicaGeneraPowerUp(altura, 1, 0, -1);
    }


    private void logicaGeneraPowerUp(int altura, int cantidad, int aumento, int intervaloSeparacion)
    {
        int separacion = 0;
        int totalReserva = reservaPowerUpVida.Count - 1;
        for (int i = 0; i < cantidad; i++)
        {
            if (i == intervaloSeparacion)
            {
                separacion += aumento;
                intervaloSeparacion += intervaloSeparacion;
            }

            Vector3 SpawnPosition = new Vector3(transform.position.x, (transform.position.y + altura) - separacion, transform.position.z);
            reservaPowerUpVida[totalReserva - i].GetComponent<PowerUpVida>().modificarVelocidad(velocidad);
            reservaPowerUpVida[totalReserva - i].transform.position = SpawnPosition;
            reservaPowerUpVida[totalReserva - i].SetActive(true);
            reservaPowerUpVida.RemoveAt(totalReserva - i);
            altura--;

            if (intervaloSeparacion == -1)
            {
                separacion += aumento;
            }

        }
    }



    //UTILIDADES

    public void desactivarPowerUp(GameObject enemigoRojo) //Usada para descativar enemigos (los propios enemigos llaman a esta función desde sus scripts)
    {
        enemigoRojo.SetActive(false);
        enemigoRojo.transform.position = new Vector3(10f, 0f, transform.position.z);
        reservaPowerUpVida.Add(enemigoRojo);
    }

    public void modificarVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }


    ///////////////////////////////////////////////////////////////////////////////////////
    public void instanciarTodaLaPool(List<GameObject> poolRoja) //sin uso
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
