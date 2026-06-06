using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// elle famoso crochescipt
public class crocheScipt : MonoBehaviour
{
    [SerializeField] private float speedProjectile = 5f;
    [SerializeField] private float timeBeforeDestroy = 3f;
    private Rigidbody2D rbProjectile;
    private Vector2 directionTir;
    private float knockbackOnPlayerX;
    private float knockbackOnPlayerY;

    // pas utilisé?
    private int damageProjectile;
    //private PlayerController _playerController;
    
    public void Launch(Vector2 direction, bool fromPlayer, int damage, float kbX = 0, float kbY = 0)
    {
        if (fromPlayer)
        {
            if (gameObject.CompareTag("DoProjectile"))
            {
                // ptet voir à stocker tous ces nametolayer
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

        knockbackOnPlayerX = kbX;
        knockbackOnPlayerY = kbY;
        damageProjectile = damage;
        directionTir = direction.normalized;
        Destroy(gameObject, timeBeforeDestroy);
    }

    // perso j'suis team "start et awake en haut" pour avoir une logique de lecture claire:
    // on commence -> start awake, on update, on fait nos fonctions, et à la fin, les ondestroy
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
            playerController.KnockBack(projectileTouchFromRight, knockbackOnPlayerX, knockbackOnPlayerY);
        }
        Destroy(gameObject);
    }
}
