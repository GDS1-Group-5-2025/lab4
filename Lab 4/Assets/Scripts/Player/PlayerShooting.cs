using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerShooting : MonoBehaviour
{

    public int bulletCount = 10;
    public float fireRate = 0.5f;
    public float reloadTime = 2f;

    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptyGunSound;

    public GameObject loadingImage;
    public Image radialProgressBar;

    private AudioSource _audioSource;
    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    private BulletManager _bulletManager;

    private int _bullets;
    private float _timeSinceLastShot;
    private bool _isReloading;
    private float _timeSinceReloadStart;

    private void Start()
    {
        // Get audio source component
        _audioSource = GetComponent<AudioSource>();

        // Get Bullet Manager
        _bulletManager = FindFirstObjectByType<BulletManager>();
        // Get player input component
        _playerInput = GetComponentInParent<PlayerInput>();

        // Get current player input action map
        _playerActionMap = _playerInput.currentActionMap;

        // Set initial bullet count
        _bullets = bulletCount;
        // progress bar set to 0
        radialProgressBar.fillAmount = 0f;
    }


    private void Update()
    {
        // If player is reloading
        if (_isReloading)
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
            }
        }

        if (_timeSinceLastShot <= 1f / fireRate)
        {
            _timeSinceLastShot += Time.deltaTime;
        }
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap != _playerActionMap || !ctx.performed) return;
        if (_timeSinceLastShot < 1f / fireRate) return;

        if (_isReloading)
        {
            // Play empty gun sound
            _audioSource.PlayOneShot(emptyGunSound);
            return;
        }

        _bulletManager.Shoot(transform.position + transform.up * 1f, transform.rotation);
        _audioSource.PlayOneShot(shootSound);
        _bullets--;
        _timeSinceLastShot = 0f;
        if (_bullets == 0)
        {
            loadingImage.SetActive(true);
            _isReloading = true;
            _timeSinceReloadStart = 0f;
        }
    }
}
