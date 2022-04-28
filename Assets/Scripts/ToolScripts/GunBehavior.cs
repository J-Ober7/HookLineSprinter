using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles the firing of our fish
 */

public class GunBehavior : MonoBehaviour
{
    // This is the fish prefab: needs a collider and rigidbody to function but can be anything
    public GameObject bulletPrefab;
    public float bulletSpeed;

    // Where the fish will be shot from
    private GameObject gunTip;
    private GameObject fishLoadedModel;
    
    // The object that the fish will be packed under
    public Transform fishParent;
    private GameObject gunModel;

    private Rigidbody playerrb;
    private Vector3 gunAimedPos;

    public float gunDownTime = 2f;
    private float currGunTime = 0f;

    public AudioClip shootSFX;
    public GameObject shootPS;

    // Start is called before the first frame update
    void Start()
    {
        gunTip = transform.GetChild(0).gameObject;
        fishLoadedModel = transform.GetChild(2).gameObject;
        playerrb = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject.GetComponent<Rigidbody>();
        gunAimedPos = transform.localEulerAngles;
        gunModel = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // When you right click, shoot the fish out of the gun
        if(Input.GetButton("Fire2"))
        {
            AimFish();
        }
        else
        {
            currGunTime += Time.deltaTime;
            UnAimFish();
        }

        if(Input.GetButtonUp("Fire2"))
        {
            ShootFish();
        }

    }

    void AimFish()
    {
        fishLoadedModel.SetActive(true);
        transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, gunAimedPos, 1000 * Time.deltaTime);
        currGunTime = 0f;

    }

    void UnAimFish()
    {
        if (transform.localRotation.x != 30f && currGunTime >= gunDownTime)
        {
            Vector3 rotateVec = new Vector3(30f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, rotateVec, 500 * Time.deltaTime);
        }
    }

    // This makes the fish and puts it neatly under the "fishholder" object in the inspector
    // The fish are then given a random rotation and are fire at the speed that we declare in the direction the camera is facing
    void ShootFish()
    {
        GameObject firedBullet = Instantiate<GameObject>(bulletPrefab, gunTip.transform, false);
        firedBullet.transform.SetParent(fishParent);
        firedBullet.transform.LookAt(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100,100)));

        //spawn shot particle effect and sound effect
        AudioSource.PlayClipAtPoint(shootSFX, gunTip.transform.position);
        GameObject gunSmoke = Instantiate<GameObject>(shootPS, gunTip.transform.position, gunTip.transform.rotation);
        

        fishLoadedModel.SetActive(false);
        firedBullet.GetComponent<Rigidbody>().velocity = (Camera.main.transform.forward)* (playerrb.velocity.magnitude + bulletSpeed);
    }
}
