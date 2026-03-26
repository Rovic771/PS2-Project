using UnityEngine;

public class NoteProjectile : MonoBehaviour
{
    [SerializeField] float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Micro micro = collision.GetComponent<Micro>();

        if (micro != null)
        {
            micro.Activate();
        }

        Destroy(gameObject);
    }  
}