using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ExplosionCreate : MonoBehaviour
{

    public GameObject explosion;

    public List<GameObject> reservaExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ifRellenaReserva(int cantidad) //Comprueba si hay suficientes enemigos en la reserva y los crea si es necesario
    {
        if (reservaExplosion.Count < cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                GameObject instance = Instantiate(explosion);
                instance.SetActive(false);
                reservaExplosion.Add(instance);
            }
        }
    }

    public void GenerarExplosion(UnityEngine.Transform posicion)
    {
        int separacion = 0;
        ifRellenaReserva(1);

        int totalReserva = reservaExplosion.Count - 1;
        Vector3 SpawnPosition = new Vector3(posicion.position.x, (posicion.position.y) - separacion, posicion.position.z);
        reservaExplosion[totalReserva].transform.position = SpawnPosition;
        //FindObjectOfType<ControlExplosiones>().generar();
        reservaExplosion[totalReserva].SetActive(true);
        reservaExplosion.RemoveAt(totalReserva);
    }

    public void desactivarExplosion(GameObject explosion) //Usada para descativar enemigos (los propios enemigos llaman a esta función desde sus scripts)
    {
        explosion.SetActive(false);
        explosion.transform.position = new Vector3(10f, 0f, transform.position.z);
        reservaExplosion.Add(explosion);
    }


}
