using System;
using UnityEngine;

public class Activateur : MonoBehaviour
{
    [SerializeField] private GameObject objectItAffect;
    private PlateformeMouvante plateformAffect;

    // j'te conseille de les stocker comme ca pour éviter les nametolayer couteux potentiellement appellés régulierement
    private readonly int layer = LayerMask.NameToLayer("Projectile");
    
    private void Start()
    {
        plateformAffect = objectItAffect.GetComponent<PlateformeMouvante>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == layer)
        {
            // oublie aps de retirer les logs quand t'as fini de débuguer, ca coute pas mal en perf
            Debug.Log(gameObject.name + " activé");
            plateformAffect.objectActive = true;
        }
    }
}
