using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSoundHandler : MonoBehaviour
{
    public AudioClip stepSound;
    public AudioClip landSound;

    public float minPitchRange = 0.5f;
    public float maxPitchRange = 1.5f;
    public float walkSoundVar = 0.5f;


    public float moveThreshold = 0.5f;


    Rigidbody rb;
    AudioSource stepSource;
    PlayerMovement pm;

    bool playing;

    float prevZcomp;
    float prevXcomp;

    Vector3 prevPos;
    bool prevGrounded;


    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        pm = rb.gameObject.GetComponent<PlayerMovement>();
        stepSource = GetComponent<AudioSource>();
        playing = false;
        prevXcomp = rb.velocity.x;
        prevZcomp = rb.velocity.z;
        prevPos = new Vector3(rb.transform.position.x, transform.position.y, rb.transform.position.z);
        prevGrounded = pm.grounded;
    }

    private void Update()
    {
        if(prevGrounded != pm.grounded)
        {
            stepSource.PlayOneShot(landSound);
        }

        if (playing && pm.grounded)
        {
            prevPos = new Vector3(rb.transform.position.x, transform.position.y, rb.transform.position.z);
            stepSource.pitch = Random.Range(minPitchRange, maxPitchRange);
            stepSource.PlayOneShot(stepSound);
            playing = false;
        }

        /////////

        if (Mathf.Abs(rb.velocity.x) < 1
            && Mathf.Abs(rb.velocity.z) < 1 && pm.grounded)
        {
            prevPos = new Vector3(rb.transform.position.x, transform.position.y, rb.transform.position.z);
        }

        if (moveThreshold <= Vector3.Distance(prevPos, new Vector3(rb.transform.position.x, transform.position.y, rb.transform.position.z))
            || (prevXcomp >= 0 && rb.velocity.x < 0)
            || (prevXcomp <= 0 && rb.velocity.x > 0)
            || (prevZcomp <= 0 && rb.velocity.z > 0)
            || (prevZcomp >= 0 && rb.velocity.z < 0)
            && pm.grounded)
        {
            playing = true;
        }

        prevXcomp = rb.velocity.x;
        prevZcomp = rb.velocity.z;
        prevGrounded = pm.grounded;
    }

}
