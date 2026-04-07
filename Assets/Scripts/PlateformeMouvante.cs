using System;
using UnityEngine;

public class PlateformeMouvante : MonoBehaviour
{
    [SerializeField] private GameObject origine;
    [SerializeField] private GameObject end;
    [SerializeField] private float speed = 1;
    [SerializeField] private bool loop;
    [SerializeField] public bool objectActive = true; 
    private Vector3 targetPos;
    public bool noteTouched; // false ça veut dire que c les projectile Re qui vont activer et vice versa

    private void Start()
    {
        if (noteTouched == false)
        {
            transform.position = origine.transform.position;
            targetPos = end.transform.position;
        }
        else
        {
            transform.position = end.transform.position;
            targetPos = origine.transform.position;
        }
    }
    
    private void FixedUpdate()
    {
        if (loop)
        {
            if (Vector3.Distance(transform.position, targetPos) < 0.1f && targetPos == end.transform.position)
            {
                targetPos = origine.transform.position;
            }
            else if (Vector3.Distance(transform.position, targetPos) < 0.1f && targetPos == origine.transform.position)
            {
                targetPos = end.transform.position;
            }
        }
        if (objectActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
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
            if (other.gameObject.CompareTag("ReProjectile") && noteTouched == false)
            {
                targetPos =  end.transform.position;
                noteTouched = true;
            }
            else if (other.gameObject.CompareTag("DoProjectile") && noteTouched == true)
            {
                targetPos = origine.transform.position;
                noteTouched = false;
            }
            objectActive = true;
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
