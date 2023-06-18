using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBlue : MonoBehaviour
{
    Rigidbody2D cuerpo;

    public float velocidad = 300f;

    public float dmg = 50;

    private GeneradorShootBlue generadorShootBlue;

    //public bool impacto;

    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        generadorShootBlue = FindObjectOfType<GeneradorShootBlue>();
        //impacto= false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 11)
        {
            generadorShootBlue.desactivarDisparoBlue(gameObject);
        }
    }

    private void FixedUpdate()
    {
        cuerpo.velocity = new Vector2(velocidad * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy") /*&& impacto==false*/)
        {
            //impacto = true;
            //GetComponent<BoxCollider2D>().enabled = false;
            generadorShootBlue.desactivarDisparoBlue(gameObject);
        }
    }
}
