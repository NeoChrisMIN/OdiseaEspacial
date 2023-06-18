using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPowerUp : MonoBehaviour
{
    Rigidbody2D cuerpo;
    public float velocidad = -200;

    private GeneradorShootPowerUp generadorPowerUp;
    // Start is called before the first frame update
    void Start()
    {
        cuerpo = GetComponent<Rigidbody2D>();
        generadorPowerUp = FindObjectOfType<GeneradorShootPowerUp>();
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
        cuerpo.velocity = new Vector2(velocidad * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            desactivar();
        }

    }

    private void desactivar()
    {
        generadorPowerUp.desactivarPowerUp(gameObject);
    }

    public void modificarVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }
}
