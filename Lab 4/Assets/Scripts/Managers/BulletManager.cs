using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    public bool shootingEnabled = true;
    public GameObject bulletPrefab;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _hitSound;

    private readonly List<GameObject> _bullets = new List<GameObject>();

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayHitSound()
    {
        _audioSource.PlayOneShot(_hitSound);
    }

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
