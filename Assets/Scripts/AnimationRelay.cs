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
        Debug.Log("dsdfgrd");
        playerController.ApplyForce(typeJump);
    }

    public void TriggerShoot()
    {
        playerController.ApplyShoot();
    }
}
