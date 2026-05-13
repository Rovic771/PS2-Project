using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class crocheScipt : MonoBehaviour
{
    [SerializeField] private float speedProjectile = 5f;
    [SerializeField] private float timeBeforeDestroy = 3f;
    private Rigidbody2D rbProjectile;
    private Vector2 directionTir;

    private int damageProjectile;
    //private PlayerController _playerController;
    
    public void Launch(Vector2 direction, bool fromPlayer, int damage)
    {
        if (fromPlayer)
        {
            if (gameObject.CompareTag("DoProjectile"))
            {
                gameObject.layer = LayerMask.NameToLayer("DoProjectileAlly");
            }
            else if (gameObject.CompareTag("ReProjectile"))
            {
                gameObject.layer = LayerMask.NameToLayer("ReProjectileAlly");
            }
        }
        else
        {
            if (gameObject.CompareTag("DoProjectile"))
            {
                gameObject.layer = LayerMask.NameToLayer("DoProjectileEnemy");
            }
            else if (gameObject.CompareTag("ReProjectile"))
            {
                gameObject.layer = LayerMask.NameToLayer("ReProjectileEnemy");
            }
            
        }

        damageProjectile = damage;
        directionTir = direction.normalized;
        Destroy(gameObject, timeBeforeDestroy);
    }

    public void Awake()
    {
        rbProjectile = GetComponent<Rigidbody2D>();
    }
    

    public void FixedUpdate()
    {
        rbProjectile.linearVelocity = new Vector2(directionTir.x * speedProjectile, directionTir.y * speedProjectile);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Destructable"))
        {
            if (gameObject.CompareTag("DoProjectile")  && other.gameObject.layer == LayerMask.NameToLayer("DoObject"))
            {
                Destroy(other.gameObject);
            }
            else if (gameObject.CompareTag("ReProjectile") && other.gameObject.layer == LayerMask.NameToLayer("ReObject"))
            {
                Destroy(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            bool projectileTouchFromRight;
            if (transform.position.x > other.gameObject.transform.position.x)
            {
                projectileTouchFromRight = true;
            }
            else
            {
                projectileTouchFromRight = false;
            }
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            //playerController.TakeDamage(damageProjectile);
            playerController.KnockBack(projectileTouchFromRight);
        }
        Destroy(gameObject);
    }
}
