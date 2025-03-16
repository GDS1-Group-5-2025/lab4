using UnityEngine;
using System.Collections;

public class EnemyShooting : BaseShooting
{
    public float detectionRange = 10f;
    public float shootingOffsetRadius = 1f;

    private Transform player;

    protected override void Start()
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        base.Start();
    }

    protected override void Update()
    {
        // Still call the base update for reload logic, etc.
        base.Update();

        if (!canShoot) return;
        if (_isReloading) return;
        if (player == null) return;

        var distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            // AI logic to aim and shoot
            var targetPos = (Vector2)player.position + Random.insideUnitCircle * shootingOffsetRadius;
            var direction = (targetPos - (Vector2)transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f;

            // Attempt to shoot
            var spawnPos = (Vector2)transform.position + direction * 1f;
            AttemptShoot(spawnPos, Quaternion.Euler(0f, 0f, angle));
        }
    }
}
