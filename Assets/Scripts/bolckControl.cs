using UnityEngine;
using UnityEngine.InputSystem;

public class bolckControl : MonoBehaviour
{
    [SerializeField] private PlayerInput  playerInput;


    private void OnEnable()
    {
        print("enabled");
    }
}
