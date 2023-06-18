using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIdelJuego : MonoBehaviour
{
    public Text emailPlayer;
    public Text score;
    public Text txtvida;
    public Text txtLvlShoot;
    public Text txtCadenceShootPlayer;

    public int lvlShoot = 1;
    public float cadenceShootPlayer = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        cadenceShootPlayer = FindObjectOfType<GeneradorShootBlue>().cadencia;
        txtvida.text = "x "+FindObjectOfType<PasarParametros>().vidasTotales.ToString();
        emailPlayer.text = FindObjectOfType<PasarParametros>().emailJugador;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
       score.text = FindObjectOfType<Score>().score().ToString();
    }

    public void actualizaVidas(int vidasReales)
    {
        txtvida.text = "x " + vidasReales.ToString();
    }
    public void actualizaLvlShoot(int lvlShootReales)
    {
        txtLvlShoot.text = lvlShootReales.ToString();
    }
    public void actualizaCadenceShootPlayer(float cadenceShootPlayerReales)
    {
        txtCadenceShootPlayer.text = cadenceShootPlayerReales.ToString() + " s";
    }
}
