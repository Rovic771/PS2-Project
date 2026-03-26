using System;
using UnityEngine;

public class Deathzone : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Die();
        }
    }
}
