using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This is basically just for testing but if the player hits the thing this is attached to,
 * they will be teleported back to the beginning, if it's not the player, we destory the object to save on performance
 */
public class KillPlaneBehavior : MonoBehaviour
{
    public static bool isRespawning;

    public Transform checkpoint;
    public bool fastRespawn;
    public GameObject deathScreen;

    GameObject player;
    Rigidbody rb;
    public HookThrower hook;
    private Collider playerCollider;
    private Transform playerCamera;

    public ParticleSystem splashEffect;
    public ParticleSystem waterFoamEffect;
    public GameObject particleHolder;

    private List<GameObject> itemsInTheWater;
    private List<ParticleSystem> waterPaticlesList;

    private LevelManager levelManager;

    private bool gamePaused;

    public AudioClip splash;
    public AudioClip splashNotPlayer;

    private void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        deathScreen.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = player.GetComponent<PlayerReference>().player_camera_holder;
        checkpoint = player.transform;
        rb = player.transform.GetChild(1).GetComponent<Rigidbody>();

        itemsInTheWater = new List<GameObject>();
        waterPaticlesList = new List<ParticleSystem>();
    }

    private void Update()
    {
        if ((levelManager.gameOver)) //&& !levelManager.gameWon)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                levelManager.gameOver = false;
                levelManager.isPaused = false;
                Time.timeScale = 1;
                playerReset(playerCollider);  
                deathScreen.SetActive(false);
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {

                levelManager.gameOver = false;
                levelManager.isPaused = false;
                Time.timeScale = 1;
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            isRespawning = false;
            if(waterPaticlesList.Count != 0)
            {
                foreach(ParticleSystem ps in waterPaticlesList)
                {
                    if(ps != null)
                    {
                        ps.transform.position = new Vector3(ps.transform.position.x, transform.position.y, ps.transform.position.z);
                        ps.transform.rotation = transform.rotation;

                    }
                }
            }
        }
    }

    public void setCheckpoint(Transform newPoint)
    {
        checkpoint = newPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        ParticleSystem ps = Instantiate<ParticleSystem>(splashEffect, particleHolder.transform);
        ps.transform.position = other.transform.position;
        ps.transform.rotation = transform.rotation;

        if (other.gameObject.transform.parent != null)
        {
            if (other.gameObject.transform.CompareTag("Achilles Head"))
            {
                if (!gamePaused)
                {
                    if (fastRespawn)
                    {
                        playerReset(other.transform.parent.GetComponent<Collider>());
                    }
                    else
                    {
                        playerCollider = other.transform.parent.GetComponent<Collider>();
                        rb.velocity = Vector3.zero;
                        levelManager.gameOver = true;
                        levelManager.isPaused = true;
                        Time.timeScale = 0;
                        deathScreen.SetActive(true);

                        AudioSource.PlayClipAtPoint(splash, other.transform.parent.position);
                    }
                }

            }
            else if (other.gameObject.transform.parent.CompareTag("Projectile"))
            {
                // spawn splash particles and despawn the thing that hit the water
                Destroy(other.transform.parent.gameObject, 0.5f);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {

        if (!itemsInTheWater.Contains(other.gameObject) && !other.gameObject.layer.Equals(6) && !other.gameObject.layer.Equals(9))
        {
            itemsInTheWater.Add(other.gameObject);

            ParticleSystem ps = Instantiate<ParticleSystem>(waterFoamEffect, other.gameObject.transform);

            ps.transform.position = new Vector3(ps.transform.position.x, transform.position.y, ps.transform.position.z);
            ps.transform.rotation = Quaternion.identity;


            waterPaticlesList.Add(ps);
            AudioSource.PlayClipAtPoint(splashNotPlayer, ps.transform.position);
        }

        if (!itemsInTheWater.Contains(other.gameObject) && other.gameObject.CompareTag("Projectile"))
        {
            itemsInTheWater.Add(other.gameObject);

            ParticleSystem ps = Instantiate<ParticleSystem>(waterFoamEffect, other.gameObject.transform);

            ps.transform.position = new Vector3(ps.transform.position.x, transform.position.y, ps.transform.position.z);
            ps.transform.rotation = Quaternion.identity;


            waterPaticlesList.Add(ps);
            AudioSource.PlayClipAtPoint(splashNotPlayer, ps.transform.position);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if(itemsInTheWater.Contains(other.gameObject))
        {
            itemsInTheWater.Remove(other.gameObject);

            ParticleSystem ps;

            if(ps = other.gameObject.GetComponentInChildren<ParticleSystem>())
            {
                waterPaticlesList.Remove(ps);
            }


            Destroy(ps.gameObject);
        }
    }


    private void playerReset(Collider player)
    {
        player.transform.position = checkpoint.transform.position;
        playerCamera.transform.localRotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        hook.HookJumpRelease();
        isRespawning = true;
    }
}
