using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoParadax : MonoBehaviour
{
    private Transform tranform;
    private Rigidbody2D cuerpo;
    public float velocidad = 0.01f;
    public float intervalo = -15.85f;
    // Start is called before the first frame update
    void Start()
    {
        tranform = GetComponent<Transform>();
        cuerpo = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //18.10613
        if (tranform.localPosition.x > intervalo)
        { //-15.8f
            //cuerpo.velocity = new Vector3(velocidad, tranform.localPosition.y, tranform.localPosition.z);
            tranform.localPosition = new Vector3(tranform.localPosition.x - velocidad*Time.deltaTime, tranform.localPosition.y, tranform.localPosition.z);
        }
        else
        {// 2.323867
            tranform.localPosition = new Vector3(2.323867f, tranform.localPosition.y, tranform.localPosition.z);
        }
    }
}
