using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject detectionZone;
    [SerializeField] private GameObject patternPointA;
    [SerializeField] private GameObject patternPointB;
    [SerializeField] public EnemyType typeEnemy;
    private Vector2 targetPos;
    public bool playerDetected = false;
    public GameObject player; 
    public float life;
    public float damage;
    public float speed;
    public Rigidbody2D rbEnemy;
    
    public enum EnemyType { Do, Re }
    private void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        targetPos = patternPointB.transform.position;
        life = _enemyData.life;
        damage = _enemyData.speed;
        speed = _enemyData.speed;
        Debug.Log(targetPos);
    }

    private void GoToPoint()
    {
        float distanceEnemyPointA = Vector2.Distance(transform.position, patternPointA.transform.position);
        float distanceEnemyPointB = Vector2.Distance(transform.position, patternPointB.transform.position);
        if (distanceEnemyPointA < distanceEnemyPointB)
        {
            targetPos = patternPointB.transform.position;
        }
        else
        {
            targetPos = patternPointA.transform.position;
        }
    }
    
    public void Die()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void FixedUpdate()
    {
        if (!playerDetected)
        {
            if (Vector2.Distance(transform.position, targetPos) < 1f)
            {
                GoToPoint();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }
        }
    }

    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnTriggerExit2D(Collider2D other);
}
