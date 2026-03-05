using UnityEngine;

public class Micro : MonoBehaviour
{
    public MovingPlatform platform;

    public void Activate()
    {
        platform.MoveTowardPlayer();
    }
}