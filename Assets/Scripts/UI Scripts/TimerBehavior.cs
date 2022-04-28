using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerBehavior : MonoBehaviour
{
    public static float timer;
    public static bool timeStarted = false;
    private TMP_Text text;

    private float minutes = 0;
    private float seconds = 0;
    private float miliseconds = 0;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        timeStarted = true;
    }

    void Update()
    {
        if (timeStarted)
        {

            if (miliseconds > 99)
            {
                if (seconds >= 59)
                {
                    minutes++;
                    seconds = 0;
                }
                else
                {
                    seconds++;
                }

                miliseconds = 0;
            }

            miliseconds += Time.deltaTime * 100;
        }


        text.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, (int)miliseconds);
    }

    public static void StopTimer()
    {
        timeStarted = false;
    }

    public string GetTimerText()
    {
        string result = text.text;
        return result;
    }
}
