using System.Collections;
using UnityEngine;

public class SprinterEnemy : Enemy
{
    [SerializeField] private float attackSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float knockbackOnPlayerForceX = 10f;
    [SerializeField] private float knockbackOnPlayerForceY = 10f;
    private Vector2 posInit;
    
    public bool pursuitSoundPlayed = false;
    
    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField] private float volumeAttack = 1f;
    
    
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall() && _playerController.isTargetable)
        {
            playerDetected = true;
            if (!pursuitSoundPlayed)
            {
                _audioSource.clip = AudioManager.Instance.EnemyClips[2];
                _audioSource.loop = true;
                _audioSource.volume = volumeAttack;
                _audioSource.Play();
                pursuitSoundPlayed = true;
            }
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isStun || TestIfWall() || !_playerController.isTargetable)
            {
                playerDetected = false;
                pursuitSoundPlayed = false;
                _audioSource.clip = null;
            }
        }
    }

    public override void Init()
    {
        animator = GetComponent<Animator>();
        posInit = transform.position;
        _audioSource = GetComponentInParent<AudioSource>();
        classEnemy = ClassEnemy.flute;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall() && _playerController.isTargetable)
        {
            playerDetected = true;
            if (!pursuitSoundPlayed)
            {
                _audioSource.clip = AudioManager.Instance.EnemyClips[2];
                _audioSource.loop = true;
                _audioSource.volume = volumeAttack;
                _audioSource.Play();
                pursuitSoundPlayed = true;
            }
            //playerWasDetected = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerDetected = false;
            _audioSource.clip = null;
        }
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun)
        {
            bool touchFromRight;
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (transform.position.x < other.gameObject.transform.position.x)
            {
                touchFromRight = false;
            }
            else
            {
                touchFromRight = true;
            }
            playerController.TakeDamage(damage);
            playerController.KnockBack(touchFromRight, knockbackOnPlayerForceX, knockbackOnPlayerForceY);
        }
        
    }

    private void PursuitPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, attackSpeed * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.1f, 0.2f), 0f, groundLayer);
    }

    private void ReturnToInitPosition()
    {
        if (Vector2.Distance(transform.position, posInit) < 1f)
        {
            isWalking = false;
            isIddle = true;

        }
        else
        {
            isWalking = true;
            transform.position = Vector3.MoveTowards(transform.position, posInit, speed * Time.deltaTime);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsGrounded() && !isStun)
        {
            isIddle = false;
            switch (playerDetected)
            {
                case true:
                    isWalking = false;
                    PursuitPlayer();
                    break;
    
                case false: ;
                    if(basicMove)
                    {
                        isWalking = true;
                        break;
                    }
                    ReturnToInitPosition();
                    break;
            }
        }
        else if(!IsGrounded())
        {
            isWalking = false;
            isIddle = true;
        }
        //Debug.Log(IsGrounded());
        animator.SetBool("isIddle", isIddle);
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("playerDetected", playerDetected);
    }
}
