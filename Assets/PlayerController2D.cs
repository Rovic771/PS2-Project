using UnityEngine;
using UnityEngine.InputSystem;

 public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 7f;

    [Header("Jump")]
    public float jumpForce = 14f;
    public float jumpCutMultiplier = 0.5f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Wall")]
    public float wallSlideSpeed = 2f;
    public float wallJumpForceX = 10f;
    public float wallJumpForceY = 14f;

    [Header("Checks")]
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    private Rigidbody2D rb;
    private InputSystem_Actions input; // <-- nom de ta classe Input Actions

    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpHeld;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;

    private float coyoteCounter;
    private float jumpBufferCounter;

    private int wallDirection;

    
    [SerializeField] GameObject NotePrefab;
    [SerializeField] Transform NoteSpawn;
    [SerializeField] float ProjectileSpeed = 15f;
    [SerializeField] float fireRate = 0.5f;

    private float nextFireTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new InputSystem_Actions();

        // Mouvement
        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Saut
        input.Player.Jump.performed += ctx =>
        {
            jumpPressed = true;
            jumpHeld = true;
        };
        input.Player.Jump.canceled += ctx =>
        {
            jumpHeld = false;
        };
    }

    private void OnEnable() => input.Enable();
    private void OnDisable() => input.Disable();

    private void Update()
    {
        CheckSurroundings();
        HandleTimers();
        HandleJump();
        HandleWallSlide();

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        if (moveInput.x > 0)
            transform.localScale = Vector3.one;
        else if (moveInput.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        wallDirection = transform.localScale.x > 0 ? 1 : -1;
    }

    void HandleTimers()
    {
        // Coyote time
        coyoteCounter = isGrounded ? coyoteTime : coyoteCounter - Time.deltaTime;

        // Jump buffer
        jumpBufferCounter = jumpPressed ? jumpBufferTime : jumpBufferCounter - Time.deltaTime;
    }

    void HandleJump()
    {
        // Wall jump prioritaire
        if (jumpBufferCounter > 0 && isWallSliding)
        {
            rb.linearVelocity = new Vector2(-wallDirection * wallJumpForceX, wallJumpForceY);
            jumpBufferCounter = 0;
            jumpPressed = false;
            return;
        }

        // Jump normal avec coyote + buffer
        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0;
        }

        // Variable jump height
        if (!jumpHeld && rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);

        jumpPressed = false;
    }

    void HandleWallSlide()
    {
        if (isTouchingWall && !isGrounded && moveInput.x != 0 && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
        else
            isWallSliding = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        if (wallCheck != null)
            Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(
            NotePrefab,
            NoteSpawn.position,
            Quaternion.identity
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        rb.linearVelocity = Vector2.right * direction * ProjectileSpeed;
    }
}