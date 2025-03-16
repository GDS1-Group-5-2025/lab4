using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IMovement
{
    [Header("Movement Settings")]
    public float speed;

    private float _movementInput;

    private Rigidbody2D _rb;
    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;
    private Animator _animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();

        // Get current player input action map
        _playerActionMap = _playerInput.currentActionMap;
    }

    void Update()
    {
        // Preserving x-component in case rb x-location is not frozen
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _movementInput * speed);

        if (_animator != null)
        {
            _animator.SetFloat("Speed", Mathf.Abs(_movementInput * speed));
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap == _playerActionMap)
        {
            _movementInput = ctx.ReadValue<float>();
        }
    }
}
