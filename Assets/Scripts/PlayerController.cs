using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
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
    
    [Header("Animator")]
    [SerializeField] private Animator animatorD;
    [SerializeField] private Animator animatorG;
    
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

    IEnumerator ShootDelay()
    {
        Debug.Log("tir");
        canShoot = false;
        isShoot = true;
        yield return new WaitForSeconds(0.1f);
        canShoot = true;
        isShoot = false;
        Debug.Log("CanShoot = " + canShoot);
        Debug.Log("Shoot Delay end");
    }
    
    void Awake()
    {
        rb.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        posInit = transform.position;
        if(animatorD is null) animatorD = GetComponentInChildren<Animator>();
        if (animatorG is null) animatorG = GetComponentInChildren<Animator>();
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

    public void ApplyForce(string typeJump)
    {
        Vector2 force = Vector2.zero;
        switch (typeJump)
        {
            case "basicJump":
                Debug.Log("basicJump");
                force = Vector2.up * jumpStrength;
                isGround = false; 
                isJump = true;
                coyoteTimer = 0;
                break;
            case "wallJump":
                Debug.Log("wallJump");
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
                Debug.Log("Double Jump");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                force = Vector2.up * doubleJumpStrength;
                canDoubleJump = false;
                isDoubleJump = true;
                break;
        }
        Debug.Log("Force ajoutée " + force);
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
                animatorD.SetTrigger("isJump");
                animatorG.SetTrigger("isJump");
            }
        else
        {
            if (IsWalled() || coyoteTimer > 0 && wasWalled)
            {
                animatorD.SetTrigger("isWallJump");
                animatorG.SetTrigger("isWallJump");
            }
            if (canDoubleJump && !isGround && !isWall)
            {
                {
                    animatorD.SetTrigger("isDoubleJump");
                    animatorG.SetTrigger("isDoubleJump");
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
                proj.GetComponent<crocheScipt>().Launch(direction, true);
                animatorD.SetTrigger("isShoot");
                animatorG.SetTrigger("isShoot");
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
                proj.GetComponent<crocheScipt>().Launch(direction, true);
                animatorD.SetTrigger("isShoot");
                animatorG.SetTrigger("isShoot");
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
                        isFacingRight = false;
                    }
                    else if ((_currentAimAngle < 90 && _currentAimAngle > -90) && !isFacingRight)
                    {
                        isFacingRight = true;
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
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
            if (coyoteTimer <= 0)
            {
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
        
        if (isGround)
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
            wallCheck.gameObject.transform.localPosition = new Vector2(1.2f, 2.5f);
            isFacingRight = true;
        }
        else if (moveInput.x < -0.2f && isFacingRight)
        {
            isFacingRight = false;
            wallCheck.gameObject.transform.localPosition =  new Vector2(-1.2f, 2.5f);
        }
        
        WallSlide();
        
        animatorD.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        animatorD.SetBool("isGrounded", isGround);
        animatorD.SetBool("isDoubleJump", isDoubleJump);
        animatorD.SetBool("isWall", isWall);
        animatorD.SetBool("isFall", isFall);
        animatorD.SetBool("canDoubleJump", canDoubleJump);
        animatorD.SetBool("canShoot", canShoot); 
        
        animatorG.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        animatorG.SetBool("isGrounded", isGround);
        animatorG.SetBool("isDoubleJump", isDoubleJump);
        animatorG.SetBool("isWall", isWall);
        animatorG.SetBool("isFall", isFall);
        animatorG.SetBool("canDoubleJump", canDoubleJump);
        animatorG.SetBool("canShoot", canShoot);
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

    public void Die()//c temporaire je la mettrais autre part plus tard
    {
        transform.position = posInit;
        rb.linearVelocity = Vector2.zero;
    }
}


