using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPantalla : MonoBehaviour
{
    private string emailMejorJugador = "anonimo";
    public Text txtEmailMejorJugador;

    private int scoreMejorJugador = 0;
    public Text txtScoreMejorJugador;

    private string emailJugador = "anonimo";
    public Text txtemailJugador;

    private int scoreActual = 0;
    public Text txtScoreActual;

    private int tuMejorScore = 0;
    public Text txtTuMejorScore;
    

    // Start is called before the first frame update
    void Start()
    { 
        FindObjectOfType<FireBase>().getRecord(OnRecordReceived);
        txtemailJugador.text = (emailJugador = FindObjectOfType<PasarParametros>().emailJugador);
        scoreActual = FindObjectOfType<PasarParametros>().scoreLogrado;
        txtScoreActual.text = scoreActual.ToString();
        FindObjectOfType<FireBase>().getBestRecord(OnBestRecordReceived);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pantallaPrincipal()
    {
        SceneManager.LoadScene(0);
    }

    public void SalirJuego()
    {
        Application.Quit();
    }

    private void OnRecordReceived(int record)
    {
        if (!FindObjectOfType<FireBase>().CheckInternetConnection()) return;

        if (scoreActual > record ||record == -1)
        {
            tuMejorScore = scoreActual;
            txtTuMejorScore.text = tuMejorScore.ToString();
            FindObjectOfType<FireBase>().addRecord(tuMejorScore);
        }
        else
        {
            tuMejorScore = record;
            txtTuMejorScore.text = tuMejorScore.ToString();
        }
    }

    private void OnBestRecordReceived(int record,string email)
    { //emailMejorJugador   scoreMejorJugador
        if (!FindObjectOfType<FireBase>().CheckInternetConnection()) return;

        if (scoreActual > record || record == -1)
        {
            scoreMejorJugador = scoreActual;
            txtScoreMejorJugador.text = scoreMejorJugador.ToString();

            emailMejorJugador = emailJugador;
            txtEmailMejorJugador.text = emailMejorJugador;
            FindObjectOfType<FireBase>().addBestRecord(scoreActual);
        }
        else
        {
            scoreMejorJugador = record;
            txtScoreMejorJugador.text = scoreMejorJugador.ToString();

            emailMejorJugador = email;
            txtEmailMejorJugador.text = emailMejorJugador;
        }
    }
}
