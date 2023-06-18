using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private float tiempoTranscurrido = 0f;
    private int enemigosRojos = 0;
    private int enemigosVerdes = 0;
    private int enemigosAzules = 0;

    // Start is called before the first frame update
    void Start()
    {
        tiempoTranscurrido = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;
    }


    public int score()
    {
        float scoreTotal = 0;
        scoreTotal += tiempoTranscurrido;

        scoreTotal += enemigosRojos * 30;
        scoreTotal += enemigosVerdes * 25;
        scoreTotal += enemigosAzules * 50;

        return (int)scoreTotal;
    }

    public void killEnemy(string enemy)
    {
        if (enemy == "rojo")enemigosRojos++;
        if (enemy == "verde") enemigosVerdes++;
        if (enemy == "azul") enemigosAzules++;
    }

}
