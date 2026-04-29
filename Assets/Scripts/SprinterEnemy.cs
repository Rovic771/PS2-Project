using UnityEngine;

public class SprinterEnemy : Enemy
{
    [SerializeField] private float attackSpeed;
    
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall())
        {
            playerDetected = true;
        }
        else playerDetected = false;
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun && !TestIfWall())
        {
            playerDetected = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerDetected = false;
        }
    }

    private void PursuitPlayer()
    {
        Debug.Log("PursuitPlayer");
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, attackSpeed * Time.deltaTime);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        switch (playerDetected)
        {
            case true:
                PursuitPlayer();
                break;
        }
    }
}
