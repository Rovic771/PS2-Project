using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField] float timeDelay = 15f;
    void Start()
    {
        Destroy(gameObject, timeDelay);
    }

}
