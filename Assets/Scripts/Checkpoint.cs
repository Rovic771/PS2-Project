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
    private Animator _animator;

    [Header("Audio")] 
    [SerializeField] private float volumeCheckpoint = 1f;

    private void Start()
    {
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(alreadyTake) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AudioManager.Instance.PlaySoundEnvironment(AudioManager.EnvironmentSound.Checkpoint, volumeCheckpoint);
            PlayerPrefs.SetFloat("checkpointX", transform.position.x);
            PlayerPrefs.SetFloat("checkpointY", transform.position.y);
            alreadyTake = true;
            _animator.SetTrigger("takeCheckPoint");
        }
        
    }
}
