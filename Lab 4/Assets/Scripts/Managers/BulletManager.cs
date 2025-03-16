using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    public bool shootingEnabled = true;
    private readonly List<GameObject> _bullets = new List<GameObject>();

    public void Shoot(Vector3 from, Quaternion at)
    {
        if (!shootingEnabled) return;

        var bullet = Instantiate(bulletPrefab, from, at);
        bullet.transform.SetParent(transform);
        _bullets.Add(bullet);
    }

    public void ClearBullets()
    {
        foreach (var bullet in _bullets)
        {
            Destroy(bullet);
        }
        _bullets.Clear();
    }
}
