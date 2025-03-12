using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{

    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    private BulletManager _bulletManager;

    private void Start()
    {

        _bulletManager = FindFirstObjectByType<BulletManager>();
        _playerInput = GetComponentInParent<PlayerInput>();

        // Get current player input action map
        _playerActionMap = _playerInput.currentActionMap;
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap == _playerActionMap && ctx.performed)
        {
            _bulletManager.Shoot(transform.position + transform.up * 1f, transform.rotation);
        }
    }
}
