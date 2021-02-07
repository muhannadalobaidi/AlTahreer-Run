using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    //sound
    public AudioClip crashSound;
    //movement
    private const float LANE_DISTANCE = 2.5f;
    public float JumpForce = 20.0f;
    public float Gravity = 75.0f;
    private float VerticalVelocity;
    private CharacterController controller;
    private int desiredLane = 1;
    private float turn_Speed = 0.1f;

    // Speed Modifier
    public float originalSpeed = 12.0f;
    private float Speed; 
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;

    //
    private bool isRunning = false;
    

    //Animation
    private Animator anim;


    
    private void Start ()
    {
        Speed = originalSpeed;

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (!isRunning)

            return;

        //speed modifier 

        if ((Time.time - speedIncreaseLastTick) > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            Speed += speedIncreaseAmount;
            GameManager.Instance.UpdateModifier(Speed - originalSpeed);
        }

        //gather the input on which lane we should be 
        if (mobileInput.Instance.SwipeLeft)
            //mobileInput.Instance.SwipeLeft
            //Input.GetKeyDown(KeyCode.LeftArrow)
            MoveLane(false);
        


        if (mobileInput.Instance.SwipeRight)
            //mobileInput.Instance.SwipeRight
            //Input.GetKeyDown(KeyCode.RightArrow)

            MoveLane(true);
        

        // calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;

        switch (desiredLane)

        {

            case 0:

                targetPosition += Vector3.left * LANE_DISTANCE;

                break;

            case 2:

                targetPosition += Vector3.right * LANE_DISTANCE;

                break;

        }

        //moving vector
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * Speed * 1.5f;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        //calculate y


        if (IsGrounded()) // if grounded 
        {
            VerticalVelocity = -1f;

            if (mobileInput.Instance.SwipeUp)

            {
                //jump
                anim.SetTrigger("Jump");
                VerticalVelocity = JumpForce;
            }
            else if (mobileInput.Instance.SwipeDown)
            {
                //slid
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }

        else
        {
            VerticalVelocity -= (Gravity * Time.deltaTime);

            //fast falling michanic
            if (mobileInput.Instance.SwipeDown)
            {
                VerticalVelocity = -JumpForce;
            }
        }
        moveVector.y = VerticalVelocity;
        moveVector.z = Speed;


        //move player
        controller.Move(moveVector * Time.deltaTime);

        // rotate the player to where he is going
        Vector3 dir = controller.velocity;



        if (dir != Vector3.zero)

        {

            dir.y = 0f;

            transform.forward = Vector3.Lerp(transform.forward, dir, turn_Speed);

        }
    }
    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 3;
        controller.center = new Vector3(controller.center.x, controller.center.y/2, controller.center.z);

    }
    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 3;
        controller.center = new Vector3(controller.center.x, controller.center.y*2, controller.center.z);
    }

    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
        

    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(new Vector3(controller.bounds.center.x,(controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,controller.bounds.center.z),Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction,Color.red, 1.0f);

        return Physics.Raycast(groundRay, 0.2f  + 0.1f);

    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
    }

    private void Crash()
    {
        AudioSource.PlayClipAtPoint(crashSound, transform.position);
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.OnDeath();
        CameraShake.Shake(0.15f, 1.5f);
    }

    private void Trap()
    {
        Speed = originalSpeed;
        CameraShake.Shake(0.15f, 1.5f);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
            case "SideHit":
                Trap();
                break;
        }
    }

}
