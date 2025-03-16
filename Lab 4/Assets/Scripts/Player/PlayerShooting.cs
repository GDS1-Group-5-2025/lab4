using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShooting : BaseShooting
{
    public Image radialProgressBar;

    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    private Animator _animator;

    // We don’t need a separate _bullets here because BaseShooting already uses _currentBullets
    private bool _shootingDisabled = false;

    protected override void Start()
    {
        base.Start();

        _playerInput = GetComponentInParent<PlayerInput>();
        _playerActionMap = _playerInput.currentActionMap;

        _animator = GetComponentInParent<Animator>();

        // Initialize progress bar
        if (radialProgressBar != null)
        {
            radialProgressBar.fillAmount = 0f;
        }
    }

    protected override void Update()
    {
        // Always call base.Update() so reloading and fire-rate logic work properly
        base.Update();

        // Update radial progress bar if reloading
        if (_isReloading && radialProgressBar != null)
        {
            float progress = _timeSinceReloadStart / reloadTime;
            radialProgressBar.fillAmount = progress;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // Subscribe shoot action
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
        // Unsubscribe shoot action
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
        if (!canShoot || !ctx.performed) return;

        if (_shootingDisabled)
        {
            Debug.Log("Shooting is temporarily disabled after respawning.");
            return;
        }

        if (_isReloading)
        {
            // Optionally play empty gun sound
            if (emptyGunSound != null)
                _audioSource.PlayOneShot(emptyGunSound);
            return;
        }

        // Animate shoot
        if (_animator != null && !_isReloading)
        {
            _animator.SetTrigger("Shoot");
        }

        // Attempt to fire using base logic
        if (ctx.action.actionMap == _playerActionMap)
        {
            Vector2 spawnPos = transform.position + transform.up * 1f;
            Quaternion rotation = transform.rotation;
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
