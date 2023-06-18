using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerdeEnemyControl : MonoBehaviour
{
    Rigidbody2D cuerpo;
    public float velocidadX = -200f;
    public float velocidadY = -200f;

    private VerdeGenerador verdeGenerador;
    private float vida = 50;
    private float vidaMaxima = 50;
    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        verdeGenerador = FindObjectOfType<VerdeGenerador>();
        vida = vidaMaxima;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -11)
        {
            verdeGenerador.desactivarEnemigoVerde(gameObject);
        }
    }

    private void FixedUpdate()
    {
        cuerpo.velocity = new Vector2(velocidadX * Time.deltaTime, velocidadY * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("limite"))
        {
            cambioDireccion();
        }

        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Score>().killEnemy("verde");
            FindObjectOfType<ExplosionCreate>().GenerarExplosion(transform);
            desactivar();
        }

        if (collision.CompareTag("shootBlue"))
        {
            
            vida -= collision.gameObject.GetComponent<ShootBlue>().dmg;
        }

        if (vida <= 0)
        {
            FindObjectOfType<Score>().killEnemy("verde");
            FindObjectOfType<ExplosionCreate>().GenerarExplosion(transform);
            desactivar();
        }
    }

    public void cambioDireccion()
    {
        velocidadY -= velocidadY * 2;
    }

    private void desactivar()
    {
        vida = vidaMaxima;
        verdeGenerador.desactivarEnemigoVerde(gameObject);
    }

    public void modificarVelocidad(float nuevaVelocidadX, float nuevaVelocidadY)
    {
        velocidadX = nuevaVelocidadX;
        velocidadY = nuevaVelocidadY;
    }
}
