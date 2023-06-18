using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JugadorControl : MonoBehaviour
{
    //------------------------------------
    float entradaX;
    float entradaY;
    float velocidad = 8;
    Rigidbody2D cuerpo;
    SpriteRenderer spriteUso;
    public Sprite spriteNormal;
    public Sprite spriteSubir;
    public Sprite spriteBajar;
    //------------------------------------

    float horizontalMove = 0;
    float verticalMove = 0;
    public float horizontalSpeed = 3;
    public float verticalSpeed = 3;
    public float runSpeed = 0;

    //------------------------------------
    public Joystick joystick;
    private Animator animator;
    //------------------------------------
    public GameObject escudo;
    private GameObject contenedorEscudo;
    public AudioSource sonidoPowerUp;


    public int vida;
    private bool inmortal;
    public float periodoDeInmortal;
    private GeneradorShootBlue generadorShootBlue;
    // Start is called before the first frame update
    void Start()
    {
        
        cuerpo = GetComponent < Rigidbody2D >();
        spriteUso = GetComponent < SpriteRenderer >();
        animator= GetComponent < Animator >();
        vida = FindObjectOfType<PasarParametros>().vidasTotales;
        inmortal = false;
        generadorShootBlue = GetComponent<GeneradorShootBlue>();
        //Debug.Log(FindObjectOfType<PasarParametros>().paco);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        verticalMove = joystick.Vertical * verticalSpeed;
        horizontalMove= joystick.Horizontal * horizontalSpeed;
        animator.SetFloat("ejeY", joystick.Vertical);
        transform.position += new Vector3(horizontalMove, verticalMove, 0) * Time.deltaTime * runSpeed;
        //Debug.Log("Y-" + verticalMove + "||  X-" + horizontalMove);*/
        movimientoTeclado();
    }

    private void movimientoTeclado() //no se deberia usar ya que el juego sera para moviles
    {
        entradaX = 0;
        entradaY = 0;

        if (Input.GetKey(KeyCode.LeftArrow)) entradaX -= 2.69f;
        if (Input.GetKey(KeyCode.RightArrow)) entradaX += 2.69f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            entradaY += 2.69f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            entradaY -= 2.69f;
        }
        else if (spriteUso != spriteNormal)
        {
            entradaY = 0;
        }

        //Debug.Log("Y-" + entradaY + "||  X-" + entradaX);
        animator.SetFloat("ejeY", entradaY);
        transform.position += new Vector3(entradaX, entradaY, 0) * Time.deltaTime * runSpeed;
        //cuerpo.velocity = new Vector2(entradaX * velocidad, entradaY * velocidad);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            if (!inmortal)
            {
                inmnortalidad();
            }

        }
        else if (collision.CompareTag("shootRed"))
        {
            if (!inmortal)
            {
                inmnortalidad();
            }
        }

        if (collision.CompareTag("powerUpVida"))
        {
            vida++;
            FindObjectOfType<UIdelJuego>().actualizaVidas(vida);
            sonidoPowerUp.Play();
        }
        if (collision.CompareTag("powerUpShoot"))
        {
            generadorShootBlue.lvlShoot++;
            if(generadorShootBlue.cadencia > 1f) generadorShootBlue.cadencia = generadorShootBlue.cadencia - 0.1f;
            FindObjectOfType<UIdelJuego>().actualizaLvlShoot(generadorShootBlue.lvlShoot);
            FindObjectOfType<UIdelJuego>().actualizaCadenceShootPlayer(generadorShootBlue.cadencia);
            sonidoPowerUp.Play();
        }

        if(vida <= 0){
            gameOver();
        }
    }

    private void gameOver()
    {
        FindObjectOfType<PasarParametros>().scoreLogrado = (int)FindObjectOfType<Score>().score();
        SceneManager.LoadScene(2);
    }

    private void inmnortalidad()
    {
        vida -= 1;
        inmortal = true;
        FindObjectOfType<UIdelJuego>().actualizaVidas(vida);
        contenedorEscudo = Instantiate(escudo, transform.position, Quaternion.identity);

        contenedorEscudo.transform.SetParent(transform, true);
        contenedorEscudo.transform.localScale = new Vector3(2f, 2f, 2f);

        Invoke("desactivarEscudo", periodoDeInmortal);
    }

    private void desactivarEscudo()
    {
        inmortal = false;
        Destroy(contenedorEscudo);
    }
}
