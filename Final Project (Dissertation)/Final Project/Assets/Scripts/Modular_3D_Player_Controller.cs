using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modular_3D_Player_Controller : MonoBehaviour
{
    
    [Header("Movement Keys")]
    [Tooltip("Here you can select which keys you would like to use for movement.")] //This text will appear when you hover over the header 
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode crouch;
    public KeyCode cameraChange;

    [Header("Movement Variables")]
    [Tooltip("This value determines how fast the character will walk")]
    public float walkingSpeed;
    float currentSpeed;
    [Tooltip("This value determines how fast the character will walk when running")]
    public float sprintSpeed;
    [Tooltip("This value determines how fast the character will walk when crouched")]
    public float crouchSpeed;
    public bool isCrouching;
    [Tooltip("This value is how much gravity is applied to the player determining how fast they will fall")]
    public float gravity;
    [Tooltip("This value determines how high the character will jump")]
    public float jumpForce;
    public bool isGrounded;
    float distToGround;
    public bool isFirstPerson;

    [Header("Attributes")]
    [Tooltip("This value determines how much health the player has")]
    public int Health;
    [Tooltip("This value determines how much stamina the player has")]
    public float Stamina;

    [Header("Scripts")]
    public mouseLook MouseScript;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the default camera position to be first person
        isFirstPerson = true;
        // Sets the current speed of the player = to the walking speed set by the user
        currentSpeed = walkingSpeed;
        // Sets the distance to the ground by
        distToGround = GetComponent<Collider>().bounds.extents.y;
        // Sets the is crouching to false so the player starts standing
        isCrouching = false;

        rb = GetComponent<Rigidbody>();
    }

    //Vector3 velocity;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(forward))
        {
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }

        if (Input.GetKey(backward))
        {
            transform.position += -transform.forward * currentSpeed * Time.deltaTime;
        }
            
        if (Input.GetKey(left))
        {
            transform.position += -transform.right * currentSpeed * Time.deltaTime;
        }
            
        if (Input.GetKey(right))
        {
            transform.position += transform.right * currentSpeed * Time.deltaTime;
        }
   
        if (Input.GetKeyDown(jump) && isGrounded == true)
        {

            rb.velocity = Vector3.up * jumpForce * Time.deltaTime;
            //transform.position += Vector3.up * jumpForce * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(sprint))
        {
            currentSpeed = sprintSpeed;
        }
        else if (Input.GetKeyUp(sprint) )
        {
            currentSpeed = walkingSpeed;
        }

        if (Input.GetKeyDown(crouch))
        {
            //MouseScript.MoveCameraToCrouchPos();
            isCrouching = true;
            currentSpeed = crouchSpeed;
        }
        else if(Input.GetKeyUp(crouch))
        {
            //MouseScript.MoveCameraToStandPos();
            isCrouching = false;
            currentSpeed = walkingSpeed;
        }

        if(isGrounded == false)
        {
            transform.position += Vector3.down * gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown(cameraChange) && isFirstPerson == true)
        {
            isFirstPerson = false;
        }
        else if (Input.GetKeyDown(cameraChange) && isFirstPerson == false)
        {
            isFirstPerson = true;
        }
    }
    void FixedUpdate()
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f))
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }
}
