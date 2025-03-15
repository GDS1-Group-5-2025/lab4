using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_playerActionMap != null)
        {
            InputAction shootAction = _playerActionMap.FindAction("Shoot");
            if (shootAction != null)
            {
                shootAction.performed += Shoot;
            }
        }
    }

    protected override void OnDisable()
    {
        if (_playerActionMap != null)
        {
            InputAction shootAction = _playerActionMap.FindAction("Shoot");
            if (shootAction != null)
            {
                shootAction.performed -= Shoot;
            }
        }

        base.OnDisable();
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        Debug.Log(canShoot);
        if (!canShoot) return;
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