using UnityEngine;

public class BulletTrailFactory : MonoBehaviour
{
    public GameObject bulletTrailPrefab;
    public float trailEmissionRate;

    private float _timeSinceLastEmission;

    // Update is called once per frame
    private void Update()
    {
        _timeSinceLastEmission += Time.deltaTime;
        if (!(_timeSinceLastEmission >= 1f / trailEmissionRate)) return;
        _timeSinceLastEmission = 0;
        Instantiate(bulletTrailPrefab, transform.position, Quaternion.identity);
    }
}
