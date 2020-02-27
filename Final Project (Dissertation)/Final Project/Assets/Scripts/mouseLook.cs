using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{
    //public float mouseSensitivity = 300f;

    public Transform playerBody;

    //float xRotation = 0f;

    //[Header("Camera Positons")]
    //public Transform endmarker = null;
    //public Transform topMarker = null;
    //public Transform thirdPersonMarker = null;

    [Header("Scripts")]
    public Modular_3D_Player_Controller PlayerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = PlayerControllerScript.standingMarker.position;
        // Locks the cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * PlayerControllerScript.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * PlayerControllerScript.mouseSensitivity * Time.deltaTime;

        PlayerControllerScript.xAxisRotation -= mouseY;
        PlayerControllerScript.xAxisRotation = Mathf.Clamp(PlayerControllerScript.xAxisRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(PlayerControllerScript.xAxisRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);

        if (PlayerControllerScript.isCrouching == true)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerControllerScript.crouchMarker.position, Time.deltaTime);
        }
        else if (PlayerControllerScript.isCrouching == false && PlayerControllerScript.isFirstPerson == true)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerControllerScript.standingMarker.position, Time.deltaTime);
        }

        //if (PlayerControllerScript.isFirstPerson == true)
        //{
           // transform.position = Vector3.Lerp(transform.position, topMarker.position, Time.deltaTime);
        //}
        if (PlayerControllerScript.isFirstPerson == false)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerControllerScript.thirdPersonMarker.position, Time.deltaTime);
        }

    }
}
