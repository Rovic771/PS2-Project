using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
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
    [SerializeField] private float coyoteTimeWallJump = 0.5f;
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
    [SerializeField] private float knockbackForceX = 10f;
    [SerializeField] private float knockbackForceY = 10f;
    
    [Header("Animator")]
    [SerializeField] private Animator animator;
    
    [Header("Statistiques")] 
    [SerializeField] private PlayerData _playerData;
    public int life;
    public int damage;
    
    
    [Header("Autre")]
    private Vector2 moveInput;
    private Vector2 inputRotation;
    public bool isGround; 
    public bool isWall;
    public bool isFall;
    public bool isShoot = false;
    private bool wasWalled;
    public bool isFacingRight = true;
    private bool isFacingRightAim = true;
    public bool canDoubleJump;
    public bool isDoubleJump;
    public bool isWallJump;
    private bool isWallSliding;
    private float _currentAimAngle;
    private float _lastAimAngle;
    public bool canShoot = true;
    float _flipValue = 0;
    public Vector3 posInit;
    public bool zoneActive = true; // true c do et false c re
    private bool isWalking;
    public bool isJump;
    private float coyoteTimer;
    public static List<GameObject> currentPlateform = new List<GameObject>();
    private bool canFlip = true;
    public bool isKnockback = false;

    IEnumerator ShootDelay()
    {
        canShoot = false;
        isShoot = true;
        yield return new WaitForSeconds(0.1f);
        canShoot = true;
        isShoot = false;
    }

    IEnumerator KnockbackDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isKnockback = false;
    }
    
    void Awake()
    {
        rb.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        posInit = transform.position;
        if(animator is null) animator = GetComponentInChildren<Animator>();
        if (PlayerPrefs.HasKey("checkpointX"))
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat("checkpointX"), PlayerPrefs.GetFloat("checkpointY"));
        }

        life = _playerData.life;
        damage = _playerData.damage;
    }

    public void NewGame(InputAction.CallbackContext context)
    {
        PlayerPrefs.DeleteKey("checkpointX");
        PlayerPrefs.DeleteKey("checkpointY");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public void KnockBack(bool toucheFromRight)
    {
        StopCoroutine(KnockbackDelay());
        Vector2 force = Vector2.zero;
        if (toucheFromRight)
        {
            force = new Vector2(-knockbackForceX, knockbackForceY);
        }
        else
        {
            force = new Vector2(knockbackForceX, knockbackForceY);
        }
        isKnockback = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
        StartCoroutine(KnockbackDelay());
    }
    
    public void ApplyForce(string typeJump)
    {
        Vector2 force = Vector2.zero;
        switch (typeJump)
        {
            case "basicJump":
                force = Vector2.up * jumpStrength;
                isGround = false; 
                isJump = true;
                coyoteTimer = 0;
                break;
            case "wallJump":
                rb.linearVelocity = Vector2.zero;
                coyoteTimer = 0;
                isJump = true;
                isWallJump = true;
                if (isFacingRight)
                {
                    force = new Vector2(-transform.localScale.x * forceWallJumpX, forceWallJumpY);
                }
                else
                {
                    force = new Vector2(transform.localScale.x * forceWallJumpX, forceWallJumpY);
                }
                break;
            case "doubleJump" :
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                force = Vector2.up * doubleJumpStrength;
                canDoubleJump = false;
                isDoubleJump = true;
                break;
        }
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ApplyShoot()
    {
        StartCoroutine(ShootDelay());
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (isGround)
            {
                animator.SetTrigger("isJump");
            }
        else
        {
            if (IsWalled() || coyoteTimer > 0 && wasWalled)
            {
                animator.SetTrigger("isWallJump"); ;
            }
            if (canDoubleJump && !isGround && !isWall)
            {
                {
                    animator.SetTrigger("isDoubleJump");
                }
                isGround = false;
            }
        }
    }

    public void DoShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canShoot)
            {
                GameObject proj = Instantiate(doProjectile, viseurAncragePoint.transform.position, Quaternion.identity);
                Vector2 direction = (viseurStartProjectile.transform.position - viseurAncragePoint.transform.position).normalized;
                proj.GetComponent<crocheScipt>().Launch(direction, true, damage);
                animator.SetTrigger("isShoot");
            }
        }
    }

    public void ReShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(canShoot)
            {
                GameObject proj = Instantiate(reProjectile, viseurAncragePoint.transform.position, Quaternion.identity);
                Vector2 direction = (viseurStartProjectile.transform.position - viseurAncragePoint.transform.position).normalized;
                proj.GetComponent<crocheScipt>().Launch(direction, true, damage);
                animator.SetTrigger("isShoot");
            }
        }
    }

    public void ActiveColliderZone(string typeZone)
    {
        switch (typeZone)
        {
            case "do":
                doZone.GetComponent<CircleCollider2D>().enabled = true;
                break;
            case "re":
                reZone.GetComponent<CircleCollider2D>().enabled = true;
                break;
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
            doZone.GetComponent<CircleCollider2D>().enabled = false;
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
            reZone.GetComponent<CircleCollider2D>().enabled = false;
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
            }
            viseurAncragePoint.transform.rotation = Quaternion.Euler(0, 0, _currentAimAngle);
        }
    }
    
    public void FixedUpdate()
    {
        if (IsGrounded() && !isJump)
        {
            coyoteTimer = coyoteTime;
            isGround = true;
            canDoubleJump = true;
            isFall = false;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            if (coyoteTimer <= 0)
            {
                isGround = false;
            }
        }

        if (IsWalled() && !wasWalled)
        {
            coyoteTimer = coyoteTimeWallJump;
            wasWalled = true;
            canFlip = false;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            if (coyoteTimer <= 0)
            {
                canFlip = true;
                wasWalled = false;
            }
        }

        if (rb.linearVelocity.y <= -0.1f)
        {
            isJump = false;
            isDoubleJump = false;
            isFall = true;
        }

        if (IsWalled())
        {
            isWall = true;
        }
        else
        {
            isWall = false;
        }
        
        if (isGround && !isKnockback)
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
                rb.AddForce(new Vector2(moveInput.x * airControlIntensity, 0), ForceMode2D.Force);
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
        
        animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("isGrounded", isGround);
        animator.SetBool("isDoubleJump", isDoubleJump);
        animator.SetBool("isWall", isWall);
        animator.SetBool("isFall", isFall);
        animator.SetBool("canDoubleJump", canDoubleJump);
        animator.SetBool("canShoot", canShoot); 
    }
    
    private void Flip()
    {
        if (canFlip)
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
            animator.gameObject.transform.rotation = Quaternion.Euler(0, _flipValue, 0);
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.2f, 2f), 0f,wallLayer); //(où est le truc qui détecte, la taille du rayon de cercle, avec quoi il intéragit) cépadélia
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(1f, 0.2f), 0f, groundLayer);
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

    public void TakeDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Die();
        }
    }
    
    public void Die()//c temporaire je la mettrais autre part plus tard
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


