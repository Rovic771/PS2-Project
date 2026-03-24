using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crocheScipt : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float timeBeforeDestroy = 3f;
    [SerializeField] private LayerMask _layerMask;
    private Transform viseur;
    private Transform crocheSpawn;
    private PlayerController _playerController;
    private Vector3 directionTir;
    private GameObject player;

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }
    
    public void Start()
    {
        viseur = GameObject.Find("viseur").transform;
        directionTir = (viseur.transform.position - player.transform.position).normalized;
        crocheSpawn = GameObject.FindWithTag("crocheSpawn").transform;
        player = GameObject.FindWithTag("Player");
        _playerController = player.GetComponent<PlayerController>();
        Debug.Log(crocheSpawn);
        //ChangeShotState();
        StartCoroutine(DestroyTime());
    }

    public void ChangeShotState()
    {
        /*
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
        */
    }
    

    public void ChangeCrocheRotation(Quaternion quaternion)
    {
        transform.rotation = quaternion;
    }

    public void FixedUpdate()
    {
        transform.position += new Vector3(directionTir.x * speed, directionTir.y * speed, 0) * Time.deltaTime;
        
        /*
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
        }*/
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
