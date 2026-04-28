using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject detectionZone;
    public GameObject player; 
    public float life;
    public float damage;
    public float speed;
    [SerializeField] public EnemyType typeEnemy;
    
    public enum EnemyType { Do, Re }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        life = _enemyData.life;
        damage = _enemyData.speed;
        speed = _enemyData.speed;
    }

    public void Die()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    public abstract void OnTriggerEnter2D(Collider2D other);
    public abstract void OnTriggerExit2D(Collider2D other);
}
