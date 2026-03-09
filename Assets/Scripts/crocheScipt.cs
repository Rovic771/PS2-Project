using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crocheScipt : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float timeBeforeDestroy = 3f;
    private PlayerController playerController;

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }
    
    public void Start()
    {
        Debug.Log("la croche spawn");
        GameObject Player = GameObject.FindWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
        StartCoroutine(DestroyTime());
    }

    public void FixedUpdate()
    {
        if (playerController.directionTir == true)
        {
            transform.position += new Vector3(speed,0,0) * Time.deltaTime;
            Debug.Log("coucou");
            Debug.Log("direction tir : " + playerController.directionTir);
        }
        else
        {
            transform.position += new Vector3(-speed,0,0) * Time.deltaTime;
            Debug.Log("caca");
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Croche detruite");
        Destroy(gameObject);
    }
}
