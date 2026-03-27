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
    [SerializeField] private GameObject viseurAncragePoint;
    [SerializeField] private GameObject viseurStartProjectile;
    [SerializeField] private GameObject doZone;
    [SerializeField] private GameObject reZone;
    [SerializeField] private GameObject doProjectile;
    [SerializeField] private GameObject reProjectile;
    
    private Vector2 moveInput;
    private Vector2 inputRotation;
    public bool isGrounded = true;
    private bool isWall;
    private bool isFacingRight = true;
    private bool canDoubleJump = true;
    private bool isWallSliding;
    private float wallSlidingSpeed = 0.2f;
    private float _currentAimAngle;
    private float _lastAimAngle;
    public bool canShoot = true;
    float _flipValue = 0;
    private Vector3 posInit;
    public bool zoneActive = true; // true c do et false c re

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
        if (isGrounded)
        {
            isGrounded = false;
        }
        /*
        else if (!isGrounded && IsWalled())
        {
            isWall = false;
        }*/
    }

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
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
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
                rb.AddForce(Force, ForceMode2D.Impulse);
                Debug.Log("WallJump");
            }
            if (canDoubleJump && !IsWalled())
            {
                {
                    Debug.Log("Double Jump");
                    rb.AddForce(Vector2.up * doubleJumpStrength, ForceMode2D.Impulse);
                    canDoubleJump = false;
                }
                isGrounded = false;
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
    }
    
    public void ReProtectionZone(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            zoneActive = false;
            doZone.SetActive(false);
            reZone.SetActive(true);
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
        if (isGrounded)
        {
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
            if (Mathf.Abs(moveInput.x) > 0.05f && Mathf.Abs(rb.linearVelocity.x) < airControlLimit)
            {
                rb.AddForce(new Vector2(moveInput.x * airControlIntensity, 0));
            }
        }
        
        if (moveInput.x > 0.2f && !isFacingRight)
        {
            flip();
        }
        else if (moveInput.x < -0.2f && isFacingRight)
        {
            flip();
        }
        WallSlide();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            canDoubleJump = true;
            //Debug.Log(isGrounded);
        }

        if (IsWalled())
        {
            isWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            StartCoroutine(CoyoteTime());
            //jumpStrength = 1.0f;
            //Debug.Log(isGrounded);
        }
        if (!IsWalled())
        {
            isWall = false;
        }
        /*
        if (other.gameObject.CompareTag("Wall"))
        {
            isGrounded = false;
            jumpStrength = 1.0f;
        }
        */
    }

    private void flip()
    {
        if (isGrounded)
        {
            rb.linearVelocity= new Vector2(0, rb.linearVelocity.y);
        }
        isFacingRight = !isFacingRight;
        _flipValue += 180;
        transform.rotation = Quaternion.Euler(0, _flipValue, 0);
        Debug.Log("flip :" + isFacingRight);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer); //(où est le truc qui détecte, la taille du rayon de cercle, avec quoi il intéragit) cépadélia
    }

    private void WallSlide()
    {
        if (IsWalled() && isGrounded == false)
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
    
}
