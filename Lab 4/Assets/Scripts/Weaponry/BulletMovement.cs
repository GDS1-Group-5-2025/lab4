using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float power = 10f;

    private Camera _camera;
    private Rigidbody2D _rb;

    private bool _hasHitWall;

    private void Start()
    {
        _camera = Camera.main;
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

        // If bullet is out of camera view, destroy it
        var viewPos = _camera.WorldToViewportPoint(transform.position);
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Wall") && !_hasHitWall){ _hasHitWall = true; }
        else{ Destroy(gameObject); }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
