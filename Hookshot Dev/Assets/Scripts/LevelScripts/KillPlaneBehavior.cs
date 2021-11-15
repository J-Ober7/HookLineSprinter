using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is basically just for testing but if the player hits the thing this is attached to,
 * they will be teleported back to the beginning, if it's not the player, we destory the object to save on performance
 */
public class KillPlaneBehavior : MonoBehaviour
{
    public Transform checkpoint;
    GameObject player;
    Rigidbody rb;
    public HookThrower hook;
    public bool fastRespawn;
    public GameObject deathScreen;
    private Collider playerCollider;

    private bool gamePaused;
    private void Start()
    {
        deathScreen.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        checkpoint = player.transform;
        rb = player.transform.GetChild(1).GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (gamePaused)
        {
            if (Input.GetKey(KeyCode.R))
            {
                gamePaused = false;
                Time.timeScale = 1;
                playerReset(playerCollider);  
                deathScreen.SetActive(false);
            }
        }
    }

    public void setCheckpoint(Transform newPoint)
    {
        checkpoint = newPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.CompareTag("Player"))
        {
            if (!gamePaused)
            {
                if (fastRespawn)
                {
                    playerReset(other);
                }
                else
                {
                    playerCollider = other;
                    gamePaused = true;
                    Time.timeScale = 0;
                    deathScreen.SetActive(true);
                }
            }
            
        }
        else
        {
            Destroy(other.transform.parent.gameObject);
        }
    }

    private void playerReset(Collider player)
    {
        player.transform.position = checkpoint.transform.position;
        rb.velocity = Vector3.zero;
        hook.HookJumpRelease();
    }
}
