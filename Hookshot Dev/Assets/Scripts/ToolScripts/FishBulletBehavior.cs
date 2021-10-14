using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles the basic despawning of the projectiles we fire from our gun,
 * because too many projectiles can mess up a lot of stuff
 */

public class FishBulletBehavior : MonoBehaviour
{
    // The time, in seconds, it takes our projectile to despawn
    public float despawnTime = 3;
    
    // The maximum number of fish that can exist at one time
    public float maxFishOut;


    // Start is called before the first frame update
    void Start()
    {
        // If we exceed the max amount of fish we can have, then delete the oldest fish
        // Otherwise, delete the fish after a certain number of seconds
        if(transform.parent.childCount > maxFishOut)
        {
            Destroy(transform.parent.transform.GetChild(0).gameObject);
        }
        else
        {
            Destroy(gameObject, despawnTime);
        }
    }
}
