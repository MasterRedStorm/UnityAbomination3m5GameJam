using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyTimer : MonoBehaviour
{
    private Text timerLabel;
    private GameManager gameManager;

    public float time;
    
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        timerLabel = GameObject.Find("Timer").GetComponent<Text>();
    }

    void Update()
    {

        if (gameManager.gameIsRunning)
        {
            time += Time.deltaTime;
            var minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
            var seconds = time % 60;//Use the euclidean division for the seconds.
            var fraction = (time * 100) % 100;

            //update the label value
            timerLabel.text = string.Format("Time {0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
        }
    }
}