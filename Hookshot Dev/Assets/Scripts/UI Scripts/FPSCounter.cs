using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FPSCounter : MonoBehaviour
{
    public int targetFrames = 60;
    TMP_Text fpsDisplay;
    int framesPassed = 0;
    float fpsTotal = 0f;

    // Start is called before the first frame update
    void Start()
    {
        fpsDisplay = GetComponent<TMP_Text>();
        Application.targetFrameRate = targetFrames;
    }

    // Update is called once per frame
    void Update()
    {
        int fps = (int)(1 / Time.unscaledDeltaTime);
        fpsTotal += fps;
        framesPassed++;
        fpsDisplay.SetText("" + Mathf.Round(fpsTotal/framesPassed) + " FPS");
        if(framesPassed > 120)
        {
            fpsTotal = 0;
            framesPassed = 0;
        }
    }
}
