using UnityEngine;
using System.Collections;

public class TargetHealth : MonoBehaviour
{
    private BulletManager _bulletManager;
    private Collider _collider;
    private float _shakeDuration = 0.2f;
    private float _shakeMagnitude = 0.1f;  // Fixed missing semicolon

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _bulletManager = FindFirstObjectByType<BulletManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            TargetHit(1);
        }
    }

    private void TargetHit(int damage)
    {
        _bulletManager.ClearBullets();
        ScoreManager.Instance.IncrementScoreForTarget();
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        // Store the original local position of the target
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < _shakeDuration)
        {
            // Calculate random offset within the shake magnitude and add to original position
            float x = Random.Range(-1f, 1f) * _shakeMagnitude;
            float y = Random.Range(-1f, 1f) * _shakeMagnitude;
            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset back to the original position
        transform.localPosition = originalPosition;
    }
}
