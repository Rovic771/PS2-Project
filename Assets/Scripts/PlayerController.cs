using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float airControlIntensity = 5f;
    [SerializeField] private float airControlLimit = 5f;
    [SerializeField] private float jumpStrength = 8f;
    [SerializeField] private float doubleJumpStrength = 5f;
    [SerializeField] private float forceWallJumpX = 2f;
    [SerializeField] private float forceWallJumpY = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject viseurAncragePoint;
    [SerializeField] private GameObject viseurStartProjectile;
    [SerializeField] private GameObject doZone;
    [SerializeField] private GameObject reZone;
    [SerializeField] private GameObject doProjectile;
    [SerializeField] private GameObject reProjectile;
    [SerializeField] private float wallSlidingSpeed = 0.2f;
    
    private Vector2 moveInput;
    private Vector2 inputRotation;
    public bool isGround; 
    public bool isWall;
    private bool isFacingRight = true;
    private bool isFacingRightAim = true;
    public bool canDoubleJump;
    private bool isWallSliding;
    private float _currentAimAngle;
    private float _lastAimAngle;
    public bool canShoot = true;
    float _flipValue = 0;
    private Vector3 posInit;
    public bool zoneActive = true; // true c do et false c re
    private bool isWalking;
    private bool isJump;
    private float coyoteTimer;

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(1);
        canShoot = true;
    }
    
    void Awake()
    {
        rb.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        posInit = transform.position;
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (context.performed)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (isGround)
            {
                rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
                isGround = false;
                Debug.Log("isGround" + isGround);
                isJump = true;
            }
        else
        {
            if (IsWalled())
            {
                Vector2 Force = new Vector2(0,0);
                rb.linearVelocity = Vector2.zero;
                if (isFacingRight)
                {
                    Force = new Vector2(-transform.localScale.x * forceWallJumpX, forceWallJumpY);
                }
                else
                {
                    Force = new Vector2(transform.localScale.x * forceWallJumpX, forceWallJumpY);
                }
                Flip();
                rb.AddForce(Force, ForceMode2D.Impulse); ;
                Debug.Log("WallJump");
            }
            if (canDoubleJump && !isGround && !isWall)
            {
                {
                    Debug.Log("Double Jump");
                    rb.AddForce(Vector2.up * doubleJumpStrength, ForceMode2D.Impulse);
                    canDoubleJump = false;
                }
                isGround = false;
            }
        }
    }

    public void DoProtectionZone(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            zoneActive = true;
            reZone.SetActive(false);
            doZone.SetActive(true);
        }
        if (context.canceled) 
        {
            doZone.SetActive(false);
        }
    }
    
    public void ReProtectionZone(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            zoneActive = false;
            doZone.SetActive(false);
            reZone.SetActive(true);
        }
        if (context.canceled)
        {
            reZone.SetActive(false);
        }
    }
    
    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 inputRotationJSD = context.ReadValue<Vector2>(); // JSD = joystick droit
        if (context.performed)
        {
            if (inputRotationJSD.x < 0.1 && inputRotationJSD.y < 0.1 && inputRotationJSD.x > -0.1 && inputRotationJSD.y > -0.1) // si le joystick est pas touché
            {
                _currentAimAngle = _lastAimAngle; // le dernier input donné
            }
            else
            {
                _currentAimAngle = Mathf.Atan2(inputRotationJSD.y, inputRotationJSD.x) * Mathf.Rad2Deg;
                _lastAimAngle = _currentAimAngle;
                if (!isWalking && isGround)
                {
                    if ((_currentAimAngle > 90 || _currentAimAngle < -90) && isFacingRight)
                    {
                        Flip();
                    }
                    else if ((_currentAimAngle < 90 && _currentAimAngle > -90) && !isFacingRight)
                    {
                        Flip();
                    }
                }
                if(canShoot)
                {
                    if (zoneActive)
                    {
                        Instantiate(doProjectile, transform.position, viseurStartProjectile.transform.rotation);
                    }
                    else
                    {
                        Instantiate(reProjectile, transform.position, viseurStartProjectile.transform.rotation);
                    }
                    canShoot = false;
                    StartCoroutine(ShootDelay());
                }
            }
            viseurAncragePoint.transform.rotation = Quaternion.Euler(0, 0, _currentAimAngle);
        }
    }
    
    public void FixedUpdate()
    {
        
        if (IsGrounded())
        {
            Debug.Log("isGround" + isGround);
            coyoteTimer = coyoteTime;
            isGround = true;
            canDoubleJump = true;
            isJump = false;
        }
        else if(!isJump)
        {
            Debug.Log("isGround" + isGround);
            coyoteTimer -= Time.deltaTime;
            if (coyoteTimer <= 0)
            {
                Debug.Log("isGround" + isGround);
                isGround = false;
            }
        }

        if (IsWalled())
        {
            isWall = true;
        }
        else
        {
            isWall = false;
        }
        
        if (isGround)
        {
            //Debug.Log("Vitesse X" + rb.linearVelocity.x);

            if (Mathf.Abs(moveInput.x) > 0.05f) 
            {
                rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
            }
            else 
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }
        else
        {
            //Debug.Log("Vitesse Y" + rb.linearVelocity.y);
            if (Mathf.Abs(moveInput.x) > 0.05f && Mathf.Abs(rb.linearVelocity.x) < airControlLimit)
            {
                rb.AddForce(new Vector2(moveInput.x * airControlIntensity, 0));
            }
        }
        
        
        if (moveInput.x > 0.2f && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < -0.2f && isFacingRight)
        {
            Flip();
        }
        WallSlide();
    }
    



    private void Flip()
    {
        if (isGround)
        {
            rb.linearVelocity= new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y);
        }
        isFacingRight = !isFacingRight;
        _flipValue += 180;
        transform.rotation = Quaternion.Euler(0, _flipValue, 0);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.2f, 2f), 0f,wallLayer); //(où est le truc qui détecte, la taille du rayon de cercle, avec quoi il intéragit) cépadélia
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.4f, 0.1f), 0f, groundLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && isGround == false)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    public void Die()//c temporaire je la mettrais autre part plus tard
    {
        transform.position = posInit;
        rb.linearVelocity = Vector2.zero;
    }

    [SerializeField] private float test = 2f;
    
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, new Vector3(1f, 0.1f,0)); 
        }
        
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(wallCheck.position, new Vector3(0.2f, 2, 0));
        }
    }
}


