using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShooting : BaseShooting
{
    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    private Animator _animator;

    private int _bullets;
    private float _timeSinceLastShot;
    private bool _isReloading;
    private float _timeSinceReloadStart;
    private bool _shootingDisabled = false;

    protected override void Start()
    {
        base.Start(); // Call the base Start to set up everything

        _playerInput = GetComponentInParent<PlayerInput>();
        _playerActionMap = _playerInput.currentActionMap;

        _animator = GetComponentInParent<Animator>();
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

        if (_shootingDisabled)
        {
            Debug.Log("Shooting is temporarily disabled after respawning.");
            return;
        }

        if (_isReloading)
        {
            // Play empty gun sound
            _audioSource.PlayOneShot(emptyGunSound);
            return;
        }
        Vector2 spawnPos = transform.position + transform.up * 1f;
        Quaternion rotation = transform.rotation;

        if (_animator != null && !_isReloading)
        {
            _animator.SetTrigger("Shoot");
        }

        // Attempt to shoot using the base method
        if (ctx.action.actionMap == _playerActionMap)
        {
            AttemptShoot(spawnPos, rotation);
        }
    }
    public void DisableShooting(float seconds = 0)
    {
        _shootingDisabled = true;
        if(seconds > 0){
            Invoke("EnableShooting", seconds);
        }
        
    }
    private void EnableShooting()
    {
        _shootingDisabled = false;
    }
}
