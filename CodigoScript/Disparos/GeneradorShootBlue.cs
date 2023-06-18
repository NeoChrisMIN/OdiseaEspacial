using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorShootBlue : MonoBehaviour
{
    public GameObject shootBlue;
    public List<GameObject> reservaDisparoBlue;

    public float cadencia = 1.6f;
    private float tiempoActual;

    public int lvlShoot = 1;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        tiempoActual = cadencia;
        cadencia = 1.5f; //FindObjectOfType<PasarParametros>().cadenceShoot;
    }

    // Update is called once per frame
    void Update()
    {
        tiempoActual -= Time.deltaTime;

        if (tiempoActual <= 0)
        {
            tiempoActual = cadencia;
            if (lvlShoot >=1 && lvlShoot <=5)generarDisparos(1, 0, 0);
            if (lvlShoot >= 6 && lvlShoot <= 10) generarDisparos(2, 0.2f, 0.4f);
            if (lvlShoot >= 11) generarDisparos(3, 0.4f, 0.4f);
        }
    }

    //Generar
    public void ifRellenaReserva(int cantidad) //Comprueba si hay suficientes disparos en la reserva y los crea si es necesario
    {
        if (reservaDisparoBlue.Count < cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                GameObject instance = Instantiate(shootBlue);
                instance.SetActive(false);
                reservaDisparoBlue.Add(instance);
            }
        }
    }

    public void generarDisparos(int cantidad, float altura, float separacion)
    {
        ifRellenaReserva(cantidad);
        int totalReserva = reservaDisparoBlue.Count - 1;
        for (int i = 0; i < cantidad; i++)
        {

            Vector3 SpawnPosition = new Vector3(transform.position.x, (transform.position.y + altura), transform.position.z);
            reservaDisparoBlue[totalReserva - i].transform.position = SpawnPosition;
            reservaDisparoBlue[totalReserva - i].SetActive(true);
            //reservaDisparoBlue[totalReserva - i].GetComponent<ShootBlue>().impacto= false;
            reservaDisparoBlue.RemoveAt(totalReserva - i);
            altura -= separacion;

        }
        audioSource.Play();
    }

    //UTILIDADES

    public void desactivarDisparoBlue(GameObject disparoBlue) //Usada para descativar disparos (los propios disparos llaman a esta función desde sus scripts)
    {
        disparoBlue.SetActive(false);
        disparoBlue.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        reservaDisparoBlue.Add(disparoBlue);
    }
}
