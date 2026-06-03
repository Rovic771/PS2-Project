using UnityEngine;

public class AnimationRelayPlayer : MonoBehaviour
{
    public PlayerController playerController;
    public Enemy enemy;
    
    void Start()
    {
        if(playerController is null) playerController = GetComponentInParent<PlayerController>();
    }

    public void TriggerShoot()
    {
        playerController.ApplyShoot();
    }

    public void TriggerColliderZone(string typeZone)
    {
        playerController.ActiveColliderZone(typeZone);
    }
}
