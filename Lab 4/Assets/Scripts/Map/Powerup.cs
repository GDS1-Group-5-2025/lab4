using System;
using UnityEngine;

/**
 * Powerup class
 *
 * This is the trigger for the powerup. When the player collides with this object, the powerup is activated.
 *
 * Logic:
 *
 * - When brought to life, will move on the x-axis at a speed of 1 unit per second towards the center of the screen.
 * - Will stop at the same x position as the player closest to it.
 * - The y should not change
 * - It will stay there for tauntTime seconds
 * - After tauntTime seconds, it will move off the screen at a speed of 1 unit per second
 * - Once off-screen, it will let the PowerupManager know that it has expired [PowerupExpired]
 * - If the player collides with the powerup, the PowerupManager will be notified [PowerupPickedUp]
 */
public class Powerup : MonoBehaviour
{

    private enum State
    {
        GoingToPlayer,
        Taunting,
        Leaving
    }

    public float speed = 1f;
    public float tauntTime = 5f;

    private PowerupManager _powerupManager;

    private Vector3 _targetPosition;
    private Vector3 _startPosition;

    private State _state = State.GoingToPlayer;

    private void Start()
    {
        _powerupManager = FindFirstObjectByType<PowerupManager>();
        _startPosition = transform.position;

        // find the player closest to the powerup ( players will be either on +x or -x)
        // so we just need to find the player with the same x sign as the powerup
        var players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);

        var closestPlayer = players[0];
        foreach (var player in players)
        {
            if (player.transform.position.x * transform.position.x > 0)
            {
                closestPlayer = player;
                break;
            }
        }

        // target position is the same x as the player, but the y is the same as the powerup
        _targetPosition = new Vector3(closestPlayer.transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (_state)
        {
            case State.GoingToPlayer:
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
                if (transform.position == _targetPosition)
                {
                    _state = State.Taunting;
                }
                break;
            case State.Taunting:
                tauntTime -= Time.deltaTime;
                if (tauntTime <= 0)
                {
                    _state = State.Leaving;
                }
                break;
            case State.Leaving:
                transform.position = Vector3.MoveTowards(transform.position, _startPosition, speed * Time.deltaTime);
                if (transform.position == _startPosition)
                {
                    _powerupManager.PowerupExpired();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // When the player collides with the powerup, the PowerupManager will be notified
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _powerupManager.PowerupPickedUp();
        }
    }
}
