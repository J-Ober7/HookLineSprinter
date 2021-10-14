using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is basically just for testing but if the player hits the thing this is attached to,
 * they will be teleported back to the beginning, if it's not the player, we destory the object to save on performance
 */
public class KillPlaneBehavior : MonoBehaviour
{
    Transform checkpoint;
    GameObject player;
    Rigidbody rb;
    public HookThrower hook;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        checkpoint = player.transform;
        rb = player.transform.GetChild(1).GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.CompareTag("Player"))
        {
            other.transform.position = checkpoint.transform.position;
            rb.velocity = Vector3.zero;
            hook.HookJumpRelease();
        }
        else
        {
            Destroy(other.transform.parent.gameObject);
        }
    }

}
