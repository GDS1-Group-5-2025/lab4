using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : BaseShooting
{
    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;

    protected override void Start()
    {
        base.Start(); // Call the base Start to set up everything

        _playerInput = GetComponentInParent<PlayerInput>();
        _playerActionMap = _playerInput.currentActionMap;
    }

    private void OnEnable()
    {
        if (_playerActionMap != null)
        {
            InputAction shootAction = _playerActionMap.FindAction("Shoot");
            if (shootAction != null)
            {
                shootAction.performed += Shoot;
            }
        }
    }

    private void OnDisable()
    {
        if (_playerActionMap != null)
        {
            InputAction shootAction = _playerActionMap.FindAction("Shoot");
            if (shootAction != null)
            {
                shootAction.performed -= Shoot;
            }
        }
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Vector2 spawnPos = transform.position + transform.up * 1f;
        Quaternion rotation = transform.rotation;

        // Attempt to shoot using the base method
        if (ctx.action.actionMap == _playerActionMap)
        {
            AttemptShoot(spawnPos, rotation);
        }
    }
}