using System;
using UnityEngine;

// la petite majuscule
public class scrolling : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 endScrolling;
    private Vector2 posInit;
    
    void Start()
    {
        posInit = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, endScrolling) < 1f)
        {
            transform.position = posInit;
        }
        transform.position = Vector2.MoveTowards(transform.position, endScrolling, speed * Time.deltaTime);
    }
}
