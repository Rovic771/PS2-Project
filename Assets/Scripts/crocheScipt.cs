using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crocheScipt : MonoBehaviour
{
    [SerializeField] private float speedProjectile = 5f;
    [SerializeField] private float timeBeforeDestroy = 3f;
    private Rigidbody2D rbProjectile;
    private Vector2 directionTir;
    public void Launch(Vector2 direction)
    {
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
            if (gameObject.CompareTag("DoProjectile") && other.gameObject.layer == LayerMask.NameToLayer("DoObject"))
            {
                Destroy(other.gameObject);
            }
            else if (gameObject.CompareTag("ReProjectile") && other.gameObject.layer == LayerMask.NameToLayer("ReObject"))
            {
                Destroy(other.gameObject);
            }
        }
        else if(other.gameObject.CompareTag("DoProjectile") || other.gameObject.CompareTag("ReProjectile")) Destroy(gameObject);
        Destroy(gameObject);
    }
}
