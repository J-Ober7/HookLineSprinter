using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishLineHat : MonoBehaviour
{

    public float hatRotateSpeed = 20f;
    public float timeuntilwinscreen = 1f;
    public HookThrower ht;
    public GameObject winscreen;
    public GameObject winScreenTimerText;
    private LevelManager levelManager;
    public TimerBehavior tb;



    public bool levelWon = false;

    private Transform hatRotateTransform;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        hatRotateTransform = transform.GetChild(0);
        levelWon = false;
        winscreen.SetActive(false);
        levelManager.gameWon = false;
        Time.timeScale = 1;

    }

    private void Update()
    {
        if (levelWon)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {

                levelreset();
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("QUITTING");
                levelManager.isPaused = false;
                levelManager.gameWon = false;
                Time.timeScale = 1;
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            transform.Rotate(new Vector3(0, hatRotateSpeed * Time.deltaTime, 0));
        }

        if(levelWon)
        {
            if(timeuntilwinscreen >= 0)
            {
                timeuntilwinscreen -= Time.deltaTime;
            }
            else
            {
                levelManager.isPaused = true;
                levelManager.gameWon = true;

                Time.timeScale = 0;
                winScreenTimerText.GetComponent<TMP_Text>().text = "Time: " + tb.GetTimerText();
                tb.gameObject.SetActive(false);
                winscreen.SetActive(true);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!levelWon && (collision.gameObject.CompareTag("Hook") || collision.gameObject.transform.root.CompareTag("Player"))) {
            FinishLevel();
            levelWon = true;
        }


    }

    // call this function when you want the level to be beat
    void FinishLevel()
    {
        ht.StopGrapple();
        gameObject.layer = LayerMask.NameToLayer("Ungrapplable");
        GetComponent<ShakeObject>().enabled = true;
        TimerBehavior.StopTimer();
        Debug.Log("LEVEL COMPLETE!");
        GetComponent<AudioSource>().Play();
    }

    private void levelreset()
    {

        levelManager.isPaused = false;
        levelManager.gameWon = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

}
