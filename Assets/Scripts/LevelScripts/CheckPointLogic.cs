using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointLogic : MonoBehaviour
{
    public KillPlaneBehavior killPlane;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent != null)
        {
            if (other.gameObject.transform.parent.CompareTag("Player"))
            {
                killPlane.setCheckpoint(gameObject.transform);
            }
        }
    }


}
