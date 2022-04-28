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
    public float distanceToCheckPast;
    public HookThrower ht;
    private float maxDistance;

    // This should be updated when it changes in the HookThrower
    public LayerMask whatIsGrappleable;
    public Image leftSideArrow;
    public Image rightSideArrow;
    public Image topSideArrow;
    public Image bottomSideArrow;
    public Image centerDot;
    public Transform cam;

    private Vector3 startPos;
    private Camera maincamera;
    private Vector3 screenHitPoint;
    private Vector3[] conePoints;

    private RectTransform transform;


    // Start is called before the first frame update
    void Start()
    {
        distance = ht.maxDistance;
        maxDistance = distance + distanceToCheckPast;
        whatIsGrappleable = ht.whatIsGrappleable;

        transform = GetComponent<RectTransform>();
        startPos = GetComponent<RectTransform>().anchoredPosition;
        maincamera = Camera.main;
        conePoints = ht.GetConePoints();
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();//ConeCastExtension.ConeCast(cam.position, 2f, cam.forward, maxDistance, 15f, ref hit, whatIsGrappleable)
        if (ConeCastExtension.ConeCastPoints(cam.position, ht.forgivenessIncrement/2, cam.forward, maxDistance, 15f, conePoints, ref hit, whatIsGrappleable) && !hit.collider.isTrigger)
        {
            float howFar = hit.distance;
            //Debug.Log(howFar);
            if (howFar > distance)
            {
                
                leftSideArrow.color = startcolor;
                rightSideArrow.color = startcolor;
                topSideArrow.color = startcolor;
                bottomSideArrow.color = startcolor;
                centerDot.color = startcolor;
                leftSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10 - ((howFar - distance)), 0);
                rightSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + ((howFar - distance)), 0);
                topSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 10 + ((howFar - distance)));
                bottomSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10 - ((howFar - distance)));
                
                
                screenHitPoint = maincamera.WorldToScreenPoint(hit.point);

                transform.position = screenHitPoint;


                Debug.Log("Just Shy: " + howFar);

            }
            else
            {
                leftSideArrow.color = hookColor;
                rightSideArrow.color = hookColor;
                topSideArrow.color = hookColor;
                bottomSideArrow.color = hookColor;
                centerDot.color = hookColor;
                leftSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10, 0);
                rightSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, 0);
                topSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 10);
                bottomSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10);

                // if out hit gets changed this will have to change too
                screenHitPoint = maincamera.WorldToScreenPoint(hit.point);

                transform.position = screenHitPoint;
                
            }
        }
        else
        {
           
            leftSideArrow.color = startcolor;
            rightSideArrow.color = startcolor;
            topSideArrow.color = startcolor;
            bottomSideArrow.color = startcolor;
            centerDot.color = startcolor;
            leftSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10 - distanceToCheckPast, 0);
            rightSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + distanceToCheckPast, 0);
            bottomSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10 - distanceToCheckPast);
            topSideArrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 10 + distanceToCheckPast);
            transform.anchoredPosition3D = startPos;



        }
    }
}
