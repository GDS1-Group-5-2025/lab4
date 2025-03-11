using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int startingLives = 2;
    [SerializeField] private Vector2 startingPosition;

    private int currentLives;

    
    private Collider2D _collider;
    private PlayerMovement _playerMovement;
    // private PlayerShooting _playerShooting;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _playerMovement = GetComponent<PlayerMovement>();
        // _playerShooting = GetComponent<PlayerShooting>();
    }

    private void Start()
    {
        currentLives = startingLives;
        startingPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only respond if we collided with a "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        currentLives -= damage;

        // Example: If the player still has 1 life left, remove the player's hat
        if (currentLives == 1)
        {
            Debug.Log("Player has lost a life");
            // Remove the player's hat here
        }

        if (currentLives <= 0)
        {
            PlayerDeath();
            Debug.Log("Player has died");
        }
    }

    private void PlayerDeath()
    {
        // Disable movement
        if (_playerMovement != null)
            _playerMovement.enabled = false;

        // Disable shooting
        // if (_playerShooting != null)
        //     _playerShooting.enabled = false;

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
        transform.position = startingPosition;

        // Reset lives
        currentLives = startingLives;

        // Re-enable movement
        if (_playerMovement != null)
            _playerMovement.enabled = true;

        // Re-enable shooting
        // if (_playerShooting != null)
        //     _playerShooting.enabled = true;

        // Re-enable the collider
        _collider.enabled = true;

        // Restore hat
    }
}
