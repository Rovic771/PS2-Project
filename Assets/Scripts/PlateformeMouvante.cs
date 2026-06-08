using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeMouvante : MonoBehaviour
{
    [SerializeField] private GameObject origine;
    [SerializeField] List<GameObject> points = new List<GameObject>();
    [SerializeField] private float speed = 1;
    [SerializeField] public bool objectActive = false; 
    private Vector3 targetPos;
    public int currentPointIndex;
    private int previousPointIndex;

    private void Start()
    {
        transform.position = origine.transform.position;
        currentPointIndex = points.IndexOf(origine);
    }
    
    private void FixedUpdate()
    {
        if (objectActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                objectActive = false;
                if (points[currentPointIndex] == origine)
                {
                    IgnoreOrigine();
                    objectActive = true;
                }
            }
        }
    }

    private void IgnoreOrigine()
    {
        if (previousPointIndex < currentPointIndex)
        {
            currentPointIndex++;
        }
        else if (previousPointIndex > currentPointIndex)
        {
            currentPointIndex--;
        }
        targetPos = points[currentPointIndex].transform.position;
    }

    private void ChangeTargetPoint(string typeProjectile)
    {
        if (objectActive)
        {
            return;
        }
        
        if (typeProjectile == "Re")
        {
            if (currentPointIndex + 1 < points.Count)
            {
                previousPointIndex = currentPointIndex;
                currentPointIndex++;
            }
            else
            {
                return;
            }
        }
        else if (typeProjectile == "Do")
        {
            if (currentPointIndex - 1 >= 0)
            {
                previousPointIndex = currentPointIndex;
                currentPointIndex--;
            }
            else
            {
                return;
            }
        }
        objectActive = true;
        targetPos = points[currentPointIndex].transform.position;
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.currentPlateform.Add(gameObject);
            if (PlayerController.currentPlateform.Count == 1)
            {
                other.gameObject.transform.SetParent(PlayerController.currentPlateform[0].transform);
            }
            else
            {
                other.gameObject.transform.SetParent(PlayerController.currentPlateform[PlayerController.currentPlateform.Count - 1].transform);
            }
        }
        else if (other.gameObject.CompareTag("ReProjectile") || other.gameObject.CompareTag("DoProjectile"))
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("ReProjectileAlly"))
            {
                ChangeTargetPoint("Re");
            }
            else if(other.gameObject.layer == LayerMask.NameToLayer("DoProjectileAlly"))
            {
                ChangeTargetPoint("Do");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ReZone"))
        {
            ChangeTargetPoint("Re");
        }
        else if(other.gameObject.CompareTag("DoZone"))
        {
            ChangeTargetPoint("Do");
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController.currentPlateform.Remove(gameObject); 

            if (PlayerController.currentPlateform.Count > 0)
            {
                other.gameObject.transform.SetParent(PlayerController.currentPlateform[PlayerController.currentPlateform.Count - 1].transform);
            }
            else
            {
                other.gameObject.transform.SetParent(null);
            }
        }
    }
}
