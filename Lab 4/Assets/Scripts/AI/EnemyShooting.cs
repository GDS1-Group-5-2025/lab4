using UnityEngine;
using System.Collections;

public class EnemyShooting : BaseShooting
{
    public float detectionRange = 10f;
    public float shootingOffsetRadius = 1f;

    private Transform player;
    private bool canShoot = true;

    protected override void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        base.Start();
    }

    private void OnEnable()
    {
        // Subscribe to any player’s death
        PlayerHealth.OnAnyPlayerDied += HandleAnyPlayerDied;
    }

    private void OnDisable()
    {
        // Unsubscribe when disabled/destroyed
        PlayerHealth.OnAnyPlayerDied -= HandleAnyPlayerDied;
    }

    private void HandleAnyPlayerDied()
    {
        // Stop shooting for 2 seconds
        StartCoroutine(StopShootingTemporarily(2f));
    }

    private IEnumerator StopShootingTemporarily(float duration)
    {
        canShoot = false;
        yield return new WaitForSeconds(duration);
        canShoot = true;
    }

    protected override void Update()
    {
        // Still call the base update for reload logic, etc.
        base.Update();

        if (!canShoot) return;
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            // AI logic to aim and shoot
            Vector2 targetPos = (Vector2)player.position + Random.insideUnitCircle * shootingOffsetRadius;
            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f;

            // Attempt to shoot
            Vector2 spawnPos = (Vector2)transform.position + direction * 1f;
            AttemptShoot(spawnPos, Quaternion.Euler(0f, 0f, angle));
        }
    }
}

