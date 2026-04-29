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
    private Collider2D playerCollider;
    
    public void Launch(Vector2 direction, string tagName = "Projectile")
    {
        if (tagName == "AllyProjectile")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, true);
        }
        else
        {
            Debug.Log("BBBBBBBBBBBBBBBBBBBBBBBB");
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider, false);
        }
        Debug.Log(tagName);
        directionTir = direction.normalized;
        Destroy(gameObject, timeBeforeDestroy);
    }

    public void Awake()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
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
            if (gameObject.layer == LayerMask.NameToLayer("DoProjectile") && other.gameObject.layer == LayerMask.NameToLayer("DoObject"))
            {
                Destroy(other.gameObject);
            }
            else if (gameObject.layer == LayerMask.NameToLayer("ReProjectile") && other.gameObject.layer == LayerMask.NameToLayer("ReObject"))
            {
                Destroy(other.gameObject);
            }
        }

        
        Destroy(gameObject);
    }
}
