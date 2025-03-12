using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;

    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;

    void Start()
    {

        _playerInput = GetComponentInParent<PlayerInput>();

        // Get current player input action map
        _playerActionMap = _playerInput.currentActionMap;
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap == _playerActionMap && ctx.performed)
        {
            Instantiate(bulletPrefab, transform.position + transform.up * 1f, transform.rotation);
        }
    }
}
