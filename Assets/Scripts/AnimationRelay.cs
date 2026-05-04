using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    public PlayerController playerController;
    
    void Start()
    {
        if(playerController is null) playerController = GetComponentInParent<PlayerController>();
    }

    public void TriggerJump(string typeJump)
    {
        playerController.ApplyForce(typeJump);
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
