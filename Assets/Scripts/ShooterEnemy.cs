using System;
using System.Collections;
using UnityEngine;

public class ShooterEnemy : Enemy
{ 
    [SerializeField] private GameObject viseurAncragePoint;
    [SerializeField] private GameObject viseurStartProjectile;
    [SerializeField] private GameObject doProjectile;
    [SerializeField] private GameObject reProjectile;
    public bool isWalking;
    public bool canShoot = true;
    

    public void Shoot()
    {
        canShoot = true;
        GameObject projectile;
        if (typeEnemy == EnemyType.Re)
        {
            projectile = reProjectile;
        }
        else
        {
            projectile = doProjectile;
        }
        GameObject proj = Instantiate(projectile, viseurStartProjectile.transform.position, Quaternion.identity);
        Vector2 direction = (viseurStartProjectile.transform.position - viseurAncragePoint.transform.position).normalized;
        proj.GetComponent<crocheScipt>().Launch(direction, false, damage);
        canShoot = false;
    }

    /*
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall())
        {
            playerDetected = true;
        }
        else playerDetected = false;
    }*/

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
            ResetEnemyState();
            playerDetected = false;
        }
    }

    public override void ResetEnemyState()
    {
        canShoot = true;
    }

    private void LookToPlayer()
    {
        Vector2 direction = player.transform.position - viseurStartProjectile.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!isFacingRight) angle += 20;
        viseurAncragePoint.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        switch (playerDetected)
        {
            case true:
                isWalking = false;
                LookToPlayer();
                animator.SetTrigger("isShot");
                break;
            case false:
                isWalking = true;
                break;
        }
        
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("canShoot", canShoot);
        animator.SetFloat("life", life);
    }
}
