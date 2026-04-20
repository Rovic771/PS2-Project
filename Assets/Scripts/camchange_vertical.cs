using UnityEngine;
using Unity.Cinemachine;

public class camchange_vertical : MonoBehaviour
{
    public CinemachinePositionComposer composer;
    public GameObject player;
    public float x;
    public float y;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            composer.Composition.ScreenPosition.x = x;
            composer.Composition.ScreenPosition.y = y;
        }
    }
}
