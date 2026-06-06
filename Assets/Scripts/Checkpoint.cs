using System;
using System.Numerics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Checkpoint : MonoBehaviour
{
    private Vector3 playerPos;
    private PlayerController _playerController;

    private void Start()
    {
        // si t'as beaucoup de checkpoints, ca risque de pas mal allourdir la création de ta scene, et du coup le temps de chargement
        // ( unity au lancement de ta scene appelle tous les starts / awake, du coup si ils sont fat, tu manges du temps de loading / freeze )
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //AudioManager.Instance.SoundExample(9);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerPrefs.SetFloat("checkpointX", transform.position.x);
            PlayerPrefs.SetFloat("checkpointY", transform.position.y);
        }
    }
}
