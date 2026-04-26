using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference lookAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
    }

    public Vector2 ReadMovementInput()
    {
        return moveAction.action.ReadValue<Vector2>();
    }

    public Vector2 ReadLookInput()
    {
        return lookAction.action.ReadValue<Vector2>() * Time.deltaTime;
    }
}