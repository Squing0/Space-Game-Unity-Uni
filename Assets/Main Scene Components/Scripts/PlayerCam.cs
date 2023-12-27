using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    //public Transform playerModel;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // Locks cursor to center of screen
        Cursor.visible = false;
    }

    private void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY; // don't understand the rotation here
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limits range of camera movement

        // rotate camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //playerModel.rotation = Quaternion.Euler(xRotation, yRotation, 0);     Didn't work
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
