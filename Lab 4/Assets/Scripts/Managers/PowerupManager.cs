using System;
using UnityEngine;
using UnityEngine.SearchService;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


/**
 * Manages powerups in the game
 *
 * Rules:
 *
 * - Powerups are spawned at random intervals after 20 seconds of game time
 * - Minimum 10 seconds between powerups
 * - Powerups are spawned at random locations on either side (player)
 * - Powerups only spawn for human players
 * - Powerups spawn first for the losing side (if score difference) or randomly
 * - Power ups will stay on spot for 4 seconds
 * - If not picked up, powerups will go away and try the other side.
 * - They will keep switching sides until picked up by someone or game over
 */
public class PowerupManager : MonoBehaviour
{
    public GameObject powerupPrefab;

    public float initialPowerupSpawnTime = 20f;
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 20f;
    public PlayerPowerup[] players;

    [FormerlySerializedAs("_screenBounds")]
    public Rect playableBounds;

    private int _activePowerupIndex = -1;
    private GameObject _powerupPickup;
    private int _currentPlayerTauntedIndex = -1;
    private float _timeSinceLastPowerup;
    private bool _initialPowerupSpawned;

    private float _timeUntilNextPowerupSpawn;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(playableBounds.center, new Vector3(playableBounds.width, playableBounds.height, 1));
    }

    private void Start()
    {
        _timeUntilNextPowerupSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        _timeSinceLastPowerup += Time.deltaTime;

        // initial powerup spawn
        if (!_initialPowerupSpawned)
        {
            if (_timeSinceLastPowerup >= initialPowerupSpawnTime)
            {
                ChooseSide();
                SpawnPowerup();
                _initialPowerupSpawned = true;
            }
        }
        else if (_currentPlayerTauntedIndex == -1 && _timeSinceLastPowerup >= _timeUntilNextPowerupSpawn && _activePowerupIndex == -1)
        {
            ChooseSide();
            SpawnPowerup();
        }
    }

    private void ChooseSide()
    {
        if (players.Length > 0) // decide which side to go for
        {
            // check scores
            var winningPlayer = ScoreManager.Instance.GetWinningPlayer();
            _currentPlayerTauntedIndex = winningPlayer switch
            {
                0 => Random.Range(0, players.Length),
                1 => 0,
                2 => 1,
                _ => Random.Range(0, players.Length)
            };
        }
        else
        {
            _currentPlayerTauntedIndex = 0;
        }
    }

    private void SpawnPowerup()
    {
        var player = players[_currentPlayerTauntedIndex];

        // Find out if the player is closer to the top or the bottom of the screen
        var position = player.transform.position - new Vector3(playableBounds.center.x, playableBounds.center.y, 0);

        // Players are always away from playableBounds.center, either - something or + something. so we can use this to determine the side
        var side = Mathf.Sign(position.x);

        // Choose position farthest from the player
        var yMax = Mathf.Sign(position.y) * ((playableBounds.height / 2) + playableBounds.center.y);

        // Now we spawn the powerup
        var spawnPosition = new Vector3
        {
            // y 20% away from yMax towards the center
            y = playableBounds.center.y + (yMax * 0.8f),
            // x position will be just off the screen, on the side of the player
            x = playableBounds.center.x + (side * (playableBounds.width / 2 + 1)),
        };

        _powerupPickup = Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
    }

    // Called by the power up on contact with player
    public void PowerupPickedUp()
    {
        // start powerup timer for next one
        _timeSinceLastPowerup = 0f;
        _timeUntilNextPowerupSpawn = float.MaxValue;
        // powerup picked up by player
        _activePowerupIndex = _currentPlayerTauntedIndex;
        // apply powerup effect
        players[_activePowerupIndex].enabled = true;
        // remove powerup from scene
        Destroy(_powerupPickup);
        // clear variables
        _powerupPickup = null;
        _currentPlayerTauntedIndex = -1;
    }

    // Called by the player when they lose the powerup (received damage)
    public void PowerupLost()
    {
        // start powerup timer for next one
        _timeSinceLastPowerup = 0f;
        _timeUntilNextPowerupSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
        // remove powerup effect
        players[_activePowerupIndex].enabled = false;
        // powerup lost by player
        // clear variables
        _activePowerupIndex = -1;
        _powerupPickup = null;
        _currentPlayerTauntedIndex = -1;
    }

    // Called by the powerup when it expires
    public void PowerupExpired()
    {
        // powerup expired
        // remove powerup from scene
        Destroy(_powerupPickup);
        // switch sides
        _currentPlayerTauntedIndex = _currentPlayerTauntedIndex == 0 ? 1 : 0;
        // clear variables
        _powerupPickup = null;
        _activePowerupIndex = -1;

        // if powerup was not picked up, spawn on the other side
        SpawnPowerup();
    }

    // Called when the game is over or restarted
    public void Reset()
    {
        _activePowerupIndex = -1;
        Destroy(_powerupPickup);
        _powerupPickup = null;
        _currentPlayerTauntedIndex = -1;
        _timeSinceLastPowerup = 0f;
        _initialPowerupSpawned = false;
        _timeUntilNextPowerupSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
