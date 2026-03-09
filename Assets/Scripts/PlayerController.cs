using System;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    
    private Vector2 moveInput;
    private bool isGrounded = true;
    private bool isWall;
    private bool isCharging = false;
    private bool isFacingRight = true;
    private bool mouvement = false;
    private bool canDoubleJump = true;
    private bool isWallSliding;
    private float wallSlidingSpeed = 0.2f;
    public bool directionTir;

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
                Debug.Log(Force);
                rb.AddForce(Force, ForceMode2D.Impulse);
                Debug.Log("wall jump effectué");
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
                Instantiate(croche, transform.position + new Vector3(crocheSpawn.transform.position.x,0,0), croche.transform.rotation);
            }
            else
            {
                Instantiate(croche, transform.position + new Vector3(0,-2,0), croche.transform.rotation);
            }
            
        }
    }
    
    public void FixedUpdate()
    {
        //transform.Translate(moveInput * speed * Time.deltaTime, Space.World);  Je sais pas quelle méthode elle la meilleure pour le déplacement du perso
        if (mouvement == true && isGrounded && rb.linearVelocity.magnitude < 1f)
        {
            rb.AddForce(new Vector2(moveInput.x * speed, moveInput.y * speed));
            Debug.Log("Mouvement au sol: " + moveInput.x);
        }
        else if (mouvement == true && !isGrounded)
        {
            rb.AddForce(new Vector2(Mathf.Clamp(moveInput.x * airControlIntensity, -airControlX, airControlX), rb.linearVelocity.y));
            Debug.Log("Mouvement en l'air: " + moveInput.x);
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

        if (crocheSpawn.transform.localScale.x > 0)
        {
            directionTir = true;
        }
        else if (croche.transform.localScale.x < 0)
        {
            directionTir = false;
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
            Debug.Log(isGrounded);
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
            Debug.Log(isGrounded);
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
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        crocheSpawn.transform.localScale = localScale;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer); //(où est le truc qui détecte, la taille du rayon de cercle, avec quoi il intéragit)
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
