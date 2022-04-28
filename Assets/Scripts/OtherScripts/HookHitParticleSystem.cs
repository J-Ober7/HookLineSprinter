using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookHitParticleSystem : MonoBehaviour
{
    private HookThrower ht;
    public ParticleSystem sparks;

    // Start is called before the first frame update
    void Start()
    {
        ht = GetComponent<HookThrower>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitSparks(Transform location)
    {
        ParticleSystem oneShotSparks = Instantiate(sparks);
    }
}
