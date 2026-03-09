using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crocheScipt : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float timeBeforeDestroy = 3f;
    [SerializeField] private LayerMask _layerMask;
    private Transform crocheSpawn;
    private PlayerController _playerController;
    private string directionTir;

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }
    
    public void Start()
    {
        crocheSpawn = GameObject.FindWithTag("crocheSpawn").transform;
        GameObject Player = GameObject.FindWithTag("Player");
        _playerController = Player.GetComponent<PlayerController>();
        Debug.Log(crocheSpawn);
        ChangeShotState();
        StartCoroutine(DestroyTime());
    }

    public void ChangeShotState()
    {
        if (crocheSpawn.transform.localPosition.x > 0 && _playerController.isGrounded == true)
        {
            directionTir = "droite";
        }
        else if(crocheSpawn.transform.localPosition.x < 0 && _playerController.isGrounded == true)
        {
            directionTir = "gauche";
            ChangeCrocheRotation(new Quaternion(0,0, 90,1));
        }
        else if (_playerController.isGrounded == false)
        {
            directionTir = "bas";
        }
    }
    

    public void ChangeCrocheRotation(Quaternion quaternion)
    {
        transform.rotation = quaternion;
    }

    public void FixedUpdate()
    {
        switch (directionTir)
        {
            case "droite":
                transform.position += new Vector3(speed,0,0) * Time.deltaTime;
                break;
            
            case "gauche":
                transform.position += new Vector3(-speed,0,0) * Time.deltaTime;
                break;
            case "bas":
                transform.position += new Vector3(0,-speed, 0) * Time.deltaTime;
                break;
        }
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
