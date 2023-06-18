using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RojoEnemyControl : MonoBehaviour
{
    Rigidbody2D cuerpo;
    public float velocidad = -200;

    private RojoGenerador rojoGenerador;
    private float vida = 100;
    private float vidaMaxima = 100;
    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        rojoGenerador = FindObjectOfType<RojoGenerador>();
        vida = vidaMaxima;
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -11)
        {
            desactivar();
        }
    }

    private void FixedUpdate()
    {
        cuerpo.velocity = new Vector2(velocidad * Time.deltaTime , 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Score>().killEnemy("rojo");
            FindObjectOfType<ExplosionCreate>().GenerarExplosion(transform);
            desactivar();
        }

        if (collision.CompareTag("shootBlue"))
        {
            vida -= collision.gameObject.GetComponent<ShootBlue>().dmg;
        }

        if ( vida <= 0)
        {
            FindObjectOfType<Score>().killEnemy("rojo");
            FindObjectOfType<ExplosionCreate>().GenerarExplosion(transform);
            desactivar();
        }

    }

    private void desactivar()
    {
        vida = vidaMaxima;
        rojoGenerador.desactivarEnemigoRojo(gameObject);
    }

    public void modificarVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }

}
