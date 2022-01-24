using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairBehavior : MonoBehaviour
{
    public Color startcolor;
    // The color that the reticle turns when you can hook onto the thing you're looking at
    public Color hookColor;
    private float distance;
    public HookThrower ht;

    // This should be updated when it changes in the HookThrower
    public LayerMask whatIsGrappleable;
    private Image thisIm;
    public Transform cam;




    // Start is called before the first frame update
    void Start()
    {
        thisIm = GetComponent<Image>();
        distance = ht.maxDistance;
        whatIsGrappleable = ht.whatIsGrappleable;
        startcolor = thisIm.color;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.SphereCast(cam.position, 1f, cam.forward, out hit, distance, whatIsGrappleable) && !hit.collider.isTrigger)
        {
            thisIm.color = hookColor;
        }
        else
        {
            if(thisIm.color != startcolor)
            {
                thisIm.color = startcolor;
            }
        }
    }
}
