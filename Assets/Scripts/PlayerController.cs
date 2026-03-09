using System;
using System.Collections;
using System.Numerics;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float gravity = -12f;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float airControlIntensity = 5.0f;
    [SerializeField] private float airControlX = 1.0f;
    [SerializeField] private float jumpStrengthHorizontal = 300.0f;
    [SerializeField] private float jumpStrengthVertical = 300.0f;
    [SerializeField] private float doubleJumpStrength = 150.0f;
    [SerializeField] private float forceWallJumpX = 2f;
    [SerializeField] private float forceWallJumpY = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject croche;
    [SerializeField] private Transform crocheSpawn;
    [SerializeField] private crocheScipt _crocheScipt;
    [SerializeField] private GameObject viseurTest;
    
    private Vector2 moveInput;
    private Vector2 inputRotation;
    public bool isGrounded = true;
    private bool isWall;
    private bool isCharging = false;
    private bool isFacingRight = true;
    private bool mouvement = false;
    private bool canDoubleJump = true;
    private bool isWallSliding;
    private float wallSlidingSpeed = 0.2f;
    private float _currentAimAngle;
    private float _lastAimAngle;
    float _flipValue = 0;

    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTime);
        isGrounded = false;
    }
    
    void Awake()
    {
        rb.GetComponent<Rigidbody2D>();
        Physics2D.gravity = new Vector2(0, gravity);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //rb.linearVelocity = new Vector3(moveInput.x * speed, rb.linearVelocity.y, moveInput.y * speed);
        if (context.started)
        {
            mouvement = true;
        }
        else if (context.canceled)
        {
            mouvement = false;
        }
        else if (context.performed)
        {
            //Debug.Log(context.ReadValue<Vector2>());
            inputRotation = context.ReadValue<Vector2>();
            Aim();
        }
        if (context.canceled && isGrounded)
        {
           rb.linearVelocity = Vector2.zero;
        }
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                if (moveInput.x != 0)
                {
                 rb.AddForce(Vector2.up * jumpStrengthHorizontal, ForceMode2D.Force);   
                }
                else
                {
                    rb.AddForce(Vector2.up * jumpStrengthVertical, ForceMode2D.Force);
                    isGrounded = false;
                }
            }
            else if (!isGrounded && IsWalled())
            {
                rb.linearVelocity = Vector2.zero;
                Vector2 Force = new Vector2(-transform.localScale.x * forceWallJumpX, forceWallJumpY);
                //Debug.Log(Force);
                rb.AddForce(Force, ForceMode2D.Impulse);
                //Debug.Log("wall jump effectué");
                IsWalled();
            }
            else if (!isGrounded && canDoubleJump)
            {
                rb.AddForce(Vector2.up * doubleJumpStrength, ForceMode2D.Force);
                canDoubleJump = false;
            }
        }

    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                //_crocheScipt.ChangeShotState();
                Instantiate(croche, transform.position + new Vector3(crocheSpawn.transform.localPosition.x,crocheSpawn.transform.localPosition.y,0), croche.transform.rotation);
            }
            else
            {
                Instantiate(croche, transform.position + new Vector3(0,-2,0), croche.transform.rotation);
            }
            
        }
    }
    
    private void Aim()
    {
        
        if (inputRotation.x < 0.1 && inputRotation.y < 0.1 && inputRotation.x > -0.1 && inputRotation.y > -0.1) // si le joystick est pas touché
        {
            _currentAimAngle = _lastAimAngle; // le dernier input donné
        }
        else
        {
            _currentAimAngle = Mathf.Atan2(inputRotation.y, inputRotation.x) * Mathf.Rad2Deg; // calcul de fou là
            _lastAimAngle = _currentAimAngle;
        }
        viseurTest.transform.rotation = Quaternion.Euler(0, 0, _currentAimAngle);
    }
    
    public void FixedUpdate()
    {
        if (mouvement == true && isGrounded && rb.linearVelocity.magnitude < 1f)
        {
            rb.AddForce(new Vector2(moveInput.x * speed, 0));
            //Debug.Log("Mouvement au sol: " + moveInput.x);
        }
        else if (mouvement == true && !isGrounded)
        {
            rb.AddForce(new Vector2(Mathf.Clamp(moveInput.x * airControlIntensity, -airControlX, airControlX), 0));
            //Debug.Log("Mouvement en l'air: " + moveInput.x);
        }
        /*
        if (isCharging == true && jumpStrength <= 10.0f)
        {
            jumpStrength += 30.0f * Time.deltaTime;
        }*/
        if (moveInput.x > 0 && !isFacingRight)
        {
            flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            flip();
        }
        WallSlide();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            rb.linearVelocity = Vector2.zero;
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
        isFacingRight = !isFacingRight;
        _flipValue += 180;
        Debug.Log(_flipValue);
        transform.rotation = Quaternion.Euler(0, _flipValue, 0);
        crocheSpawn.transform.localPosition *= -1;
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
}
