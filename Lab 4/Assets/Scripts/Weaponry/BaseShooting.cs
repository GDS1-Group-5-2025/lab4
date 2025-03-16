using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseShooting : MonoBehaviour
{
    private static readonly int Shoot = Animator.StringToHash("Shoot");
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

    protected AudioSource audioSource;
    protected int currentBullets;
    protected float timeSinceLastShot;
    protected float timeSinceReloadStart;
    protected bool isReloading;

    public bool canShoot = true;

    protected Animator animator;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bulletManager = FindFirstObjectByType<BulletManager>();

        animator = GetComponentInParent<Animator>();

        // Initialize bullets and hide loading image
        currentBullets = bulletCount;
        if (loadingImage != null)
            loadingImage.SetActive(false);
    }

    protected virtual void Update()
    {
        // Handle reload timing
        if (isReloading)
        {
            timeSinceReloadStart += Time.deltaTime;

            // If reload timer has finished, finalize reload
            if (timeSinceReloadStart >= reloadTime)
            {
                FinishReload();
            }
        }

        // Track time since last shot for fire-rate limiting
        timeSinceLastShot += Time.deltaTime;
    }

    protected void AttemptShoot(Vector2 spawnPos, Quaternion spawnRotation)
    {
        // Don’t shoot if already reloading
        if (isReloading)
        {
            if (emptyGunSound )
                audioSource.PlayOneShot(emptyGunSound);
            return;
        }

        // Enforce fire rate
        if (timeSinceLastShot < 1f / fireRate) return;

        // If we have bullets left, shoot
        if (currentBullets > 0)
        {
            // Animation trigger
            if (animator  && !isReloading)
            {
                animator.SetTrigger(Shoot);
            }

            ShootImplementation(spawnPos, spawnRotation);
            currentBullets--;
            timeSinceLastShot = 0f;

            // If out of bullets now, start reloading
            if (currentBullets <= 0)
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
        if (bulletManager)
        {
            bulletManager.Shoot(spawnPos, spawnRotation);
        }

        if (shootSound)
        {
            // Stop any currently playing sound before playing the new one
            audioSource.Stop();
            audioSource.PlayOneShot(shootSound);
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
        if (isReloading) return;

        isReloading = true;
        timeSinceReloadStart = 0f;

        if (loadingImage)
            loadingImage.SetActive(true);
    }

    // Called when reload time has elapsed
    protected virtual void FinishReload()
    {
        isReloading = false;
        currentBullets = bulletCount;
        timeSinceReloadStart = 0f;

        if (reloadSound)
            audioSource.PlayOneShot(reloadSound);

        if (loadingImage)
            loadingImage.SetActive(false);
    }

    private IEnumerator StopShootingTemporarily(float duration)
    {
        canShoot = false;
        yield return new WaitForSeconds(duration);
        canShoot = true;
    }
}
