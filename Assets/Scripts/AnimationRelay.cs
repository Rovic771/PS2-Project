using UnityEngine;

public class AnimationRelay : MonoBehaviour
{
    public PlayerController playerController;
    
    void Start()
    {
        if(playerController is null) playerController = GetComponentInParent<PlayerController>();
        Debug.Log(playerController);
    }

    public void TriggerJump(string typeJump)
    {
        Debug.Log("dsdfgrd");
        playerController.ApplyForce(typeJump);
    }
}
