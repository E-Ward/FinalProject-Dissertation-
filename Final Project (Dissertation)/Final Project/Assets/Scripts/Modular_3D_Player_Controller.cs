using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modular_3D_Player_Controller : MonoBehaviour
{
    //
    //
    // This is the character controller I wrote for my third year final project
    // It is designed to be modular
    // And to be friendly to non-programmers
    // While also allowing programmers to be easily able to add in their own code
    //
    //

    /// <summary>
    /// This section is used to store all of the keys used in the controller
    /// It provides tool tips so you can hover over a key in the inspector to get more information
    /// </summary>
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

    /// <summary>
    /// This section is where all of the movement variables are stored
    /// 
    /// </summary>
    [Header("Movement Variables")]
    [Tooltip("This value determines how fast the character will walk")]
    public float walkingSpeed;
    float currentSpeed;
    [Tooltip("This value determines how fast the character will move when running")]
    public float sprintSpeed;
    [Tooltip("This value determines how fast the character will move when crouched")]
    public float crouchSpeed;
    [Tooltip("This bool is used to see whether the character is crouched")]
    public bool isCrouching;
    [Tooltip("This bool is used to see whether the character is running")]
    public bool isRunning;
    [Tooltip("This value is how much gravity is applied to the player determining how fast they will fall")]
    public float gravity;
    [Tooltip("This value determines how high the character will jump")]
    public float jumpForce;
    [Tooltip("This is a true or false to see if the character is touching the ground")]
    public bool isGrounded;
    float distToGround;
    [Tooltip("This bool is used to see whether the camera is in the first person perspective")]
    public bool isFirstPerson;

    /// <summary>
    /// This section stores all of the character attributes
    /// </summary>
    [Header("Attributes")]
    [Tooltip("This value determines how much health the player has")]
    public int Health;
    [Tooltip("This value determines how much stamina the player has")]
    public float Stamina;
    [Tooltip("This value determines how fast the stamina re-charges")]
    public float staminaRechargeRate;
    [Tooltip("This is where you can drag the Health UI object from the canvas")]
    public Text HealthUIText;
    [Tooltip("This is where you can drag the Stamina UI object from the canvas")]
    public Text StaminaUIText;
    [Tooltip("How much stamin is taken when the character jumps")]
    public float staminaDrainFromJumping;
    [Tooltip("How much stamina is taken when the characher is running")]
    public float staminDrainFromRunning;

    //This float is used to store the current amount of stamina the character has
    //This does not need to be public as the user does not need to see this in the inspector
    //This value is represented in the UI
    float currentStamina;

    /// <summary>
    /// This section is where you can change the amount of damage the character takes from different sources
    /// </summary>
    [Header("Damage")]
    public int fireDamage;
    public int explosionDamage;

    /// <summary>
    /// This section is used to change the different variable of the camera attached to the character
    /// </summary>
    [Header("Camera Variables")]
    [Tooltip("Changing this value will change how sensetive the mouse is")]
    public float mouseSensitivity;

    /// <summary>
    /// This section is sued for game objects acting as the different camera positions
    /// Currently there are three positions for standing, crouching and third person
    /// </summary>
    [Header("Camera Positons")]
    public Transform standingMarker = null;
    public Transform crouchMarker = null;
    public Transform thirdPersonMarker = null;

    /// <summary>
    /// This section is used to store all of the variables used with the collider attached to the the character game object
    /// It currently includes the height, radius and center (x,y,z)
    /// </summary>
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

    /// <summary>
    /// This section is for all of the scripts this script needs access to
    /// </summary>
    [Header("Scripts")]
    public mouseLook MouseScript;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
        // Sets the default camera position to be first person
        isFirstPerson = true;

        // Sets the current speed of the player = to the walking speed set by the user in the inspector
        currentSpeed = walkingSpeed;

        // Sets the distance to the ground
        distToGround = GetComponent<Collider>().bounds.extents.y;

        // Sets the is crouching to false so the player starts standing
        isCrouching = false;

        // Sets is running to false so the player starts in a walking state
        isRunning = false;

        // This sets the rigid body to the one attached to this game object
        rb = GetComponent<Rigidbody>();

        // This sets the capCollider to the Capsule collider on the current gameobject
        capCollider = gameObject.GetComponent<CapsuleCollider>();
        // Sets the attached capsule height to the variable set by the user in the inspector
        capCollider.height = colliderHeight;
        // Sets the attached capsule radius to the variable set by the user in the inspector
        capCollider.radius = colliderRadius;
        // Sets the attached capsule center points to the variable set by the user in the inspector
        capCollider.center = new Vector3(centerX,centerY,centerZ);

        // Sets the health Ui text to the health set by the user in the isnpector
        HealthUIText.text = HealthUIText.text = "Health: " + Health.ToString();
        // Sets the stamina Ui text to the stamina set by the user in the isnpector
        // It also clamps the float to no decimal places displaying a whole number in the UI
        StaminaUIText.text = StaminaUIText.text = "Stamina: " + Stamina.ToString("F0");

        // Sets the current stamina the character has to the overall stamina set by the user in the inspector
        currentStamina = Stamina;
    }

    
    // Update is called once per frame
    void Update()
    {
        //Same as the health function in the start however, this will make the health value update every frame
        HealthUIText.text = HealthUIText.text = "Health: " + Health.ToString();
        //Same as the stamina function in the start however, this will make the stamina value update every frame
        StaminaUIText.text = StaminaUIText.text = "Stamina: " + currentStamina.ToString("F0");

        /// <summary>
        /// This section is used to se the key functions
        /// As they are using the keycodes set up at the top of this script the user can change what keys on the keyboard are used to activate the code
        /// </summary>

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

        // Checks to see if the character is grounded and if they have enough stamina and if they do, it will let the jump
        if (Input.GetKeyDown(jump) && isGrounded == true && currentStamina > 0)
        {

            rb.velocity = Vector3.up * jumpForce * Time.deltaTime;
            currentStamina -= staminaDrainFromJumping;
            //transform.position += Vector3.up * jumpForce * Time.deltaTime;
        }
        
        // Checks to see if there is enough stamina and if there is, it will allow the character to jump
        // When the user presses the sprint button it will set the is running bool to true
        if (Input.GetKeyDown(sprint) && currentStamina > 0)
        {
            currentSpeed = sprintSpeed;
            isRunning = true; 
           
        }
        else if (Input.GetKeyUp(sprint))
        {
            currentSpeed = walkingSpeed;
            isRunning = false;  
        }

        // When the crouch button is pressed the is crouching bool is set to true and the speed is set to the crouch speed
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
///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        // Enables a force like gravity if the player is not touching the ground
        // This is done only when they character is not grounded so there isn't a constant force applied to the character
        if(isGrounded == false)
        {
            transform.position += Vector3.down * gravity * Time.deltaTime;
        }

        // If the camera change button is pressed and the camera is in the first person location, sets the first person bool to false
        if (Input.GetKeyDown(cameraChange) && isFirstPerson == true)
        {
            isFirstPerson = false;
        }
        else if (Input.GetKeyDown(cameraChange) && isFirstPerson == false)
        {
            isFirstPerson = true;
        }

        // If the character is not running then the stamina will re-charge based on what variable is set by the user in the inspector
        if (currentStamina <= Stamina && isRunning == false)
        {
            currentStamina += staminaRechargeRate * Time.deltaTime;
        }

        // If the user is running and the current stamina they have is above 0 then stamina will be drained from the character
        if ( isRunning == true && currentStamina > 0)
        {
            currentStamina -= staminDrainFromRunning * Time.deltaTime;
        }

    }
    // This is used for the raycast out of the bottom of the character
    // The raycast is used to get the distance the character is from the ground
    // Then in another part of the script the distance is used to calculate of the character is touching the ground or not
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
