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

    protected virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        bulletManager = FindFirstObjectByType<BulletManager>();

        _currentBullets = bulletCount;
        if (loadingImage != null)
            loadingImage.SetActive(false);
    }

    protected virtual void Update()
    {
        // If reloading, count reload time
        if (_isReloading)
        {
            _timeSinceReloadStart += Time.deltaTime;
            if (_timeSinceReloadStart >= reloadTime)
            {
                FinishReload();
            }
        }

        // Track time since last shot
        _timeSinceLastShot += Time.deltaTime;
    }

    // Attempt to shoot once.  Children call this when the AI or Player has decided to shoot.
    protected void AttemptShoot(Vector2 spawnPos, Quaternion spawnRotation)
    {
        if (_isReloading)
        {
            if (emptyGunSound != null)
                _audioSource.PlayOneShot(emptyGunSound);
            return;
        }

        // Check rate of fire
        if (_timeSinceLastShot < 1f / fireRate) return;

        // Fire bullet
        if (_currentBullets > 0)
        {
            ShootImplementation(spawnPos, spawnRotation);
            _currentBullets--;
            _timeSinceLastShot = 0f;

            // If out of bullets, reload
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

    // Spawns the bullet via the bullet manager.
    protected virtual void ShootImplementation(Vector2 spawnPos, Quaternion spawnRotation)
    {
        if (bulletManager != null)
        {
            bulletManager.Shoot(spawnPos, spawnRotation);
        }

        if (shootSound != null)
            _audioSource.PlayOneShot(shootSound);
    }

    private IEnumerator StopShootingTemporarily(float duration)
    {
        canShoot = false;
        yield return new WaitForSeconds(duration);
        canShoot = true;
    }

    protected virtual void OnEnable()
    {
        // Subscribe to any player’s death
        PlayerHealth.OnAnyPlayerDied += HandleAnyPlayerDied;
    }

    protected virtual void OnDisable()
    {
        // Unsubscribe when disabled/destroyed
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

    // Finishes reload process
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
}
