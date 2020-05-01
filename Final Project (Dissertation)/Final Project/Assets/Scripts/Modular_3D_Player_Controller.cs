using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modular_3D_Player_Controller : MonoBehaviour
{
    //
    //
    // This is the character controller I wrote for my third year final project
    // It is designed to be friendly to non programmers
    //
    //

    [Header("Movement Keys")]
    [Tooltip("This is the key used to make the character move forward")]
    public KeyCode forward;
    [Tooltip("This is the key used to make the character move backward")]
    public KeyCode backward;
    [Tooltip("This is the key used to make the character move left")]
    public KeyCode left;
    [Tooltip("This is the key used to make the character move right")]
    public KeyCode right;
    [Tooltip("This is the key used to make the character jump")]
    public KeyCode jump;
    [Tooltip("This is the key used to make the character sprint")]
    public KeyCode sprint;
    [Tooltip("This is the key used to make the character crouch")]
    public KeyCode crouch;
    [Tooltip("This is the key used to switch camera perspectives")]
    public KeyCode cameraChange;

    [TextArea]
    public string ControllerInformation = "";


    [Header("Movement Variables")]
    [Tooltip("This value determines how fast the character will walk")]
    public float walkingSpeed;
    float currentSpeed;
    [Tooltip("This value determines how fast the character will move when running")]
    public float sprintSpeed;
    [Tooltip("This value determines how fast the character will move when crouched")]
    public float crouchSpeed;
    public bool isCrouching;
    public bool isRunning;
    [Tooltip("This value is how much gravity is applied to the player determining how fast they will fall")]
    public float gravity;
    [Tooltip("This value determines how high the character will jump")]
    public float jumpForce;
    [Tooltip("This is a true or false to see if the character is touching the ground")]
    public bool isGrounded;
    float distToGround;
    public bool isFirstPerson;

    [Header("Attributes")]
    [Tooltip("This value determines how much health the player has")]
    public int Health;
    [Tooltip("This value determines how much stamina the player has")]
    public float Stamina;
    public float staminaRechargeRate;
    [Tooltip("This is where you can drag the Health UI object from the canvas")]
    public Text HealthUIText;
    [Tooltip("This is where you can drag the Stamina UI object from the canvas")]
    public Text StaminaUIText;
    [Tooltip("How much stamin is taken when the character jumps")]
    public float staminaDrainFromJumping;
    [Tooltip("How much stamina is taken when the characher is running")]
    public float staminDrainFromRunning;

    float currentStamina;

    [Header("Damage")]
    public int fireDamage;
    public int explosionDamage;


    [Header("Camera Variables")]
    [Tooltip("Changing this value will change how sensetive the mouse is")]
    public float mouseSensitivity;

    [Header("Camera Positons")]
    public Transform standingMarker = null;
    public Transform crouchMarker = null;
    public Transform thirdPersonMarker = null;

    [Header("Capsule Collider Variables")]
    public CapsuleCollider capCollider;
    [Tooltip("This sets the height of the collider attached to the player")]
    public float colliderHeight;
    [Tooltip("This sets the radius of the collider attached to the player")]
    public float colliderRadius;
    [Tooltip("Change this value to change the position of the collider")]
    public float centerX;
    public float centerY;
    public float centerZ;

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

        isRunning = false;

        rb = GetComponent<Rigidbody>();

        //This sets the capCollider to the Capsule collider on the current gameobject
        capCollider = gameObject.GetComponent<CapsuleCollider>();
        capCollider.height = colliderHeight;
        capCollider.radius = colliderRadius;
        capCollider.center = new Vector3(centerX,centerY,centerZ);

        HealthUIText.text = HealthUIText.text = "Health: " + Health.ToString();
        StaminaUIText.text = StaminaUIText.text = "Stamina: " + Stamina.ToString("F0");

        currentStamina = Stamina;
    }

    //Vector3 velocity;
    // Update is called once per frame
    void Update()
    {
        HealthUIText.text = HealthUIText.text = "Health: " + Health.ToString();
        StaminaUIText.text = StaminaUIText.text = "Stamina: " + currentStamina.ToString("F0");

        

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

        if (Input.GetKeyDown(jump) && isGrounded == true && currentStamina > 0)
        {

            rb.velocity = Vector3.up * jumpForce * Time.deltaTime;
            currentStamina -= staminaDrainFromJumping;
            //transform.position += Vector3.up * jumpForce * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(sprint))
        {
            currentSpeed = sprintSpeed;
            isRunning = true; 
           
        }
        else if (Input.GetKeyUp(sprint)  && currentStamina > 0)
        {
            currentSpeed = walkingSpeed;
            isRunning = false;  
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

        if (currentStamina <= Stamina && isRunning == false)
        {
            currentStamina += staminaRechargeRate * Time.deltaTime;
        }

        if ( isRunning == true && currentStamina > 0)
        {
            currentStamina -= staminDrainFromRunning * Time.deltaTime;
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
