using System;
using UnityEngine;

public class SprinterEnemy : Enemy
{
    [SerializeField] private float attackSpeed;
    //private Animator animatorSprinter;
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

    public override void FixedUpdate()
    {
        base.FixedUpdate();
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
        
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("playerDetected", playerDetected);
    }
}
