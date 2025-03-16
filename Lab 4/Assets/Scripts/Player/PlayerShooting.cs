using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShooting : BaseShooting
{

    public Image radialProgressBar;

    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    private Animator _animator;

    private int _bullets;
    private bool _shootingDisabled = false;

    protected override void Start()
    {
        base.Start(); // Call the base Start to set up everything

        _playerInput = GetComponentInParent<PlayerInput>();
        _playerActionMap = _playerInput.currentActionMap;

        _animator = GetComponentInParent<Animator>();
        // Set initial bullet count
        _bullets = bulletCount;
        // progress bar set to 0
        radialProgressBar.fillAmount = 0f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_playerActionMap != null)
        {
            // Update progress bar
            _timeSinceReloadStart += Time.deltaTime;
            float progress = _timeSinceReloadStart / reloadTime;
            radialProgressBar.fillAmount = progress;
            // If reload time has passed
            if (_timeSinceReloadStart >= reloadTime)
            {
                // Reset bullets, progress bar and reload flag
                _bullets = bulletCount;
                _isReloading = false;
                _audioSource.PlayOneShot(reloadSound);
                loadingImage.SetActive(false);
                radialProgressBar.fillAmount = 0f;
                InputAction shootAction = _playerActionMap.FindAction("Shoot");
                if (shootAction != null)
                {
                    shootAction.performed += Shoot;
                }
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
