using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject detectionZone;
    [SerializeField] public GameObject patternPointA;
    [SerializeField] public GameObject patternPointB;
    [SerializeField] public EnemyType typeEnemy;
    [SerializeField] private float stunTime = 5;
    [SerializeField] private GameObject stunIndicator;
    public Vector2 targetPos;
    public bool playerDetected = false;
    public GameObject player; 
    public float life;
    public int damage;
    public float speed;
    public Rigidbody2D rbEnemy;
    public bool isStun = false;
    public float _flipValue = 0;
    public bool isFacingRight = true;
    public Animator animator;
    
    [Header("Raycast")]
    [SerializeField] Color rayColor = Color.green;
    [SerializeField] private Transform rayCastOrigin;
    [SerializeField] private LayerMask whatToHit;

    
    private IEnumerator StunTime()
    {
        yield return new WaitForSeconds(stunTime);
        EndStun();
    }
    
    
    public enum EnemyType { Do, Re }
    private void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        targetPos = patternPointA.transform.position;
        life = _enemyData.life;
        damage = _enemyData.damage;
        speed = _enemyData.speed;
        Init();
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
    
    public virtual void Stun()
    {
        if (isStun) return; 
        animator.SetBool("isStun", true);
        isStun = true;
        stunIndicator.SetActive(true);
        StopAllCoroutines(); 
        StartCoroutine(StunTime());
    }

    public void EndStun()
    {
        isStun = false;
        stunIndicator.SetActive(false);
        life = _enemyData.life;
        animator.SetBool("isStun", false);
    }

    private void DetectWhenFlip()
    {
        if (!playerDetected)
        {
            if (targetPos.x > transform.position.x && isFacingRight)
            {
                Flip();
            }
            else if (targetPos.x < transform.position.x && !isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            if (player.transform.position.x > transform.position.x && isFacingRight)
            {
                Flip();
            }
            else if (player.transform.position.x < transform.position.x && !isFacingRight)
            {
                Flip();
            }
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
        DetectWhenFlip();
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        _flipValue += 180;
        transform.rotation = Quaternion.Euler(0, _flipValue, 0);
    }
    
    protected bool TestIfWall()
    {
        if (player == null || rayCastOrigin == null) return true;
        
        Vector2 origin = rayCastOrigin.position;
        Vector2 target = player.transform.position + new Vector3(0.3f,0,0);
        Vector2 direction = (target - origin) + new Vector2(0,1);
        float distance = direction.magnitude; 
        RaycastHit2D hit = Physics2D.Raycast(origin, direction.normalized, distance, whatToHit);
        Debug.DrawRay(origin, direction.normalized * distance, rayColor, 0.1f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return false;
            }
        }

        return true;
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DoProjectileAlly") || other.gameObject.layer == LayerMask.NameToLayer("ReProjectileAlly"))
        {
            life--;
            if (life <= 0)
            {
                Stun();
            }
        }
    }

    public abstract void Init();
    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnTriggerExit2D(Collider2D other);
}
