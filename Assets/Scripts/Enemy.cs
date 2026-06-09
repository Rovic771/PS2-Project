using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject detectionZone;
    [SerializeField] public GameObject patternPointA;
    [SerializeField] public GameObject patternPointB;
    [SerializeField] public EnemyType typeEnemy;
    [SerializeField] private float stunTime = 5;
    [SerializeField] public bool basicMove = true;
    public Vector2 targetPos;
    public bool playerDetected = false;
    public GameObject player; 
    protected PlayerController _playerController;
    public float life;
    public int damage;
    public float speed;
    public Rigidbody2D rbEnemy;
    public bool isStun = false;
    public bool isIddle = false;
    public float _flipValue = 0;
    public bool isFacingRight = true;
    public bool isWalking;
    public Animator animator;
    public  ClassEnemy classEnemy;
    public enum ClassEnemy { violon, flute }
    
    [Header("Raycast")]
    [SerializeField] Color rayColor = Color.green;
    [SerializeField] private Transform rayCastOrigin;
    [SerializeField] private LayerMask whatToHit;
    
    [Header("Audio")]
    [SerializeField] private float volumeHit = 1f;
    public AudioSource _audioSource;

    
    private IEnumerator StunTime()
    {
        yield return new WaitForSeconds(stunTime);
        animator.SetTrigger("Revive");
    }
    
    
    public enum EnemyType { Do, Re }
    private void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _audioSource = GetComponentInParent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        _playerController = player.GetComponent<PlayerController>();
        if (typeEnemy == EnemyType.Do) gameObject.tag = "DoEnemy";
        else if (typeEnemy == EnemyType.Re) gameObject.tag = "ReEnemy";
        
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
        gameObject.layer = LayerMask.NameToLayer("EnemyStun");
        animator.SetBool("isStun", true);
        isStun = true;
        StopAllCoroutines(); 
        StartCoroutine(StunTime());
    }

    public void EndStun()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        isStun = false;
        ResetEnemyState();
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

    public virtual void ResetEnemyState()
    {
        
    }
    
    public virtual void FixedUpdate()
    {
        if (!playerDetected && !isStun)
        {
            if (Vector2.Distance(transform.position, targetPos) < 1f)
            {
                GoToPoint();
            }
            else if(basicMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }
            else if(!basicMove)
            {
                isWalking = false;
                isIddle = true;
            }
        }
        DetectWhenFlip();
    }
    
    private void Flip()
    {
        if (!isStun)
        {
            isFacingRight = !isFacingRight;
            _flipValue += 180;
            transform.rotation = Quaternion.Euler(0, _flipValue, 0);
        }
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
        if (other.gameObject.layer == LayerMask.NameToLayer("DoProjectileAlly") && gameObject.CompareTag("DoEnemy") 
            || other.gameObject.layer == LayerMask.NameToLayer("ReProjectileAlly") && gameObject.CompareTag("ReEnemy"))
        {
            switch (classEnemy)
            {
                case ClassEnemy.violon:
                    AudioManager.Instance.PlaySoundEnemy(AudioManager.EnemySound.HitViolon, volumeHit);
                    break;
                case ClassEnemy.flute:
                    AudioManager.Instance.PlaySoundEnemy(AudioManager.EnemySound.HitFlute, volumeHit);
                    break;
            }
            LoseHp();
        }
    }

    public void LoseHp()
    {
        life--;
        if (life <= 0)
        {
            Stun();
        }
    }

    public abstract void Init();
    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnTriggerExit2D(Collider2D other);
}
