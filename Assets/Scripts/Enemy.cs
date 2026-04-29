using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject detectionZone;
    [SerializeField] private GameObject patternPointA;
    [SerializeField] private GameObject patternPointB;
    [SerializeField] public EnemyType typeEnemy;
    [SerializeField] private float stunTime = 5;
    [SerializeField] private GameObject stunIndicator;
    private Vector2 targetPos;
    public bool playerDetected = false;
    public GameObject player; 
    public float life;
    public float damage;
    public float speed;
    public Rigidbody2D rbEnemy;
    public bool isStun = false;

    private IEnumerator StunTime()
    {
        yield return new WaitForSeconds(stunTime);
        Stun();
    }
    
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
    
    public void Stun()
    {
        switch (isStun)
        {
            case false:
                stunIndicator.SetActive(true);
                isStun = true;
                StartCoroutine(StunTime());
                break;
            
            case true:
                stunIndicator.SetActive(false);
                isStun = false;
                life = _enemyData.life;
                break;
        }
    }

    public virtual void FixedUpdate()
    {
        if (!playerDetected && !isStun)
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

        if (life <= 0 && !isStun)
        {
            Stun();
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Truc touché");
        if (other.gameObject.layer == LayerMask.NameToLayer("DoProjectileAlly") || other.gameObject.layer == LayerMask.NameToLayer("ReProjectileAlly"))
        {
            life--;
            Debug.Log("life " + life);
        }
    }

    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnTriggerExit2D(Collider2D other);
}
