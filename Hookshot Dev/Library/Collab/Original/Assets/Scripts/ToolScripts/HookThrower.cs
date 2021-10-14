using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is given to our fishing rod tip and it handles the firing, 
 * letting go and reeling in of the fishing pole, it also validates the grapple
 */
public class HookThrower : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public float reelInSpeed;
    public float hookFlySpeed = 100;
    public float springMinDistance;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, guncamera, player;
    public float maxDistance = 100f;
    SpringJoint joint;
    public float maxSpringForce = 10f;
    public GameObject hook;
    public Vector3 shotHookRotation;
    private Quaternion startHookRotation;
    private Vector3 origHookPos;

    public float Hookjump_Min_Time;

    public GameObject fishingRod;

    public bool isHooking = false;

    public GameObject hookedTo;

    public float hookLifeTime = 0;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        origHookPos = hook.transform.localPosition;
        hookLifeTime = 0;
    }

    private void Start()
    {
        startHookRotation = hook.transform.rotation;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if(IsGrappling())
        {
            hookLifeTime += Time.deltaTime;
            ReelInOverTime();
        }

    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        // Send out a raycast to see if what we're looking at is grapplable
        RaycastHit hit;
        if (Physics.Raycast(guncamera.position, guncamera.forward, out hit, maxDistance, whatIsGrappleable) && !hit.collider.isTrigger)
        {
            isHooking = true;
            grapplePoint = hit.point;
            hookedTo = hit.collider.gameObject;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = springMinDistance;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    // If you're attached to something, you will reel in over time as dictated by the reel speed
    // BUG: its difficult to change the reel in speed as we rely on the spring tension to create that speed
    void ReelInOverTime()
    {
        joint.maxDistance = Mathf.Lerp(joint.maxDistance, joint.minDistance, Time.deltaTime * reelInSpeed);
        joint.spring = Mathf.Lerp(joint.spring, maxSpringForce, Time.deltaTime * 2f);
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        if (isHooking)
        {
            isHooking = false;
            hook.transform.localPosition = origHookPos;
            fishingRod.transform.localRotation = startHookRotation;
            lr.positionCount = 0;
            Destroy(joint);
            hookLifeTime = 0;
        }
    }

    /// <summary>
    /// Call whenever the player jumps while hooking to detach the grapple
    /// </summary>
    public void HookJumpRelease()
    {
        StopGrapple();

    }

    public bool ReadyToHookJump()
    {
        return isHooking && (hookLifeTime > Hookjump_Min_Time);
    }

    private Vector3 currentGrapplePosition;

    // Draws our fishing line to our hook, as well as rotate the rod to face our hook
    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.MoveTowards(currentGrapplePosition, grapplePoint, Time.deltaTime * hookFlySpeed);
        hook.transform.position = currentGrapplePosition;

        fishingRod.transform.LookAt(hook.transform);
        fishingRod.transform.Rotate(new Vector3(90f, 0, 0));



        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
