using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlExplosiones : MonoBehaviour
{
    public float duracion = 0.417f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        Invoke("desactivarExplosion", duracion);
    }

    private void desactivarExplosion()
    {
        FindObjectOfType<ExplosionCreate>().desactivarExplosion(gameObject);
    }
}
