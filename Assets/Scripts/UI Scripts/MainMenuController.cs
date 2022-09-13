using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public int demoIndex = 1;

    public List<GameObject> whatToHideCredits;

    public GameObject creditsScreen;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Update()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadDemo()
    {
        SceneManager.LoadScene(demoIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowCredits(bool showCredits)
    {
        foreach(GameObject g in whatToHideCredits)
        {
            g.SetActive(!showCredits);
        }

        creditsScreen.SetActive(showCredits);
    }
}
