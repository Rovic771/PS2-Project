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
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Rigidbody rb;

    private float horizontal;
    private Vector2 moveInput;
    private bool isGrounded = true;
    private bool isCharging = false;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private bool isFacingRight = true;
    
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector3 wallJumpingPower = new Vector3(8f,0,16f);

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
                gameObject.GetComponent<Rigidbody>().AddForce(Vector2.up * jumpStrength, ForceMode.VelocityChange);
                //Debug.Log(isGrounded);
            }
            if (context.canceled)
            {
                //isCharging = false;
                Debug.Log("Jump");
            }
            if (context.started && wallJumpingCounter > 0f)
            {
                isWallJumping = true;
                rb.linearVelocity = new Vector3(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y, 0);
                wallJumpingCounter = 0f;
                
                if (transform.localScale.x != wallJumpingDirection)
                {
                    Vector3 localScale = transform.localScale;
                    localScale.x *= -1f;
                    transform.localScale = localScale;
                }

                Invoke(nameof(StopWallJumping), wallJumpingDuration);
            }
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
            Debug.Log(jumpStrength);
        }
        if (moveInput.x > 0 && !isFacingRight)
        {
            flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            flip();
        }
        //WallJump(); ça marche pas
        WallSlide();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            Debug.Log(isGrounded);
        }
        /*
        if (other.gameObject.CompareTag("Wall"))
        {
            isGrounded = true;
        }
        */
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
            //jumpStrength = 1.0f;
            Debug.Log(isGrounded);
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
    
    private bool isWalled()
    {
        return Physics.CheckSphere(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (isWalled() && isGrounded == false && moveInput.x != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue), rb.linearVelocity.z);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding == true)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
