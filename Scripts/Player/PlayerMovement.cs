using UnityEngine;
using System;
using Leon;

public class PlayerMovement : MonoBehaviour
{
    public static Action OnPlayerLose;
    public static Action OnPlayerDestinationReach;
    public static Action<Vector3> OnPlayerRespawn;

    private PlayerController _playerController;
    
    GameObject clone;

    //Player Properties
    Rigidbody rb;
    //Animator anim;
    CapsuleCollider col;

    //Movement variables
    public float dirX = 1;
    [SerializeField] float speed = 7f;
    [SerializeField] float jumpForce = 14f;
    public bool doubleJump;

    //Ground Check Variables
    bool groundFall;
    bool isGrounded;
    [SerializeField] float groundedCheckDistance;

    [SerializeField] LayerMask groundMask;

    //Hang and Climb function properties
    Transform hangCol;
    Transform climbCol;

    bool checkTrg = false;
    public bool checkWallCol = false;
    public bool checkWallDrop = false;

    //Checkpoint and Lives
    Transform checkpoint;

    //Animation states
    public enum MovementState
    {
        run,
        jump,
        hang,
        climb,
        idle,
        slide,
        falling
    };

    public MovementState? state = MovementState.run;

    void Awake()
    {
        speed = 0f;

        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        InputManager.OnRunKeyPressed += HandleRun;
        InputManager.OnJumpKeyPressed += HandleJump;
        InputManager.OnSlideKeyPressed += HandleSlide;
        InputManager.OnSlideKeyUp += StopSliding;
    }

    private void OnDisable()
    {
        InputManager.OnRunKeyPressed -= HandleRun;
        InputManager.OnJumpKeyPressed -= HandleJump;
        InputManager.OnSlideKeyPressed -= HandleSlide;
        InputManager.OnSlideKeyUp -= StopSliding;
    }

    public void HandleMovementUpdate()
    {
        MoveUpdate();
        Checks();
        UpdateAnimation();
    }

    private void HandleRun()
    {
        speed = 7f;
        //anim.SetTrigger("Runforward");
    }

    private void HandleJump()
    {
        if (isGrounded || doubleJump)
        {
            doubleJump = false;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        if (checkWallCol)
        {
            transform.Rotate(0, 180, 0);
            dirX *= -1;
            rb.velocity = new Vector3(dirX * speed, jumpForce, rb.velocity.z);
        }

        if (checkWallDrop)
        {
            transform.Rotate(0, 180, 0);
            dirX *= -1;
            rb.velocity = new Vector3(dirX * speed, jumpForce / 3, rb.velocity.z);
        }
    }

    private void HandleSlide()
    {
        if (!isGrounded) return;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / 5, rb.velocity.z);
        col.height = 1;
        state = MovementState.slide;
    }

    private void StopSliding()
    {
        if (state == MovementState.slide) state = null;
        col.height = 2;
    }

//Player Movement function
    void MoveUpdate()
    {
        //Move on X Direction when dirX (1 or 0) * speed considering Y velocity
        rb.velocity = new Vector3(dirX * speed, rb.velocity.y, rb.velocity.z);

        if (isGrounded) doubleJump = true;

        //Get Button Input for Jump
        if (Input.GetButtonDown("Jump") && isGrounded || doubleJump && Input.GetButtonDown("Jump"))
        {
            doubleJump = false;

            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

//Checking if player is grounded to be able to jump / disables flying
    private void Checks()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundedCheckDistance, groundMask);
        groundFall = Physics.CheckSphere(transform.position + new Vector3(0.3f, 1.6f, 0), 0.2f, groundMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(0.3f, 1.6f, 0), 0.2f);
    }

//Animation Update for Running and Jumping
    private void UpdateAnimation()
    {
        float speedT = 0.45f;

        if (state != MovementState.slide)
            state = isGrounded && rb.velocity.x != 0f ? MovementState.run : MovementState.jump;

        if (checkTrg)
        {
            state = MovementState.climb;
            transform.position = new Vector3(hangCol.position.x, hangCol.position.y, transform.position.z);
            groundFall = false;
        }

        if (checkWallCol || checkWallDrop)
        {
            state = MovementState.hang;
            rb.velocity = new Vector3(dirX * speedT, rb.velocity.y, rb.velocity.z);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            dirX = 0;
            state = MovementState.idle;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            dirX = 1;
            state = MovementState.run;
        }

        if (groundFall)
        {
            rb.velocity += new Vector3(-1, -.75f, 0);
        }

        //anim.SetInteger("state", (int)state);
    }

//Level ontrigger for Finishing Level
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            dirX = 0;
            jumpForce = 0;
            transform.Rotate(0, 90, 0);
            //anim.SetTrigger("finish");
            OnPlayerDestinationReach?.Invoke();
        }
    }

    public void Climb()
    {
        //anim.SetInteger("state", (int)MovementState.run);
        transform.position = new Vector3(climbCol.position.x, climbCol.position.y, transform.position.z);
        checkTrg = false;
    }

    [SerializeField] private Vector3 respawnpoint;
    public GameObject drita;
    public GameObject drita2;
    public GameObject drita3;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava")) HandleGameOver();
        if (other.CompareTag("Water")) HandleGameOver();
        if (other.CompareTag("spikeball"))
        {
            HandleGameOver();
        }
        else if (other.CompareTag("Checkpoint"))
        {
            respawnpoint = transform.position;
            if (drita != null) drita.SetActive(true);
            if (drita2 != null) drita2.SetActive(false);
        }
        else if (other.CompareTag("Checkpoint1"))
        {
            respawnpoint = transform.position;
            if (drita2 != null) drita2.SetActive(true);
        }
        else if (other.CompareTag("Checkpoint3"))
        {
            respawnpoint = transform.position;
            if (drita3 != null) drita3.SetActive(true);
        }

        if (other.CompareTag("Finish")) dirX = .75f;

        if (other.gameObject.CompareTag("hangable"))
        {
            hangCol = other.transform.GetChild(0);
            climbCol = other.transform.GetChild(1);
            checkTrg = true;
        }

        if (other.gameObject.CompareTag("Checkpoint")) checkpoint = other.GetComponent<Transform>();

        if (other.gameObject.CompareTag("DirCorrector"))
        {
            if (rb.velocity.x < 0)
            {
                transform.Rotate(0, 180, 0);
                dirX *= -1;
            }
        }
    }

    private void HandleGameOver()
    {
        OnPlayerLose?.Invoke();
        OnPlayerRespawn?.Invoke(respawnpoint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obsticle")) HandleGameOver();

        if (collision.gameObject.CompareTag("mushroom"))
        {
            dirX *= 1;
            rb.velocity = new Vector3(dirX * speed, jumpForce * 1.2f, rb.velocity.z);
        }

        if (collision.gameObject.CompareTag("WallHang")) checkWallCol = true;
        if (collision.gameObject.CompareTag("WallDrop")) checkWallDrop = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("WallHang")) checkWallCol = false;
        if (collision.gameObject.CompareTag("WallDrop")) checkWallDrop = false;
    }
}