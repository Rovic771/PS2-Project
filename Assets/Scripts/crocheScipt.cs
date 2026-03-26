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

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }
    
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        rbProjectile = GetComponent<Rigidbody2D>();
        viseurPos = GameObject.FindWithTag("Viseur").transform.position;
        directionTir = (viseurPos - player.transform.position).normalized;
        StartCoroutine(DestroyTime());
    }

    public void FixedUpdate()
    {
        rbProjectile.linearVelocity = new Vector2(directionTir.x * speedProjectile, directionTir.y * speedProjectile);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3)
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
