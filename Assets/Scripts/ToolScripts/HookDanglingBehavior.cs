using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookDanglingBehavior : MonoBehaviour
{
    private HookThrower ht;
    public GameObject hook;
    public GameObject hookDangle;

    // Start is called before the first frame update
    void Start()
    {
        ht = GetComponent<HookThrower>();
        EnableHookDangle();
    }

    // Update is called once per frame
    void Update()
    {
        if(!ht.isHooking)
        {
            EnableHookDangle();
        }
        else
        {
            DisableHookDangle();
        }

    }

    void EnableHookDangle()
    {
        hook.SetActive(false);
        hookDangle.SetActive(true);
    }

    void DisableHookDangle()
    {
        hook.SetActive(true);
        hookDangle.SetActive(false);
    }
}
