using System;
using System.Numerics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Checkpoint : MonoBehaviour
{
    public bool alreadyTake = false;
    private Vector3 playerPos;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(alreadyTake) return;
        AudioManager.Instance.PlaySound(0, 9, AudioManager.Sound.checkpoint, gameObject);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerPrefs.SetFloat("checkpointX", transform.position.x);
            PlayerPrefs.SetFloat("checkpointY", transform.position.y);
        }
        alreadyTake = true;
    }
}
