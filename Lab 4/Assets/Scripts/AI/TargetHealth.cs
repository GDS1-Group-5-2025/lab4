using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class TargetHealth : MonoBehaviour
{
    [FormerlySerializedAs("_shakeDuration")]
    [SerializeField] private float shakeDuration = 0.2f;
    [FormerlySerializedAs("_shakeMagnitude")]
    [SerializeField] private float shakeMagnitude = 0.1f;

    private BulletManager _bulletManager;

    private void Awake()
    {
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
        var originalPosition = transform.localPosition;
        var elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Calculate random offset within the shake magnitude and add to original position
            var x = Random.Range(-1f, 1f) * shakeMagnitude;
            var y = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset back to the original position
        transform.localPosition = originalPosition;
    }
}
