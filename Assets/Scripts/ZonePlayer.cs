using System;
using UnityEngine;

public class ZonePlayer : MonoBehaviour
{
    [SerializeField] private PlayerController zoneActiveSys;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (zoneActiveSys.zoneActive)
        {
            if (gameObject.CompareTag("DoZone") && other.gameObject.layer == LayerMask.NameToLayer("DoObject"))
            {
                Destroy(other.gameObject);
            }
            else if (gameObject.CompareTag("DoZone") && other.gameObject.layer == LayerMask.NameToLayer("DoProjectileEnemy"))
            {
                Destroy(other.gameObject);
            }
        }
        else 
        {
            if (gameObject.CompareTag("ReZone") && other.gameObject.layer == LayerMask.NameToLayer("ReObject"))
            {
                Destroy(other.gameObject);
            }
            else if (gameObject.CompareTag("ReZone") && other.gameObject.layer == LayerMask.NameToLayer("ReProjectileEnemy"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
