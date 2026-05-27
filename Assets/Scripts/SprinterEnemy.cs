using System;
using UnityEngine;

public class SprinterEnemy : Enemy
{
    [SerializeField] private float attackSpeed;

    /*
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Transform leftLimit;
    */
    //private Animator animatorSprinter;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isWalking;
    
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall())
        {
            playerDetected = true;
        }
        else playerDetected = false;
    }

    public override void Init()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall())
        {
            playerDetected = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerDetected = false;
        }
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun)
        {
            player.GetComponent<PlayerController>().TakeDamage(damage);
            Debug.Log(gameObject + " collisionne avec " + other.gameObject.name);
        }
    }

    private void PursuitPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, attackSpeed * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, groundLayer);
    }
    
    
    private bool EnemyInZone()
    {
        if (IsGrounded()) return true; 
        return false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (EnemyInZone())
        {
            switch (playerDetected)
            {
                case true:
                    isWalking = false;
                    PursuitPlayer();
                    break;
        
                case false:
                    isWalking = true;
                    break;
            }
        }
        else
        {
            animator.SetBool("isIddle", true);
        }
        
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("playerDetected", playerDetected);
    }
}
