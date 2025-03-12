using UnityEngine;

public class TargetHealth : MonoBehaviour
{

    private BulletManager _bulletManager;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _bulletManager = FindFirstObjectByType<BulletManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TargetHit(1);
            collision.gameObject.SetActive(false);
        }
    }

    private void TargetHit(int damage)
    {
        _bulletManager.ClearBullets();
        ScoreManager.Instance.IncrementScoreForTarget();

    }
}
