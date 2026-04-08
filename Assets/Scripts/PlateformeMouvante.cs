using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeMouvante : MonoBehaviour
{
    [SerializeField] private GameObject origine;
    [SerializeField] List<GameObject> points = new List<GameObject>();
    [SerializeField] private float speed = 1;
    [SerializeField] public bool objectActive = true; 
    private Vector3 targetPos;
    //public bool noteTouched; // false ça veut dire que c les projectile Re qui vont activer et vice versa

    private void Start()
    {
        transform.position = origine.transform.position;
    }
    
    private void FixedUpdate()
    {
        if (objectActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    private int CurrentPoint()
    {
        float distanceMin = 10000;
        int nearestPointIndex = 0;
        for (int i = 0; i < points.Count; i++)
        {
            float dist = Vector2.Distance(points[i].transform.position, transform.position);
            if (dist < distanceMin)
            {
                distanceMin = dist;
                nearestPointIndex = i;
            }
        }
        return nearestPointIndex;
    }

    private void ChangeTargetPoint(string typeProjectile)
    {
        int currentPoint = CurrentPoint();
        if (typeProjectile == "Re")
        {
            if (currentPoint + 1 < points.Count)
            {
                targetPos = points[currentPoint + 1].transform.position;
            }
            else
            {
                Debug.Log("ct le dernier point");
                return;
            }
        }
        else if (typeProjectile == "Do")
        {
            if (currentPoint - 1 < points.Count)
            {
                targetPos = points[currentPoint - 1].transform.position;
            }
            else
            {
                Debug.Log("ct le premier point");
                return;
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.transform.SetParent(transform);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            if (other.gameObject.CompareTag("ReProjectile"))
            {
                ChangeTargetPoint("Re");
            }
            else if(other.gameObject.CompareTag("DoProjectile"))
            {
                ChangeTargetPoint("Do");
            }

            objectActive = true;
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
            other.gameObject.transform.SetParent(null);
        }
    }
}
