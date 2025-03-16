using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UIElements;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    // Event so that other classes can subscribe to the player dying
    public static event Action OnAnyPlayerDied;

    [SerializeField] private int playerNumber = 1;
    [SerializeField] private int startingLives = 2;
    [SerializeField] private GameObject weapon;
    [SerializeField] private bool targetMode = false;
 
    [SerializeField] private int _currentLives;
    private bool _isInvincible = false;

    private IMovement _movement;

    private Collider2D _collider;
    private BulletManager _bulletManager;
    private Vector3 _startingPosition;
    private Quaternion _startingRotation;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    public float invincibilityDuration = 2f;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _movement = GetComponent<IMovement>();
        _bulletManager = FindFirstObjectByType<BulletManager>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _currentLives = startingLives;
        _startingPosition = transform.position;
        _startingRotation = transform.rotation;

        if (_animator != null)
        {
            _animator.SetLayerWeight(_animator.GetLayerIndex("TwoLives"), 1);
            _animator.SetLayerWeight(_animator.GetLayerIndex("OneLife"), 0);
        }
    }

    private void Update()
    {
        if (_animator.GetBool("IsOneLife")){
            _animator.SetLayerWeight(0, 1);
            _animator.SetLayerWeight(1, 0);
        }
        else
        {
            _animator.SetLayerWeight(1, 1);
            _animator.SetLayerWeight(0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only respond if we collided with a "Bullet"
        if (_isInvincible)
        return;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (!targetMode)
            {
                TakeDamage(1);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        _currentLives -= damage;      

        switch (_currentLives)
        {
            case 1:
                Debug.Log("Player has lost a life");
                // Remove the player's hat here
                if (_animator != null)
                {
                    _animator.SetBool("IsOneLife", true);
                    Debug.Log(_animator.GetBool("IsOneLife"));
                }
                break;
            case <= 0:
                _animator.SetTrigger("Dead");
                _bulletManager.ClearBullets();
                PlayerDeath();
                Debug.Log("Player has died");
                break;
        }

    }

    private void PlayerDeath()
    {
        //Change Score
        ScoreManager.Instance.IncrementScoreForOppositionOf(playerNumber);

        OnAnyPlayerDied?.Invoke();

        // Disable movement
        if (_movement != null)
            _movement.enabled = false;

        // Disable the collider
        _collider.enabled = false;

        // Play death animation

        // Disable shooting and aiming
        _bulletManager.shootingEnabled = false;
        DisableShootingForSeconds(2f);

        //Respawn after 2 seconds
        Invoke(nameof(Respawn), 2f);
    }

    private void DisableShootingForSeconds(float duration)
    {
        var shootingScripts = GetComponentsInChildren<BaseShooting>(true);
        foreach (var shooter in shootingScripts)
        {
            shooter.canShoot = false;
        }

        // Start a coroutine, which remains enabled
        StartCoroutine(EnableShootingAfter(duration));
    }

    private IEnumerator EnableShootingAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        var shootingScripts = GetComponentsInChildren<BaseShooting>(true);
        foreach (var shooter in shootingScripts)
        {
            shooter.canShoot = true;
        }
    }

    private void Respawn()
    {
        Debug.Log("Player has respawned");

        // Reset position
        transform.position = _startingPosition;
        transform.rotation = _startingRotation;

        // Reset lives
        _currentLives = startingLives;
        _animator.SetBool("IsOneLife", false);

        // Re-enable movement
        if (_movement != null)
            _movement.enabled = true;

        // Re-enable shooting
        if (weapon)
            weapon.SetActive(true);
        _bulletManager.shootingEnabled = true;

        // Re-enable the collider
        _collider.enabled = true;

        // Restore hat + Reset animator layers
 
        if (_animator != null)
        {
            _animator.SetLayerWeight(_animator.GetLayerIndex("OneLife"), 0);
            _animator.SetLayerWeight(_animator.GetLayerIndex("TwoLives"), 1);
        }
        _animator.ResetTrigger("Dead");
        // Restore hat
        // Set invincibility
        _isInvincible = true;
        _collider.enabled = false;

        StartCoroutine(InvincibilityFlash());
        Invoke("RemoveInvincibility", 2f); 
        GetComponent<PlayerShooting>()?.DisableShooting(2f);
    }
    private void RemoveInvincibility()
    {
        _isInvincible = false;
        _collider.enabled = true;
        StopCoroutine(InvincibilityFlash());
        _spriteRenderer.color = new Color (1f, 1f, 1f, 1f);
        Debug.Log("Player is now vulnerable again.");
    }
    private IEnumerator InvincibilityFlash()
    {
        while (_isInvincible)
        {
            _spriteRenderer.color = new Color (1f, 1f, 1f, 0.3f); 
            yield return new WaitForSeconds (0.2f);
            _spriteRenderer.color = new Color (1f, 1f, 1f, 0.7f); 
            yield return new WaitForSeconds (0.2f);
        }

    }
}
