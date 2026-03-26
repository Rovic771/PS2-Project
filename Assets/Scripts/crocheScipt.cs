using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crocheScipt : MonoBehaviour
{
    [SerializeField] private float speedProjectile = 5f;
    [SerializeField] private float timeBeforeDestroy = 3f;
    private Rigidbody2D rbProjectile;
    private Vector3 viseurPos;
    private Vector3 directionTir;
    private GameObject player;
    private PlayerController playerController;

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }
    
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        rbProjectile = GetComponent<Rigidbody2D>();
        viseurPos = GameObject.FindWithTag("Viseur").transform.position;
        directionTir = (viseurPos - player.transform.position).normalized;
        StartCoroutine(DestroyTime());
        if (playerController.zoneActive)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("DoObject"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("ReObject"), false);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("DoObject"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Projectile"), LayerMask.NameToLayer("ReObject"), true);
        }
    }

    public void FixedUpdate()
    {
        rbProjectile.linearVelocity = new Vector2(directionTir.x * speedProjectile, directionTir.y * speedProjectile);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Destructable"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
