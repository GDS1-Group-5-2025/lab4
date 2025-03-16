using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("Aim Settings")]
    public float speed;
    public bool invertAim;
    [Range(0, 90)]
    public float range = 80;

    private float _aimInput;
    private float _currentAimAngle;

    private PlayerInput _playerInput;
    private InputActionMap _playerActionMap;

    private void Start()
    {
        _playerInput = GetComponentInParent<PlayerInput>();

        // Get current player input action map
        _playerActionMap = _playerInput.currentActionMap;

        _currentAimAngle = 90;
        transform.rotation = Quaternion.Euler(0, 0, _currentAimAngle * (invertAim ? -1 : 1));
    }

    private void Update()
    {
        if (_aimInput == 0) return;

        // Update aim angle
        _currentAimAngle += _aimInput * (invertAim ? 1 : -1) * speed * Time.deltaTime;

        // Limit angle
        _currentAimAngle = Mathf.Clamp(_currentAimAngle, 90 - range, 90 + range);

        // Rotate go to aim angle
        transform.rotation = Quaternion.Euler(0, 0, _currentAimAngle * (invertAim ? -1 : 1));

    }

    public void MoveAim(InputAction.CallbackContext ctx)
    {
        if (ctx.action.actionMap == _playerActionMap)
        {
            _aimInput = ctx.ReadValue<float>();
        }
    }
}
