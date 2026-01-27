using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementV2 : MonoBehaviour
{
    Vector2 _movementInput;
    Vector3 _moveDir;
    Vector3 _moveRelative;
    Rigidbody _rb;
    float _moveSpeed = 13f;
    PlayerGroundCheck _gc;
    Vector3 _velocityChange;
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
        Vector3 targetVelocity = _moveRelative * _moveSpeed;
        Vector3 horizontalRbVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        if (_gc.IsGrounded)
        {
            _velocityChange = targetVelocity - horizontalRbVelocity;
            _rb.AddForce(_velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 airVelocity = (targetVelocity - horizontalRbVelocity) * 0.15f;
            Vector3 airVelocityDragThing = Vector3.ClampMagnitude(airVelocity, 0.5f);
            _rb.AddForce(airVelocityDragThing, ForceMode.VelocityChange);
            _rb.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
    }
}