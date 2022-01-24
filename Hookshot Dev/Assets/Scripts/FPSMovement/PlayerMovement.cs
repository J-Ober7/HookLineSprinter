// Some stupid rigidbody based movement by Dani

using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    
    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    [Range(0.0f, 100.0f)]
    public float sensitivity = 50f;
    private float sensMultiplier = 1f;
    
    //Movement
    public float moveSpeed = 4500;
    public float minMaxSpeed = 15f;
    public float maxMaxSpeed = 100f;
    public float maxSpeedIncrement = 1f;
    float currentMaxSpeed;

    public bool grounded;
    public LayerMask whatIsGround;
    
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float crouchHeight = 0.5f;
    public float slideForce = 400;
    private float maxSpeedTemp;
    public float slideCounterMovement = 0.2f;
    public float crouchSpeed = 100f;
    private bool crouchWalk = false;
    private bool checkingUncrouch = false;  ///
    private bool uncrouchBeginning = false;  ///For determining if the uncrouch invoke has started 

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    public float extraGravityForce = 10;
    private float currentExtraGravity;
    
    //Input
    float x, y;
    bool jumping, sprinting, crouching;
    
    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;
    private Vector3 origPolePos;
    private Vector3 gunAimedPos;
    

    //Hooking
    public HookThrower hook;
    public GameObject gun;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start() 
    {
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        crouchScale.y = crouchHeight;
        origPolePos = hook.transform.parent.localPosition;
        gun = hook.transform.parent.parent.GetChild(1).gameObject;
        maxSpeedTemp = maxMaxSpeed;
        currentExtraGravity = extraGravityForce;

    }


    private void FixedUpdate() 
    {
        
        Movement();
        Momentum();
        Crouch();
    }

    private void Update() 
    {
        MyInput();
        Look();
        
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput() 
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        //crouching = Input.GetKey(KeyCode.LeftControl);
      
        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl)) //&& grounded
        {
            //Debug.Log(grounded);
            StartCrouch();
            transform.position = new Vector3(transform.position.x, transform.position.y - (playerScale.y/2), transform.position.z);
            crouching = true;

        }

        //Debug.Log("Uncrouch Check Logic: " + checkForUncrouchSpace());

        if ((Input.GetKeyUp(KeyCode.LeftControl) && crouching) || checkingUncrouch)
        {
            if (checkForUncrouchSpace())
            {
                if ((Input.GetKeyUp(KeyCode.LeftControl))){
                    StartPlayerUncrouch();
                }
                
                else
                {
                    if (!uncrouchBeginning)
                    {
                        uncrouchBeginning = true;
                        Invoke(nameof(StartPlayerUncrouch), .1f);
                    }
                }
            }
            else
            {

                checkingUncrouch = true;
            }
            
        }
    }



    /// <summary>
    /// Starts the player crouch and begins to learp the player model to the crouched size
    /// </summary>
    private void StartCrouch() 
    {
        
        transform.localScale = Vector3.Lerp(transform.localScale, crouchScale, crouchSpeed * Time.deltaTime);
        //transform.position = new Vector3(transform.position.x, transform.position.y -, transform.position.z);

        // Lower the fishing pole when you crouch
        Transform pole = hook.transform.parent;
        Vector3 crouchpos = new Vector3(pole.localPosition.x, pole.localPosition.y * 10f, pole.localPosition.z);
        pole.localPosition = Vector3.Lerp(pole.localPosition, crouchpos, Time.deltaTime);

        if (Magnitude2(rb.velocity.x, rb.velocity.z) > 0.5f) {
            if (grounded) {
                if (currentMaxSpeed < minMaxSpeed * 2)
                {
                    maxMaxSpeed = maxMaxSpeed + 15;
                    if(minMaxSpeed * 2 < maxMaxSpeed)
                    {
                        currentMaxSpeed = minMaxSpeed * 2;
                    }
                    else
                    {
                        currentMaxSpeed = maxMaxSpeed;
                    }
                }
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }

    }

    /// <summary>
    /// Determines if they player should being starting or stoping crouching based off input and player transform size
    /// </summary>
    private void Crouch()
    {
        if(transform.localScale != crouchScale && crouching)
        {
            StartCrouch();
        }
        else if (!crouching && transform.localScale != playerScale)
        {
            StopCrouch();
        }

        if(Magnitude2(rb.velocity.x, rb.velocity.z) < 0.5f && crouchWalk == false)
        {
            //Debug.Log(Magnitude2(rb.velocity.x, rb.velocity.z));
            crouchWalk = true;
        }
    }

    private bool checkForUncrouchSpace()
    {
        //RaycastHit hit;
        Debug.DrawRay(transform.position, orientation.up * 3, Color.red);
        if(Physics.Raycast(transform.position, orientation.up, 2f) )
        {
            Debug.DrawRay(transform.position, orientation.up, Color.blue);

            Debug.Log("You hit something");
            return false;
        }

        return true;
    }


    /// <summary>
    /// Starts the player uncrouch and begins to learp the player model to the uncrouched size
    /// </summary>
    private void StopCrouch() 
    {
        transform.localScale = Vector3.Lerp(transform.localScale, playerScale, crouchSpeed * Time.deltaTime);

        Transform pole = hook.transform.parent;

        pole.localPosition = origPolePos;

        maxMaxSpeed = maxMaxSpeed - 15;
        //transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    /// <summary>
    /// Invoked function for uncrouching the player
    /// </summary>
    private void StartPlayerUncrouch()
    {
        StopCrouch();
        transform.position = new Vector3(transform.position.x, transform.position.y + (playerScale.y / 2), transform.position.z);
        crouchWalk = false;
        crouching = false;
        checkingUncrouch = false;
        uncrouchBeginning = false;
    }

    /// <summary>
    /// gets the magnitude of 2 values
    /// </summary>
    private double Magnitude2 (double x, double y)
    {
        return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
    }

    private void Momentum()
    {
        if(rb.velocity.magnitude > currentMaxSpeed*0.95)
        {
            currentMaxSpeed = Mathf.Lerp(this.currentMaxSpeed, maxMaxSpeed, maxSpeedIncrement* Time.deltaTime);
        }
        else
        {
            currentMaxSpeed = Mathf.Lerp(this.currentMaxSpeed, minMaxSpeed, (maxSpeedIncrement/2)* Time.deltaTime);
        }
        Vector2 rbv = new Vector2(rb.velocity.x, rb.velocity.z);

        if(grounded)
        {
            rb.velocity = new Vector3(Vector2.ClampMagnitude(rbv, currentMaxSpeed).x, rb.velocity.y, Vector2.ClampMagnitude(rbv, currentMaxSpeed).y);
        }
        else
        {
            rb.velocity = new Vector3(Vector2.ClampMagnitude(new Vector2(rb.velocity.x, rb.velocity.z), maxMaxSpeed).x, rb.velocity.y, Vector2.ClampMagnitude(new Vector2(rb.velocity.x, rb.velocity.z), maxMaxSpeed).y);
        }

    }

    /// <summary>
    /// Primary function that moves the player. Handles most force application
    /// </summary>
    private void Movement() 
    {

        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * currentExtraGravity);

        currentExtraGravity += .5f * extraGravityForce * Time.deltaTime;
        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);
        
        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        
        //Set max speed
        if (crouching && grounded)
        {
            maxMaxSpeed = maxSpeedTemp/3;
        }
        else
        {
            // increment max speed over time

            maxMaxSpeed = maxSpeedTemp;
        }



        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        //if (x > 0 && xMag > currentMaxSpeed) x = 0;
        //if (x < 0 && xMag < -currentMaxSpeed) x = 0;
        //if (y > 0 && yMag > currentMaxSpeed) y = 0;
        //if (y < 0 && yMag < -currentMaxSpeed) y = 0;

        /*
        if(rb.velocity.sqrMagnitude > currentMaxSpeed)
        {
            rb.velocity *= .99f;
        }
        */

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;
        
        // Movement in air
        if (!grounded) 
        {
            multiplier = 0.6f;
            multiplierV = 0.6f;
        }

        
        // Movement while sliding
        if (grounded && crouching)
        {

                       
            if (crouchWalk)
            {
                multiplierV = 1f;
            }
            else
            {
                multiplierV = 0f;
            }
        }

        //Debug.Log(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        //Debug.Log(orientation.transform.forward * x * moveSpeed * Time.deltaTime * multiplier);
        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            //return;
        }
    }

    /// <summary>
    /// Function for player jumping. Handles jumping on the ground differently than jumping while hooked
    /// </summary>
    private void Jump() 
    {
        if ((grounded || hook.ReadyToHookJump()) && readyToJump)
        {
            readyToJump = false;

            //Add jump forces
            if (!hook.isHooking)
            { 
                //Normal Jump
                rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(normalVector * jumpForce * 0.5f);
            }
            else
            {
                //Jumping off the hook
                hook.HookJumpRelease();
                rb.AddForce(Vector2.up * jumpForce * .7f);
                float horizontalVelocity = Mathf.Clamp(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude, 10, 30);
                Debug.Log(horizontalVelocity);
                rb.velocity.Set(0, rb.velocity.y, 0);
                rb.AddForce(orientation.forward * jumpForce * .08f * horizontalVelocity);
               
            }
            
            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            
            
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    /// <summary>
    /// Inovoked function that handles jump cooldown
    /// </summary>
    private void ResetJump() 
    {
        readyToJump = true;
    }
    
    private float desiredX;
    /// <summary>
    /// Rotates the player's camera and orientation
    /// </summary>
    private void Look() 
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    /// <summary>
    /// Counteracts player movement to slow them down over time
    /// </summary>
    /// <param name="x">Determines if player is moving horizontally</param>
    /// <param name="y">Determines if the player is moving vertically</param>
    /// <param name="mag">the the relative velocity to where the player is looking</param>
    private void CounterMovement(float x, float y, Vector2 mag) 
    {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching && !crouchWalk) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) 
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) 
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //currentMaxSpeed = Mathf.Lerp(this.maxSpeed, minMaxSpeed, maxSpeedIncrement * Time.deltaTime);

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > currentMaxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * currentMaxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook() 
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    /// <summary>
    /// Determines if the ground the player is on is angled shallowly enough to be considered floor
    /// </summary>
    /// <param name="v"> the normal of the point where the player contacted on the object being checked</param>
    /// <returns></returns>
    private bool IsFloor(Vector3 v) 
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    
    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other) 
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) 
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                currentExtraGravity = extraGravityForce;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded) 
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    /// <summary>
    /// Invoked function for setting grounded to false
    /// </summary>
    private void StopGrounded() 
    {
        grounded = false;
    }
    
}
