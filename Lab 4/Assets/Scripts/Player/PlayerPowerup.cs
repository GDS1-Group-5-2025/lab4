using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerPowerup : MonoBehaviour
{
    public int maxBounces = 1;
    public int maxSegments = 20;
    public float rayLength = 30f;
    public GameObject aim;
    public GameObject aimIndicator;

    private LineRenderer _lineRenderer;
    private PowerupManager _powerupManager;

    private void Awake()
    {
        _powerupManager = FindFirstObjectByType<PowerupManager>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);
    }

    private void Update()
    {
        SimulateTrajectory(aim.transform.position + aim.transform.up, aim.transform.up, maxBounces);
    }

    private void OnEnable()
    {
        aimIndicator.SetActive(false);
        _lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        aimIndicator.SetActive(true);
        _lineRenderer.enabled = false;
    }

    public void TakeDamage()
    {
        if (enabled)
        {
            _powerupManager.PowerupLost();
        }
    }

    private void SimulateTrajectory(Vector3 startPos, Vector3 direction, int bouncesRemaining)
    {
        var positions = new Vector3[maxSegments];
        var segments = 1;
        positions[0] = startPos;

        var ray = new Ray2D(startPos, direction);
        var remainingLength = rayLength;

        while (bouncesRemaining >= 0 && segments < maxSegments && remainingLength > 0)
        {
            var hit = Physics2D.Raycast(ray.origin, ray.direction, remainingLength);
            if (hit)
            {
                positions[segments] = hit.point;
                segments++;

                if (hit.collider.CompareTag("Wall"))
                {
                    if (bouncesRemaining > 0)
                    {
                        // Calculate reflection direction
                        ray.direction = Vector2.Reflect(ray.direction, hit.normal);
                        ray.origin = hit.point + ray.direction * 0.01f; // Small offset to avoid hitting same point
                        remainingLength -= Vector3.Distance(positions[segments - 2], positions[segments - 1]);
                        bouncesRemaining--;
                        continue;
                    }
                }
                break;
            }

            // No hit, draw to maximum distance
            positions[segments] = ray.origin + ray.direction * remainingLength;
            segments++;
            break;
        }

        // Update line renderer
        _lineRenderer.positionCount = segments;
        for (var i = 0; i < segments; i++)
        {
            _lineRenderer.SetPosition(i, positions[i]);
        }
    }
}
