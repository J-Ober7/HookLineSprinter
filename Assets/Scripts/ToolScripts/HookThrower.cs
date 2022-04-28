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
    public LayerMask whatIsGrappleable;
    public Transform gunTip, guncamera, player; 
    SpringJoint joint;
    public GameObject hook;

    public float reelInSpeed;
    public float hookFlySpeed = 100;
    public float springMinDistance;
    public float maxDistance = 100f;
    public float maxSpringForce = 10f;
    public float forgivenessRadius;
    public float forgivenessIncrement;

    private Vector3[] conePoints;

    public Vector3 shotHookRotation;
    private Quaternion startHookRotation;
    private Vector3 origHookPos;
    private Vector3 origHookParentPos;

    public float Hookjump_Min_Time;

    public GameObject fishingRod;
    public GameObject reel;
    public float reelSpeed = 5f;

    public bool isHooking = false;

    public GameObject hookedTo;

    public float hookLifeTime = 0;

    public PhysicMaterial playerPhysMat;

    // this is to handle moving objects somewhat?
    public Transform hookParent;

    public AudioClip hookLand;

    private bool hookLanded;


    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        origHookPos = hook.transform.localPosition;
        origHookParentPos = hookParent.localPosition;
        hookLifeTime = 0;
        hookLanded = false;
        conePoints = ConeCastExtension.GenerateConePoints(forgivenessRadius, forgivenessIncrement);
        Debug.Log(conePoints.Length);
    }

    private void Start()
    {
        startHookRotation = hook.transform.rotation;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isHooking)
            {
                StartGrapple();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if(isHooking)
        {
            hookLifeTime += Time.deltaTime;

            ReelInOverTime();

            if (hookedTo == null)
            {
                StopGrapple();
            }
        }

    }

    //Called after Update
    void LateUpdate()
    {
        if (isHooking)
        {
            DrawRope();
        }
    }

   
    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        // Send out a raycast to see if what we're looking at is grapplable
        RaycastHit hit = new RaycastHit(); // (ConeCastExtension.ConeCast(guncamera.position, 2f, guncamera.forward, maxDistance, 15f, ref hit, whatIsGrappleable)
        if (ConeCastExtension.ConeCastPoints(guncamera.position, forgivenessIncrement/2, guncamera.forward, maxDistance, 15f, conePoints, ref hit, whatIsGrappleable) && !hit.collider.isTrigger)
        {
            isHooking = true;
            grapplePoint = hit.point;
            hookedTo = hit.collider.gameObject;
            hookParent.transform.position = hookedTo.transform.position;
            hookParent.transform.rotation = hookedTo.transform.rotation;

            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;


            // offset the hook's position from its parent
            hook.transform.position = grapplePoint;


            player.GetComponent<CapsuleCollider>().material.bounciness = 0.4f;

            

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = maxDistance;//distanceFromPoint * 0.8f;
            joint.minDistance = springMinDistance;

            //Adjust these values to fit the game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            hook.layer = LayerMask.NameToLayer("Ungrapplable");
            hook.transform.parent.gameObject.layer = LayerMask.NameToLayer("Ungrapplable");
            hook.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Ungrapplable");
            gameObject.layer = LayerMask.NameToLayer("Ungrapplable");
            transform.parent.gameObject.layer = LayerMask.NameToLayer("Ungrapplable");
            //reel.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Ungrapplable");

            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
            hookLanded = false;
        }
    }

    // If you're attached to something, you will reel in over time as dictated by the reel speed
    // BUG: its difficult to change the reel in speed as we rely on the spring tension to create that speed
    void ReelInOverTime()
    {
        AudioSource reelAudio = reel.GetComponent<AudioSource>();

        joint.maxDistance = Mathf.Lerp(joint.maxDistance, joint.minDistance, Time.deltaTime * reelInSpeed);
        joint.spring = Mathf.Lerp(joint.spring, maxSpringForce, Time.deltaTime * 2f);

        // "reeling in" animation
        if(Vector3.Distance(player.transform.position, joint.connectedAnchor) >= joint.maxDistance)
        {
            reel.transform.Rotate(Vector3.up, reelSpeed);

            // Play fishing reel sound here
            if(hookLanded && !reelAudio.isPlaying)
            {
                reelAudio.Play();
            }
        }
        else
        {
            //stop audio
            reelAudio.Stop();
        }

        // when you are hooked to something
        // and the something has moved
        if (hookedTo.transform.position != hookParent.transform.position
            || hookedTo.transform.rotation != hookParent.transform.rotation)
        {

            //grapplePoint = (hookedTo.transform.position - hook.transform.position);

            joint.connectedAnchor = hook.transform.position;
            

            // update the parent transform

            hookParent.transform.position = hookedTo.transform.position;
            hookParent.transform.rotation = hookedTo.transform.rotation;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    public void StopGrapple()
    {
        if (isHooking)
        {
            isHooking = false;
            hookParent.transform.position = transform.position;
            hook.transform.localPosition = origHookPos;
            currentGrapplePosition = origHookPos;

            fishingRod.transform.localRotation = startHookRotation;
            lr.positionCount = 0;
            Destroy(joint);
            hookLifeTime = 0;
            hook.layer = LayerMask.NameToLayer("HandsRenderLayer");
            hook.transform.parent.gameObject.layer = LayerMask.NameToLayer("HandsRenderLayer");
            hook.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("HandsRenderLayer");
            gameObject.layer = LayerMask.NameToLayer("HandsRenderLayer");
            transform.parent.gameObject.layer = LayerMask.NameToLayer("HandsRenderLayer");
            reel.layer = LayerMask.NameToLayer("HandsRenderLayer");
            //reel.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("HandsRenderLayer");
            player.GetComponent<CapsuleCollider>().material.bounciness = 0;

            if(reel.GetComponent<AudioSource>().isPlaying)
            {
                reel.GetComponent<AudioSource>().Stop();
            }

            hookLanded = false;
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

        // for drawing the rope progressively
        currentGrapplePosition = Vector3.MoveTowards(currentGrapplePosition, hook.transform.position, Time.deltaTime * hookFlySpeed);
        
        if(!hookLanded && Vector3.Distance(currentGrapplePosition, hook.transform.position) <= 0.02f)
        {
            hook.GetComponent<AudioSource>().PlayOneShot(hookLand);
            hookLanded = true;
        }

        //hook.transform.position = currentGrapplePosition;

        fishingRod.transform.LookAt(hook.transform);

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

    public Vector3[] GetConePoints()
    {
        return conePoints;
    }
}
