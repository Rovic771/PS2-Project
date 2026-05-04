using System;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField] private GameObject mcProfilD;
    [SerializeField] private GameObject mcProfilG;
    
    void Start()
    {
        if(playerController is null) playerController = GetComponentInParent<PlayerController>();
        Debug.Log(playerController);
    }

    private void FixedUpdate()
    {
        if (playerController.isFacingRight)
        {
            mcProfilG.SetActive(false);
            mcProfilD.SetActive(true);
        }
        else
        {
            mcProfilD.SetActive(false);
            mcProfilG.SetActive(true);
        }
    }
}
