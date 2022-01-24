using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDistanceMarker : MonoBehaviour
{

    public Color startcolor;
    // The color that the reticle turns when you can hook onto the thing you're looking at
    public Color hookColor;
    private float distance;
    public float distanceToCheckPast = 25;
    public HookThrower ht;
    private float maxDistance;

    // This should be updated when it changes in the HookThrower
    public LayerMask whatIsGrappleable;
    public Image leftSideArrow;
    public Image rightSideArrow;
    public Transform cam;




    // Start is called before the first frame update
    void Start()
    {
        distance = ht.maxDistance;
        maxDistance = distance + distanceToCheckPast;
        whatIsGrappleable = ht.whatIsGrappleable;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(cam.position, cam.forward,color:Color.red ,distance);
        if (Physics.SphereCast(cam.position, 1f, cam.forward, out hit, maxDistance, whatIsGrappleable) && !hit.collider.isTrigger)
        {
            float howFar = hit.distance;
            //Debug.Log(howFar);
            if (howFar > distance)
            {
                
                leftSideArrow.color = startcolor;
                rightSideArrow.color = startcolor;
                leftSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-15 - ((howFar - distance)), 0);
                rightSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(15 + ((howFar - distance)), 0);
            }
            else
            {
                leftSideArrow.color = hookColor;
                rightSideArrow.color = hookColor;
                leftSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-15, 0);
                rightSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(15, 0);
            }
        }
        else
        {
           
            leftSideArrow.color = startcolor;
            rightSideArrow.color = startcolor;
            leftSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-15 - distanceToCheckPast, 0);
            rightSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(15 + distanceToCheckPast, 0);
            
        }
    }
}
