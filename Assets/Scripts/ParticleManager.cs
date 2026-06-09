using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public List<ParticleSystem> allParticles = new List<ParticleSystem>();
    private GameObject player;
    [SerializeField] private float distance = 100;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
        allParticles.AddRange(particleSystems);
    }
    
    void Update()
    {
        foreach (var particle in allParticles)
        {
            if (Vector2.Distance(player.transform.position, particle.gameObject.transform.position) > distance)
            {
                particle.gameObject.SetActive(false);
            }
            else
            {
                particle.gameObject.SetActive(true);
            }
        }
    }
}
