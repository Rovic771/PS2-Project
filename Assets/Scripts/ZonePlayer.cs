using System;
using UnityEngine;

public class ZonePlayer : MonoBehaviour
{
    [SerializeField] private PlayerController zoneActiveSys;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (zoneActiveSys.zoneActive)
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.layer == LayerMask.NameToLayer("ReObject"))
            {
                Destroy(other.gameObject);
            }
        }
        else 
        {
            Debug.Log(other.gameObject.name);
            if (other.gameObject.layer == LayerMask.NameToLayer("DoObject"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}
