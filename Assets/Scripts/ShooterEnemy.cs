using System;
using System.Collections;
using UnityEngine;

public class ShooterEnemy : Enemy
{ 
    [SerializeField] private GameObject viseurAncragePoint;
    [SerializeField] private GameObject viseurStartProjectile;
    [SerializeField] private GameObject doProjectile;
    [SerializeField] private GameObject reProjectile;
    [SerializeField] private float knockbackOnPlayerForceX = 10f;
    [SerializeField] private float knockbackOnPlayerForceY = 10f;
    [SerializeField] private float delayShoot;
    private bool canShoot = true;
    
    [Header("Audio")]
    [SerializeField] private float volumeAttackDo = 1f;
    [SerializeField] private float volumeAttackRe = 1f;

    private IEnumerator DelayShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(delayShoot);
        canShoot = true;
    }

    public void Shoot()
    {
        GameObject projectile;
        if (canShoot)
        {
            if (typeEnemy == EnemyType.Re)
            {
                projectile = reProjectile;
                AudioManager.Instance.PlaySoundEnemy(AudioManager.EnemySound.AttackRé, volumeAttackRe);
            }
            else
            {
                projectile = doProjectile;
                AudioManager.Instance.PlaySoundEnemy(AudioManager.EnemySound.AttackDo, volumeAttackDo);
            }
            GameObject proj = Instantiate(projectile, viseurStartProjectile.transform.position, Quaternion.identity);
            Vector2 direction = (viseurStartProjectile.transform.position - viseurAncragePoint.transform.position).normalized;
            proj.GetComponent<crocheScipt>().Launch(direction, false, damage, knockbackOnPlayerForceX, knockbackOnPlayerForceY);
            StartCoroutine(DelayShoot());
        }
    }
    

    public override void Init()
    {
        animator = GetComponent<Animator>();
        classEnemy = ClassEnemy.violon;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall())
        {
            playerDetected = true;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isStun || TestIfWall())
            {
                playerDetected = false;
                canShoot = true;
                StopCoroutine(DelayShoot());
            }
        }
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
            canShoot = true;
			StopCoroutine(DelayShoot());
        }
    }

    public override void ResetEnemyState()
    {
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
        isIddle = false;
        switch (playerDetected)
        { 
            case true:
                isWalking = false;
                LookToPlayer();
                animator.SetTrigger("isShot");
                break;
            case false:
                if(basicMove)
                {
                    isWalking = true;
                    break;
                }
                isWalking = false;
                isIddle = true;
                break;
        }
        
        animator.SetBool("isWalking", isWalking);
        animator.SetFloat("life", life);
        animator.SetBool("isIddle", isIddle);
        animator.SetBool("canShoot", canShoot);
    }
}
