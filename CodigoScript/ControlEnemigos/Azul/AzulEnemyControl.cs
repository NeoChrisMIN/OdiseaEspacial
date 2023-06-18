using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzulEnemyControl : MonoBehaviour
{
    Rigidbody2D cuerpo;
    public float velocidadHaciaDelante = -300;
    public float velocidadMarchaAtras = 100;
    public float movimientoActual = 0;
    public bool cambioDeSentido = false;

    private float cadenciadDisparo = 1.3f;
    private float tiempoActual;

    private AzulGenerador azulGenerador;
    private GeneradorShootRed generadorShootRed;
    private float vida = 50;
    private float vidaMaxima = 50;
    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        azulGenerador = FindObjectOfType<AzulGenerador>();
        generadorShootRed = FindObjectOfType<GeneradorShootRed>();
        vida = vidaMaxima;
        cambioDeSentido = false;
        movimientoActual = velocidadHaciaDelante;
        tiempoActual = cadenciadDisparo;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -11 || (transform.position.x > 9.30 && cambioDeSentido))
        {
            desactivar();
        }

        if (movimientoActual > 0)
        {
            tiempoActual -= Time.deltaTime;

            if (tiempoActual <= 0)
            {
                tiempoActual = cadenciadDisparo;
                generadorShootRed.generarDisparos(1, 0, 0,transform);
            }
        }

    }
    private void FixedUpdate()
    {
        
        if (transform.position.x <= 0 && cambioDeSentido== false)
        {
            movimientoActual = velocidadMarchaAtras;
            cambioDeSentido = true;
        }
        
        cuerpo.velocity = new Vector2(movimientoActual * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<Score>().killEnemy("azul");
            FindObjectOfType<ExplosionCreate>().GenerarExplosion(transform);
            desactivar();
        }

        if (collision.CompareTag("shootBlue"))
        {
            vida -= collision.gameObject.GetComponent<ShootBlue>().dmg;
        }

        if (vida <= 0)
        {
            FindObjectOfType<Score>().killEnemy("azul");
            FindObjectOfType<ExplosionCreate>().GenerarExplosion(transform);
            desactivar();
        }

    }

    private void desactivar()
    {
        tiempoActual = cadenciadDisparo;
        vida = vidaMaxima;
        movimientoActual = velocidadHaciaDelante;
        cambioDeSentido=false;
        azulGenerador.desactivarEnemigoAzul(gameObject);
    }

    public void modificarVelocidad(float nuevaVelocidadHaciaDelante, float nuevaVelocidadHaciaAtras)
    {
        velocidadHaciaDelante = nuevaVelocidadHaciaDelante;
        velocidadMarchaAtras = nuevaVelocidadHaciaAtras;
    }
}
