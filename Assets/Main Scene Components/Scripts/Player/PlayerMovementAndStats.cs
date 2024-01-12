using System.Collections;
using UnityEngine;
using UI;
using UnityEngine.UI;

namespace Player
{
    // Handles all variables and methods relating to player movement and stats
    public class PlayerMovementAndStats : MonoBehaviour
    {
        [Header("Movement")]
        public float groundDrag;
        public MovementState state;

        [Header("Jumping")]
        public float jumpSpeed;
        public float jumpCooldown;
        public float airMultiplier;
        public int maxJumps;

        [Header("Score")]
        public Charge charger;

        [Header("Health")]
        public SliderManager healthBar;
        public HealthManager healthManager;

        [Header("Power Ups")]
        public float speedIncrease;
        public float jumpIncrease;
        public float powerupCooldown;

        [Header("Dashing")]
        public RawImage dashIcon;
        public float dashSpeed;
        public float dashDelay;

        [Header("keybinds")]
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;

        [Header("Ground Check")]
        public float playerHeight;
        public Transform raycastStart;
        public LayerMask whatIsGround;  // Applied to objects that are considered ground for the user

        [Header("Orientation")]
        public Transform orientation;

        // Handles move speed for all states.
        private float moveSpeed;

        // Private dash variables.
        private bool readyToDash;
        private Color originalDashColour;

        // Checks if player on ground.
        private bool grounded;

        // Private jump variables.
        private int jumpCounter;
        private bool readyToJump;

        // Private player movement variables,
        private float horizontalInput;
        private float verticalInput;

        private Vector3 moveDirection;

        // Rigidbody 
        Rigidbody rb;

        // State handler
        public enum MovementState
        {
            walking,
            running,
            air
        }

        private float walkSpeed;
        private float runSpeed;
        private int health;
        private int maxHealth;

        // Properties used here as variables accessed in other classes.
        public float WalkSpeed { get { return walkSpeed; } set { walkSpeed = value; } }
        public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
        public int Health{ get { return health; } set { health = value; } }

        public int MaxHealth {get { return maxHealth; }set { maxHealth = value; } }

        private void Awake()
        {
            // Setting jump variables.
            readyToJump = true;
            jumpCounter = 0;

            // Setting dash varaibles.
            readyToDash = true;
            originalDashColour = dashIcon.color;    // Stores initial dash icon colour for later reference.
                
            health = maxHealth; // Current health is set to full.

            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true; // Allows physics system to control rotation of object
        }
        private void Start()
        {
            // Health manager instantiated to handle all health methods.
            healthManager = new HealthManager(health, maxHealth, healthBar, "Player");
        }

        private void Update()
        {
            //ground check
            grounded = Physics.Raycast(raycastStart.position, Vector3.down, playerHeight * 1f + 0.3f, whatIsGround);  // Casts ray onto ground from players position halved to check if they're on the ground
             
            // Handles player input, speed they're going at and state they're in at once.
            MyInput();
            SpeedControl();
            StateHandler();

            if (grounded) { rb.drag = groundDrag;}  // Drag (air resistance) only applied when grounded to make air movement less restricted 
            else { rb.drag = 0;}

            if (!readyToDash) {dashIcon.color = Color.black;}   // Dash icon is set to black if used and back to original after delay.
            else{dashIcon.color = originalDashColour;}
        }
        private void FixedUpdate()
        {
            Moveplayer();    // Called within fixed update specifically as uses physics 
        }

        // Handles player movement and jumping.
        private void MyInput()
        {
            //Movement in any direction
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            //Jumping
            if (Input.GetKey(jumpKey) && readyToJump && (grounded || jumpCounter < maxJumps))    // User must be grounded, or have more than one jump to jump.
            {
                readyToJump = false;

                Jump();

                if (!grounded)  // If user not grounded and jump, add to counter.
                {
                    jumpCounter++;
                }

                if (grounded)   // Resets counter so user can continually jump
                {
                    jumpCounter = 0;
                }

                if (jumpCounter == 0)   // Check is here so that if a user is grounded that the jump counter will still be incremented
                {
                    jumpCounter++;
                }

                StartCoroutine(ResetJump());    // Delay between jumping so player can't spam.
            }
        }

        // Specifically moves player in y axis, impulse is used here as jump is instant. 
        private void Jump()
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);   
        }
        private IEnumerator ResetJump()
        {
            yield return new WaitForSeconds(jumpCooldown);  // Coroutine used to have cooldown so player can't repeatedly jump at once

            readyToJump = true;
        }
        // Changes state of player based on conditions.
        private void StateHandler()
        {
            // Running:
            if (grounded && Input.GetKey(sprintKey))   
            {
                state = MovementState.running;  // Speed is increased in running state.
                moveSpeed = runSpeed;
            }

            // Walking: 
            else if (grounded)  // Player continually in walking state if grounded and not running.
            {
                state = MovementState.walking;  // Speed is set to normal in walking state.
                moveSpeed = walkSpeed;
            }

            // Air:
            else
            {
                state = MovementState.air;
            }
        }

        // Moves player based on input.
        private void Moveplayer()
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;  // Gets direction using both x and z axis at same time
                                                                                                        // on ground
            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Acceleration); //move direction is set to whole number and used with acceleration mode as movement is continuous
            }
            //in air
            else if (!grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Acceleration); // Air multiplier is used here to slightly increase air speed
            }

            if (grounded && Input.GetMouseButtonDown(1) && readyToDash) // Player can only dash when on ground.
            {
                StartCoroutine(Dash()); // Player dashes.

                readyToDash = false;    // Bool and coroutine used to delay multiple dashes.
                StartCoroutine(ResetDash());
            }
        }

        // Resets player dash by specified time.
        private IEnumerator ResetDash()
        {
            yield return new WaitForSeconds(dashDelay); 
            readyToDash = true;
        }

        // Coroutine used to apply dash over multiple frames (Gotten from YouTube tutorial)
        private IEnumerator Dash() 
        {
            float startTime = Time.time;

            while (Time.time < startTime + 0.25f)   // Allows dash to last a specified time.
            {
                rb.AddForce(moveDirection.normalized * dashSpeed, ForceMode.Impulse);   // Impulse used to give boost effect.
                yield return null;  // Pauses coroutine until next frame.
            }
        }

        // Limits player speed if needed.
        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);    // Users regular velocity

            //Limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // Limits a user when they hit their top speed
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Decrease player health when hit with enemy projectile.
            if (other.CompareTag("Rock"))
            {
                healthManager.DecreaseHealth(1);
            }
        }

        private void OnCollisionEnter(Collision collision)  
        {
            // Decrease player health when collide with enemy.
            if (collision.gameObject.tag == "Enemy")
            {
                healthManager.DecreaseHealth(1);
            }
        }

        // Increases walking and running speed by specified amount for a specified time.
        public void SpeedUpActivate(float speedIncrease, float speedTime)  
        {
            walkSpeed += speedIncrease;
            runSpeed += speedIncrease;
            StartCoroutine(SpeedTimer(speedIncrease, speedTime));
        }

        // Resets walking and running speeds to normal.
        private IEnumerator SpeedTimer(float speedDecrease, float speedCooldown)
        {
            yield return new WaitForSeconds(speedCooldown);

            walkSpeed -= speedDecrease;
            runSpeed -= speedDecrease;
        }
    }
}