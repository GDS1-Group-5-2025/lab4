using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShooting : BaseShooting
{
    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    private bool canShoot = true;

    protected override void Start()
    {
        base.Start(); // Call the base Start to set up everything

        _playerInput = GetComponentInParent<PlayerInput>();
        _playerActionMap = _playerInput.currentActionMap;
    }

    private void OnEnable()
    {
        PlayerHealth.OnAnyPlayerDied += HandleAnyPlayerDied;
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
        PlayerHealth.OnAnyPlayerDied -= HandleAnyPlayerDied;
        if (_playerActionMap != null)
        {
            InputAction shootAction = _playerActionMap.FindAction("Shoot");
            if (shootAction != null)
            {
                shootAction.performed -= Shoot;
            }
        }
    }

    private void HandleAnyPlayerDied()
    {
        // Stop shooting for 2 seconds
        StartCoroutine(StopShootingTemporarily(2f));
    }

    private IEnumerator StopShootingTemporarily(float duration)
    {
        canShoot = false;
        yield return new WaitForSeconds(duration);
        canShoot = true;
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (!canShoot) return;
        if (!ctx.performed) return;

        Vector2 spawnPos = transform.position + transform.up * 1f;
        Quaternion rotation = transform.rotation;

        // Attempt to shoot using the base method
        AttemptShoot(spawnPos, rotation);
    }
}