using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Score")]
    public Timer timer;
    public GameObject gameover;
    GameOverScreen gm;  // CHANGE NAMES

    [Header("Health")]
    public int health;
    public int maxHealth;
    //public TMP_Text healthText;

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

    [Header("Shooting")]
    public Image enemyCrosshair;

    [Header("Enemy")]


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private Camera m_Camera;

    public int Health
    {
        get { 
            return health;
        }
        set { }
    }

    public enum MovementState
    {
        walking,
        running,
        crouching,
        air
    }

    private void Awake()
    {
        m_Camera = Camera.main; // unity tut
        enemyCrosshair.enabled = false;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Allows physics system to control rotation of object

        readyToJump = true;

        maxJumps = 2;
        jumpCounter = 0;
        maxHealth = 3;
        health = maxHealth;
        Health = health;


        startYScale = transform.localScale.y;   // Scale is set to default for crouching

        //healthUpdate();

        gm = gameover.GetComponent<GameOverScreen>();
    }

    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(raycastStart.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);  // Casts ray onto ground from players position halved to check if they're on the ground

        MyInput();
        SpeedControl();
        StateHandler();
        mouseDetection();

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
            timer.timerOn = false; // Stops timer to get end time
            gm.ActivateGameover(health, (int)timer.totalTime);
        }
    }
    private void FixedUpdate()
    {
        Moveplayer();    // Called within fixed update specifically as uses physics 
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

        // Start crouching
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);   // Halves the size of the player

            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);  // Pushes them down so that they don't remain in the air 
        }

        // Stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);    // Resets player size
        }

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = m_Camera.ScreenPointToRay(mousePos);  //Unity tutorial used here
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                //print(hit.collider.gameObject);
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    //enemyStats.Health -= 1;
                    //hit.collider.GetComponent<EnemyStats>().Health -= 1;
                    hit.collider.GetComponent<EnemyStats>().reduceHealth(1);

                    //hit.collider.GetComponentInChildren<EnemyHealthBar>().updateHealth(
                    //    hit.collider.GetComponent<EnemyStats>().Health,
                    //    hit.collider.GetComponent<EnemyStats>().MaxHealth);

                }
            }
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
        // Crouching:
        if (grounded && Input.GetKey(crouchKey))    //Each state has different speed depending on which key is pressed
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

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
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);    // Users regular velocity

        //Limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //Limits a user when they hit their top speed
        }
    }

    private void mouseDetection()
    {
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.collider.gameObject.tag == "Enemy")
            {
                enemyCrosshair.enabled = true;
            }
            else
            {
                enemyCrosshair.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //if (health < 3)
            //{
            //    health += 1;    // Increases health with a cap of 3
            //    //healthUpdate();
            //}
            //else
            //{
            //    Debug.Log("Health already full!");  // Still destroys object if player collides with it but informs that health is full            
            //}
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 1;
            //Destroy(collision.gameObject);  
            //healthUpdate();
        }
    }
    private void OnMouseOver()  // THESE TWO COPIED FROM ABOVE, SHORTEN DOWN FOR THE LOVE OF GOD
    {
        //Vector3 mousePos = Input.mousePosition;
        //Ray ray = m_Camera.ScreenPointToRay(mousePos);  //Unity tutorial used here
        //if (Physics.Raycast(ray, out RaycastHit hit))
        //{
        //    //print(hit.collider.gameObject);
        //    if (hit.collider.gameObject.tag == "Enemy")
        //    {
        //        enemyCrosshair.enabled = true;
        //    }
        //}

        //if(gameObject.gameObject.tag == "Enemy")
        //{
        //    enemyCrosshair.enabled = true;
        //    print("Hit");
        //}
        //print("Hit " + gameObject.name);
    }
    private void OnMouseExit()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Ray ray = m_Camera.ScreenPointToRay(mousePos);  //Unity tutorial used here
        //if (Physics.Raycast(ray, out RaycastHit hit))
        //{
        //    //print(hit.collider.gameObject);
        //    if (hit.collider.gameObject.tag == "Enemy")
        //    {
        //        enemyCrosshair.enabled = false;
        //    }
        //}

        //if (gameObject.gameObject.tag == "Enemy")
        //{
        //    enemyCrosshair.enabled = false;
        //    print("Not hit");
        //}
        //print("Left " + gameObject.name);
    }
    //private void healthUpdate()
    //{
    //    healthText.text = String.Format($"Health : {health}");
    //}
    private IEnumerator PowerupTimer(float powerupIncrease)
    {
        yield return new WaitForSeconds(powerupCooldown);   // Cooldown used here so that powerup only lasts for a limited while

        walkSpeed -= powerupIncrease;   //Resets powerups back to normal
        runSpeed -= powerupIncrease;

    }

    public void PowerupActivate(float powerupIncrease)
    {
        walkSpeed += powerupIncrease;
        runSpeed += powerupIncrease;
        StartCoroutine(PowerupTimer(powerupIncrease));
    }
}
