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
        // Hook up the input event
        if (_playerActionMap != null)
        {
            _playerActionMap["Shoot"].performed += Shoot; 
        }
    }

    private void OnDisable()
    {
        // Unsubscribe when disabled
        if (_playerActionMap != null)
        {
            _playerActionMap["Shoot"].performed -= Shoot;
        }
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Vector2 spawnPos = transform.position + transform.up * 1f;
        Quaternion rotation = transform.rotation;

        // Attempt to shoot using the base method
        AttemptShoot(spawnPos, rotation);
    }
}