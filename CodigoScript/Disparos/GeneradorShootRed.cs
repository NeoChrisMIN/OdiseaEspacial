using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorShootRed : MonoBehaviour
{
    public GameObject shootRed;
    public List<GameObject> reservaDisparoBlue;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generar
    private void ifRellenaReserva(int cantidad) //Comprueba si hay suficientes disparos en la reserva y los crea si es necesario
    {
        if (reservaDisparoBlue.Count < cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                GameObject instance = Instantiate(shootRed);
                instance.SetActive(false);
                reservaDisparoBlue.Add(instance);
            }
        }
    }

    public void generarDisparos(int cantidad, float altura, float separacion , Transform posicionEnemy)
    {
        ifRellenaReserva(cantidad);
        int totalReserva = reservaDisparoBlue.Count - 1;
        for (int i = 0; i < cantidad; i++)
        {

            Vector3 SpawnPosition = new Vector3(posicionEnemy.position.x, (posicionEnemy.position.y + altura), posicionEnemy.position.z);
            reservaDisparoBlue[totalReserva - i].transform.position = SpawnPosition;
            reservaDisparoBlue[totalReserva - i].SetActive(true);
            //reservaDisparoBlue[totalReserva - i].GetComponent<ShootBlue>().impacto= false;
            reservaDisparoBlue.RemoveAt(totalReserva - i);
            altura -= separacion;
        }
    }

    //UTILIDADES

    public void desactivarDisparoRed(GameObject disparoRed) //Usada para descativar disparos (los propios disparos llaman a esta función desde sus scripts)
    {
        disparoRed.SetActive(false);
        disparoRed.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        reservaDisparoBlue.Add(disparoRed);
    }
}
