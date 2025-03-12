using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float power = 10f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        // Initial force
        _rb.AddForce(transform.up * power, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (!(_rb.linearVelocity.magnitude > 0.1f))
            return;
        // Calculate angle from velocity
        var angle = Mathf.Atan2(_rb.linearVelocity.y, _rb.linearVelocity.x) * Mathf.Rad2Deg;

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, 0, angle-90);
    }
}
