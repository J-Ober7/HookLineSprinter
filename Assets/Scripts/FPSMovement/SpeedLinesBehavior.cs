using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinesBehavior : MonoBehaviour
{
    public Rigidbody rb;
    private ParticleSystem ps;
    private PlayerMovement pm;

    private AudioSource aud;

    private GameObject LevelManager;


    // Start is called before the first frame update
    void Start()
    {
        pm = rb.gameObject.GetComponent<PlayerMovement>();
        ps = GetComponent<ParticleSystem>();
        aud = GetComponent<AudioSource>();
        LevelManager = GameObject.FindGameObjectWithTag("LevelManager");
    }

    // Update is called once per frame
    void Update()
    {
        SpeedLines();
        WindRush();
    }

    void SpeedLines()
    {
        // calculate alpha proportional to the speed
        float alpha = (rb.velocity.magnitude / pm.maxMaxSpeed);
        if(Time.timeScale == 0)
        {
            alpha = 0;
        }
        var main = ps.main;
        main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, alpha);
    }

    void WindRush()
    {
        float vol = rb.velocity.magnitude / (pm.maxMaxSpeed * 10);
        if(Time.timeScale == 0)
        {
            vol = 0;
        }
        aud.volume = vol;
    }
}
