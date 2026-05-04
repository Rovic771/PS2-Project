using UnityEngine;

public class SprinterEnemy : Enemy
{
    [SerializeField] private float attackSpeed;
    
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun)
        {
            playerDetected = true;
        }
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isStun)
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
        //Debug.Log("PursuitPlayer");
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, attackSpeed * Time.deltaTime);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Debug.Log("player detected " +  playerDetected);
        switch (playerDetected)
        {
            case true:
                patternPointA.SetActive(false);
                patternPointB.SetActive(false);
                PursuitPlayer();
                break;
            
            case false:
                patternPointA.SetActive(true);
                patternPointB.SetActive(true);
                break;
        }
    }
}
