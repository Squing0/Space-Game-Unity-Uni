using System.Collections;
using UnityEngine;
using UI;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float walkSpeed;
        public float runSpeed;
        private float moveSpeed;

        public float groundDrag;
        public MovementState state;

        [Header("Jumping")]
        public float jumpSpeed;
        public float jumpCooldown;
        public float airMultiplier;
        public int maxJumps;
        private int jumpCounter;
        bool readyToJump;

        [Header("Score")]
        public Charge charger;

        [Header("Health")]
        private int health;
        public int maxHealth;

        // Health property
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
            set
            {
                maxHealth = value;
            }
        }

        [SerializeField]
        private HealthBar healthBar;

        [Header("Power Ups")]
        public float speedIncrease;
        public float jumpIncrease;
        public float powerupCooldown;

        [Header("keybinds")]
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode crouchKey = KeyCode.LeftControl;

        [Header("Ground Check")]
        public float playerHeight;
        public Transform raycastStart;
        public LayerMask whatIsGround;  // Applied to objects that are considered ground for the user
        bool grounded;

        public Transform orientation;

        // Useful for trying to keeo enemy outside of player range
        public bool playerAreaRange;
        public LayerMask isEnemy;
        public float playerRange;


        // Miscellaneous 
        public GameObject UIManager;
        private UiManager ui;

        // Player movement
        float horizontalInput;
        float verticalInput;

        Vector3 moveDirection;

        // Rigidbody and camera
        Rigidbody rb;
        private Camera m_Camera;

        // State handler
        public enum MovementState
        {
            walking,
            running,
            air
        }

        private void Awake()
        {
            m_Camera = Camera.main; // unity tut
            //playerAreaRange = Physics.CheckSphere(transform.position, playerRange, isEnemy); // Not sure if useful
        }
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true; // Allows physics system to control rotation of object

            ui = UIManager.GetComponent<UiManager>();

            readyToJump = true;
            maxJumps = 2;
            jumpCounter = 0;

            health = maxHealth;        
        }

        private void Update()
        {
            //ground check
            grounded = Physics.Raycast(raycastStart.position, Vector3.down, playerHeight * 1f + 0.3f, whatIsGround);  // Casts ray onto ground from players position halved to check if they're on the ground
            Debug.DrawRay(raycastStart.position, Vector3.down, Color.black, whatIsGround);
             
            MyInput();
            SpeedControl();
            StateHandler();
            //mouseDetection();

            if (grounded)
            {
                rb.drag = groundDrag;   // Drag (air resistance) only applied when grounded to make air movement less restricted
            }
            else
            {
                rb.drag = 0;
            }

            if (health < 1)
            {
                ui.ActivateGameover("You lost all your health!");
            }
        }
        private void FixedUpdate()
        {
            Moveplayer();    // Called within fixed update specifically as uses physics 
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerRange);
        }
        private void MyInput()
        {
            //Movement in any direction
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            //Jumping
            if (Input.GetKey(jumpKey) && readyToJump && (grounded || jumpCounter < maxJumps))    // User has a max amount of jumps that they can't go over. Can be any number
            {
                readyToJump = false;

                Jump();

                if (!grounded)
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

                StartCoroutine(ResetJump());
            }
        }

        private void Jump()
        {
            rb.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);   // Specifically moves player in y axis, impulse is used here as jump is instant 
        }
        private IEnumerator ResetJump()
        {
            yield return new WaitForSeconds(jumpCooldown);  // Coroutine used to have cooldown so player can't repeatedly jump at once

            readyToJump = true;
        }
        private void StateHandler()
        {
            // Running:
            if (grounded && Input.GetKey(sprintKey))
            {
                state = MovementState.running;
                moveSpeed = runSpeed;
            }

            // Walking:
            else if (grounded)
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }

            // Air:
            else
            {
                state = MovementState.air;
            }
        }

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
                rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Acceleration); // Air mulyiplier is used here to slightly increase air speed
            }

            if (grounded && Input.GetMouseButtonDown(1) || !grounded && Input.GetMouseButtonDown(1))
            {
                StartCoroutine(Dash());
                //rb.AddForce(moveDirection.normalized * 3f * 10, ForceMode.Impulse);
            }
        }

        private IEnumerator Dash() // Got this from youtube tutorial, change to own way (look gpt example)
        {
            float startTime = Time.time;

            while (Time.time < startTime + 0.25f)
            {
                rb.AddForce(moveDirection.normalized * .8f * 10, ForceMode.Impulse);
                yield return null;
            }
        }

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
            if (other.CompareTag("Rock"))
            {
                health -= 1;
                healthBar.updateHealth(health, maxHealth);
            }
        }

        private void OnCollisionEnter(Collision collision)  
        {
            if (collision.gameObject.tag == "Enemy")
            {
                health -= 1;
                healthBar.updateHealth(health, maxHealth);
            }
}

        public void SpeedUpActivate(float speedIncrease, float speedTime)  // MAKE MORE GENERAL LIKE ABOVE SO REUSABLE???
        {
            walkSpeed += speedIncrease;
            runSpeed += speedIncrease;
            StartCoroutine(SpeedTimer(speedIncrease, speedTime));
        }

        private IEnumerator SpeedTimer(float speedDecrease, float speedCooldown)
        {
            yield return new WaitForSeconds(speedCooldown);

            walkSpeed -= speedDecrease;
            runSpeed -= speedDecrease;

        }
    }
}