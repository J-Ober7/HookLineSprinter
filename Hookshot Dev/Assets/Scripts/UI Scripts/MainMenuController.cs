using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public int demoIndex = 1;
    public void LoadDemo()
    {
        SceneManager.LoadScene(demoIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
