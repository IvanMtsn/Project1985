using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementV2 : MonoBehaviour
{
    Vector2 _movementInput;
    Vector3 _moveDir;
    Vector3 _moveRelative;
    Rigidbody _rb;
    [SerializeField] GameObject _playerCam;
    float _moveSpeed = 13f;
    PlayerGroundCheck _gc;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gc = GetComponent<PlayerGroundCheck>();

    }
    void Update()
    {
        _movementInput = InputManager.Instance.Move;
        _moveDir = new Vector3(_movementInput.x, 0, _movementInput.y);
        if (_moveDir.magnitude > 1)
        {
            _moveDir = _moveDir.normalized;
        }
    }
    void FixedUpdate()
    {
        _moveRelative = transform.TransformDirection(_moveDir);
        if (_gc.IsGrounded)
        {
            Vector3 targetVelocity = _moveRelative * _moveSpeed;
            Vector3 velocityChange = targetVelocity - new Vector3(_rb.linearVelocity.x,0,_rb.linearVelocity.z);
            _rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
}
