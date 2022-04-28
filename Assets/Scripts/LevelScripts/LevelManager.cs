using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public bool isPaused = false;
    public bool gameOver = false;
    public bool gameWon = false;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();

    }


    void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = true;
        }

    }

    void PlayerDeath()
    {
/*        player.transform.position = checkpoint.transform.position;
        rb.velocity = Vector3.zero;
        hook.HookJumpRelease();*/
    }
}
