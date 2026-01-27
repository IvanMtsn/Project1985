using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public int MoveSpeed = 12;
    int _propulsionForce = 25;
    float _rotation = 0;
    float _rotationX = 0;
    float _mouseSensitivityHorizontal = 0.1f;
    float _mouseSensitivityVertical = 0.1f;
    float _dashDuraton = 0.5f;
    float _dashInvincibilityDuration = 0.35f;
    public float DashCooldown { get; private set; } = 3.33f;
    float _AngleUntilSlope = 0.01f;
    public float LastTimeSinceDash { get; private set; }
    public Camera PlayerCam;
    Rigidbody _rb;
    Vector3 _moveDirection;
    Vector3 _moveRelative;
    Vector2 _movementInput;
    Vector2 _lookInput;
    Coroutine _dashCoroutine;
    bool _isGrounded = true;
    public bool CanDash { get; private set; } = true;
    public bool IsDashing { get; private set; } = false;
    public bool IsGrounded => _isGrounded;
    public System.Action OnDashEnd;
    [SerializeField]LayerMask _groundLayer; 
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        LastTimeSinceDash = DashCooldown;
    }
    private void Update()
    {
        if(!CanDash && LastTimeSinceDash < DashCooldown)
        {
            LastTimeSinceDash += Time.deltaTime;
            if(LastTimeSinceDash >= DashCooldown)
            {
                CanDash = true;
            }
        }
        if (InputManager.Instance.Dash && CanDash && _rb.linearVelocity != Vector3.zero)
        {
            _rb.linearVelocity = new Vector3(0, 0, 0);
            Vector3 dashDir = new Vector3(_movementInput.x, 0, _movementInput.y);
            _dashCoroutine = StartCoroutine(Dash(dashDir));
        }
        Movement();
    }
    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(transform.position - Vector3.up * 0.9f, 0.45f, _groundLayer);
    }
    private void Movement()
    {
        _movementInput = InputManager.Instance.Move;
        _lookInput = InputManager.Instance.Look;

        _moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
        if(_moveDirection.magnitude > 1)
        {
            _moveDirection = _moveDirection.normalized;
        }

        if(_lookInput.magnitude < 0.1) { _lookInput = Vector2.zero; }

        _rotation += -_lookInput.y * _mouseSensitivityVertical;
        _rotationX += _lookInput.x * _mouseSensitivityHorizontal;
        _rotation = Mathf.Clamp(_rotation, -89, 89);

        _moveRelative = transform.TransformDirection(_moveDirection);
        if(IsOnSlope(out Vector3 slopeDirection))
        {
            _rb.useGravity = false;
            _rb.linearVelocity = slopeDirection * MoveSpeed;
        }
        else
        {
            _rb.useGravity = true;
            _rb.linearVelocity = new Vector3(_moveRelative.x * MoveSpeed, _rb.linearVelocity.y, _moveRelative.z * MoveSpeed);
        }

        Quaternion rotX = Quaternion.Euler(0, _rotationX, 0);
        _rb.MoveRotation(rotX);
        PlayerCam.transform.localRotation = Quaternion.Euler(_rotation, 0, 0);
    }
    IEnumerator Dash(Vector3 direction)
    {
        CanDash = false;
        IsDashing = true;
        GetComponent<PlayerStats>().Invicibility(_dashInvincibilityDuration);
        LastTimeSinceDash = 0;
        Vector3 dashVelocity = transform.TransformDirection(direction.normalized) * _propulsionForce;
        float timer = 0;
        while(timer < _dashDuraton)
        {
            _rb.linearVelocity = dashVelocity;
            timer += Time.deltaTime;
            yield return null;
        }
        IsDashing = false;
        OnDashEnd?.Invoke();
    }
    public void EndDash()
    {
        StopCoroutine(_dashCoroutine);
        _dashCoroutine = null;
        IsDashing = false;
        _rb.linearVelocity = Vector3.zero;
        OnDashEnd?.Invoke();
    }
    bool IsOnSlope(out Vector3 slopeDirection)
    {
        slopeDirection = Vector3.zero;
        RaycastHit hit;
        Vector3 origin = transform.position - Vector3.up * 0.9f;
        if(Physics.Raycast(origin, Vector3.down, out hit, 0.3f, _groundLayer))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if(angle > _AngleUntilSlope && angle < 45)
            {
                slopeDirection = Vector3.ProjectOnPlane(_moveRelative, hit.normal).normalized;
                //Debug.Log("on slope rn");
                return true;
            }
        }
        return false;
    }
}
