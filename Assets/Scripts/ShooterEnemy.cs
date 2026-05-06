using System;
using System.Collections;
using UnityEngine;

public class ShooterEnemy : Enemy
{    
    [SerializeField] private GameObject viseurAncragePoint;
    [SerializeField] private GameObject viseurStartProjectile;
    [SerializeField] private GameObject doProjectile;
    [SerializeField] private GameObject reProjectile;
    [SerializeField] private float shootDelay = 0.5f;
    private Animator animatorShooter;
    private bool isWalking;


    private IEnumerator ShootDelay()
    {
        GameObject projectile;
        yield return new WaitForSeconds(shootDelay);
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
        proj.GetComponent<crocheScipt>().Launch(direction, false);
        StartCoroutine(ShootDelay());
    }

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
        animatorShooter = GetComponent<Animator>();
        Debug.Log(animatorShooter);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall())
        {
            playerDetected = true;
            StartCoroutine(ShootDelay());
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StopAllCoroutines();
            playerDetected = false;
        }
    }

    private void LookToPlayer()
    {
        Vector2 direction = player.transform.position - viseurAncragePoint.transform.position;
        float angle = Mathf.Atan2(direction.y + 1, direction.x) * Mathf.Rad2Deg;
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
                break;
            case false:
                isWalking = true;
                break;
        }
        
        animatorShooter.SetBool("isWalking", isWalking);
    }
}
