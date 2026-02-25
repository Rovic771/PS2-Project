using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpStrength = 1.0f;
    [SerializeField] private Rigidbody2D rb;
    
    private Vector2 moveInput;
    private bool isGrounded = true;
    private bool isWall;
    private bool isCharging = false;
    private bool isFacingRight = true;
    
    private bool isWallSliding;
    private float wallSlidingSpeed = 0.2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    //[SerializeField] private Vector2 wallJumpDirection = new Vector2();
    

    void Awake()
    {
        rb.GetComponent<Rigidbody2D>();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded == true)
        {
            if (context.performed) // à la base ct started
            {
                //isCharging = true;
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpStrength, ForceMode2D.Force);
                //Debug.Log(isGrounded);
            }
            if (context.canceled)
            {
                //isCharging = false;
                Debug.Log("Jump");
            }
        }
        else if (!isGrounded && isWall)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-transform.localScale.x * jumpStrength * 10, 300));
            Debug.Log("caca boudin qui pu");
        }

    }

    public void FixedUpdate()
    {
        //transform.Translate(moveInput * speed * Time.deltaTime, Space.World); -> Je sais pas quelle méthode elle la meilleure pour le déplacement du perso
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        rb.linearVelocity = new Vector3(moveInput.x * speed, rb.linearVelocity.y, moveInput.y * speed);
        if (isCharging == true && jumpStrength <= 10.0f)
        {
            jumpStrength += 30.0f * Time.deltaTime;
        }
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
            isGrounded = true;
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
            isGrounded = false;
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
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer); //(où est le truc qui détecte, la taille du rayon de cercle, avec quoi il intéragit)
    }

    private void WallSlide()
    {
        if (IsWalled() && isGrounded == false && moveInput.x != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,
                Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
}
