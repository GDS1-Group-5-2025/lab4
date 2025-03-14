using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerNumber = 1;
    [SerializeField] private int startingLives = 2;
    [SerializeField] private GameObject weapon;

    private int _currentLives;
    private bool _isInvincible = false;

    private Collider2D _collider;
    private PlayerMovement _playerMovement;
    private BulletManager _bulletManager;
    private Vector3 _startingPosition;
    private Quaternion _startingRotation;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        _bulletManager = FindFirstObjectByType<BulletManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _currentLives = startingLives;
        _startingPosition = transform.position;
        _startingRotation = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only respond if we collided with a "Bullet"
        if (_isInvincible)
        return;
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            collision.gameObject.SetActive(false);
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
                break;
            case <= 0:
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


        // Disable movement
        if (_playerMovement )
            _playerMovement.enabled = false;

        // Disable shooting and aiming
        if (weapon)
            weapon.SetActive(false);
        _bulletManager.shootingEnabled = false;

        // Disable the collider
        _collider.enabled = false;

        // Play death animation

        // Respawn after 2 seconds
        Invoke("Respawn", 2f);
    }

    private void Respawn()
    {
        Debug.Log("Player has respawned");

        // Reset position
        transform.position = _startingPosition;
        transform.rotation = _startingRotation;

        // Reset lives
        _currentLives = startingLives;

        // Re-enable movement
        if (_playerMovement )
            _playerMovement.enabled = true;

        // Re-enable shooting
        if (weapon)
            weapon.SetActive(true);
        _bulletManager.shootingEnabled = true;

        // Re-enable the collider
        _collider.enabled = true;

        // Restore hat
        // Set invincibility
        _isInvincible = true;
        _collider.enabled = false;

        StartCoroutine(InvincibilityFlash());
        Invoke("RemoveInvincibility", 2f); 
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
