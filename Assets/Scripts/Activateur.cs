using System;
using UnityEngine;

public class Activateur : MonoBehaviour
{
    [SerializeField] private GameObject objectItAffect;
    private PlateformeMouvante plateformAffect;

    private void Start()
    {
        plateformAffect = objectItAffect.GetComponent<PlateformeMouvante>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Debug.Log(gameObject.name + " activé");
            plateformAffect.objectActive = true;
        }
    }
}
