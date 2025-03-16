using TMPro;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public int bulletCount = 6;
    public float fireRate = 1f;
    public float reloadTime = 2f;

    [Header("References")]
    public BulletManager bulletManager;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip emptyGunSound;
    public GameObject loadingImage;

    protected AudioSource _audioSource;
    protected int _currentBullets;
    protected float _timeSinceLastShot;
    protected float _timeSinceReloadStart;
    protected bool _isReloading;

    public bool canShoot = true;

    protected Animator _animator;

    protected virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        bulletManager = FindFirstObjectByType<BulletManager>();

        _animator = GetComponentInParent<Animator>();

        // Initialize bullets and hide loading image
        _currentBullets = bulletCount;
        if (loadingImage != null)
            loadingImage.SetActive(false);
    }

    protected virtual void Update()
    {
        // Handle reload timing
        if (_isReloading)
        {
            _timeSinceReloadStart += Time.deltaTime;

            // If reload timer has finished, finalize reload
            if (_timeSinceReloadStart >= reloadTime)
            {
                FinishReload();
            }
        }

        // Track time since last shot for fire-rate limiting
        _timeSinceLastShot += Time.deltaTime;
    }

    protected void AttemptShoot(Vector2 spawnPos, Quaternion spawnRotation)
    {
        // Don’t shoot if already reloading
        if (_isReloading)
        {
            if (emptyGunSound != null)
                _audioSource.PlayOneShot(emptyGunSound);
            return;
        }

        // Enforce fire rate
        if (_timeSinceLastShot < 1f / fireRate) return;

        // If we have bullets left, shoot
        if (_currentBullets > 0)
        {
            // Animation trigger
            if (_animator != null && !_isReloading)
            {
                _animator.SetTrigger("Shoot");
            }

            ShootImplementation(spawnPos, spawnRotation);
            _currentBullets--;
            _timeSinceLastShot = 0f;

            // If out of bullets now, start reloading
            if (_currentBullets <= 0)
            {
                StartReload();
            }
        }
        else
        {
            // Out of bullets, so reload
            StartReload();
        }
    }

    protected virtual void ShootImplementation(Vector2 spawnPos, Quaternion spawnRotation)
    {
        if (bulletManager != null)
        {
            bulletManager.Shoot(spawnPos, spawnRotation);
        }

        if (shootSound != null)
        {
            // Stop any currently playing sound before playing the new one
            _audioSource.Stop();
            _audioSource.PlayOneShot(shootSound);
        }
    }

    protected virtual void OnEnable()
    {
        // Example: subscribe to external events if needed
        PlayerHealth.OnAnyPlayerDied += HandleAnyPlayerDied;
    }

    protected virtual void OnDisable()
    {
        // Unsubscribe from events
        PlayerHealth.OnAnyPlayerDied -= HandleAnyPlayerDied;
    }

    private void HandleAnyPlayerDied()
    {
        // Stop shooting for 2 seconds
        StartCoroutine(StopShootingTemporarily(2f));
    }

    // Begins reload process
    protected virtual void StartReload()
    {
        if (_isReloading) return;

        _isReloading = true;
        _timeSinceReloadStart = 0f;

        if (loadingImage != null)
            loadingImage.SetActive(true);
    }

    // Called when reload time has elapsed
    protected virtual void FinishReload()
    {
        _isReloading = false;
        _currentBullets = bulletCount;
        _timeSinceReloadStart = 0f;

        if (reloadSound != null)
            _audioSource.PlayOneShot(reloadSound);

        if (loadingImage != null)
            loadingImage.SetActive(false);
    }

    private IEnumerator StopShootingTemporarily(float duration)
    {
        canShoot = false;
        yield return new WaitForSeconds(duration);
        canShoot = true;
    }
}
