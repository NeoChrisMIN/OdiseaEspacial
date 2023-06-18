using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRed : MonoBehaviour
{
    Rigidbody2D cuerpo;

    public float velocidad = -300f;

    //public float dmg = 50;

    private GeneradorShootRed generadorShootRed;

    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        generadorShootRed = FindObjectOfType<GeneradorShootRed>();
        //impacto= false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -11)
        {
            generadorShootRed.desactivarDisparoRed(gameObject);

        }
    }

    private void FixedUpdate()
    {
        cuerpo.velocity = new Vector2(velocidad * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") /*&& impacto==false*/)
        {
            //impacto = true;
            //GetComponent<BoxCollider2D>().enabled = false;
            generadorShootRed.desactivarDisparoRed(gameObject);
        }
    }
}
