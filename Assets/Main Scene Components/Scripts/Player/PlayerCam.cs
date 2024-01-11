using UnityEngine;

namespace Player
{
    // Handles camera sensitivity and movement.
    public class PlayerCam : MonoBehaviour
    {
        // X and Y sensitivity of camera movement.
        public float sensX;
        public float sensY;

        // Orientation of player.
        public Transform orientation;

        // X and Y rotation of camera.
        float xRotation;
        float yRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;   // Locks cursor to center of screen and makes invisible.
            Cursor.visible = false;
        }

        private void Update()
        {
            // Get mouse input and change movement depending on sensitivity.
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;

            xRotation -= mouseY; 
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Limits range of camera movement.

            // Rotate camera and orientation of player.
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}