using UnityEngine;

public class AnimationRelayEnemy : MonoBehaviour
{
    private Enemy enemy;
    
    void Start()
    {
        // il est forcément nul hein?
        if (enemy is null) enemy = GetComponent<Enemy>();
    }

    public void EndStun()
    {
        enemy.EndStun();
    }
}
