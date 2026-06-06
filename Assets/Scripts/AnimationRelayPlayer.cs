using UnityEngine;

public class AnimationRelayPlayer : MonoBehaviour
{
    public PlayerController playerController;
    public Enemy enemy;
    
    void Start()
    {
        // pareil que l'autre relay, forcément nul nan? ( à la limite lui un peu moins )
        if(playerController is null) playerController = GetComponentInParent<PlayerController>();
    }

    public void TriggerShoot()
    {
        playerController.ApplyShoot();
    }
    
}
