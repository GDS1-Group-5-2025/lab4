using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShooting : BaseShooting
{
    public Image radialProgressBar;

    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    
    private bool _shootingDisabled = false;

    protected override void Start()
    {
        base.Start();

        _playerInput = GetComponentInParent<PlayerInput>();
        _playerActionMap = _playerInput.currentActionMap;

        // Initialize progress bar
        if (radialProgressBar != null)
        {
            radialProgressBar.fillAmount = 0f;
        }
    }

    protected override void Update()
    {
        base.Update();

        // Update radial progress bar if reloading
        if (isReloading && radialProgressBar )
        {
            var progress = timeSinceReloadStart / reloadTime;
            radialProgressBar.fillAmount = progress;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // Subscribe shoot action
        var shootAction = _playerActionMap?.FindAction("Shoot");
        if (shootAction != null)
        {
            shootAction.performed += Shoot;
        }
    }

    protected override void OnDisable()
    {
        // Unsubscribe shoot action
        var shootAction = _playerActionMap?.FindAction("Shoot");
        if (shootAction != null)
        {
            shootAction.performed -= Shoot;
        }

        base.OnDisable();
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (_shootingDisabled)
        {
            Debug.Log("Shooting is temporarily disabled after respawning.");
            return;
        }

        if (isReloading)
        {
            // Optionally play empty gun sound
            if (emptyGunSound != null)
                audioSource.PlayOneShot(emptyGunSound);
            return;
        }

        // Attempt to fire using base logic
        if (ctx.action.actionMap == _playerActionMap)
        {
            Vector2 spawnPos = transform.position + transform.up * 1f;
            var rotation = transform.rotation;
            AttemptShoot(spawnPos, rotation);
        }
    }

    public void DisableShooting(float seconds = 0)
    {
        _shootingDisabled = true;
        if (seconds > 0)
        {
            Invoke(nameof(EnableShooting), seconds);
        }
    }

    private void EnableShooting()
    {
        _shootingDisabled = false;
    }
}
