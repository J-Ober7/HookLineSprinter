using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{

    public GameObject breakingPieces;
    public ParticleSystem breakingParticles;
    public float breakingForce;
    public float explosionRadius;

    public int numHitsToBreak = 1;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Projectile"))
        {
            numHitsToBreak--;
        }

        if(numHitsToBreak <= 0)
        {
            BreakDestructible();
        }
    }

    private void BreakDestructible()
    {
        if (breakingParticles != null && !breakingParticles.isPlaying)
        {
            breakingParticles.Play();
        }

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        GameObject pieces = Instantiate(breakingPieces, transform.position, transform.rotation);
        pieces.SetActive(true);
        Rigidbody[] rb = pieces.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody r in rb)
        {
            r.AddExplosionForce(breakingForce, rb[17].transform.position, explosionRadius);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {


    }
}
