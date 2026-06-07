using UnityEngine;
using UnityEngine.InputSystem;

public class bolckControl : MonoBehaviour
{
    [SerializeField] private PlayerInput  playerInput;


    private void OnEnable()
    {
        playerInput.DeactivateInput();
    }

    private void OnDisable()
    {
        playerInput.ActivateInput();
    }
}
