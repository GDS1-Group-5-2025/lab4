using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Movement Controls")]
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float vertical = 0f;

        // Using input keys specified in the inspector
        if (Input.GetKey(upKey))
        {
            vertical = 1f;
        }
        else if (Input.GetKey(downKey))
        {
            vertical = -1f;
        }

        // Preserving x-component in case rb x-location is not frozen
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * speed);
    }
}
