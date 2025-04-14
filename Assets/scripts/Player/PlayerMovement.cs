using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    int _moveSpeed = 8;
    int _propulsionForce = 15;
    float _rotation = 0;
    float _rotationX = 0;
    float _mouseSensitivityHorizontal = 0.1f;
    float _mouseSensitivityVertical = 0.1f;
    float _controllerSensitivityHorizontal = 1f;
    float _controllerSensitivityVertical = 0.6f;
    float _dashDuraton = 0.35f;
    public float DashCooldown { get; private set; } = 2f;
    float _AngleUntilSlope = 0.01f;
    public float LastTimeSinceDash { get; private set; }
    public Camera PlayerCam;
    Rigidbody _rb;
    Vector3 _moveDirection;
    Vector3 _moveRelative;
    Vector2 _movementInput;
    Vector2 _lookInput;
    bool _isGrounded = true;
    public bool CanDash { get; private set; } = true;
    public bool _isDashing { get; private set; } = false;
    [SerializeField]LayerMask _groundLayer;
    public InputActionReference _moveReference;
    public InputActionReference _lookReference;    
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
        Movement();
    }
    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(transform.position - Vector3.up * 0.9f, 0.45f, _groundLayer);
    }
    private void Movement()
    {
        // taking movement input
        _moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
        //moveDirection MUST not be normalized for controller smooth movement to work, but it must be normalized if someone plays w keyboard
        if(_moveDirection.magnitude > 1)
        {
            _moveDirection = _moveDirection.normalized;
        }

        // taking mouse movement for looking
        //checks active device, ? is for making sure null is returned instead of a crash occuring
        var device = _lookReference.action.activeControl?.device;
        float sensitivityHorizontal;
        float sensitivityVertical;
        switch (device)
        {
            case Mouse: sensitivityHorizontal = _mouseSensitivityHorizontal;
                sensitivityVertical = _mouseSensitivityVertical;
                break;
            case Gamepad: sensitivityHorizontal = _controllerSensitivityHorizontal;
                sensitivityVertical = _controllerSensitivityVertical;
                break;
            default: sensitivityHorizontal = _mouseSensitivityHorizontal;
                sensitivityVertical = _mouseSensitivityVertical;
                break;
        }
        if(_lookInput.magnitude < 0.1) { _lookInput = Vector2.zero; }

        _rotation += -_lookInput.y * sensitivityVertical;
        _rotationX += _lookInput.x * sensitivityHorizontal;
        _rotation = Mathf.Clamp(_rotation, -89, 89);

        // processing movement input
        _moveRelative = transform.TransformDirection(_moveDirection);
        if(IsOnSlope(out Vector3 slopeDirection))
        {
            _rb.useGravity = false;
            _rb.velocity = slopeDirection * _moveSpeed;
        }
        else
        {
            _rb.useGravity = true;
            _rb.velocity = new Vector3(_moveRelative.x * _moveSpeed, _rb.velocity.y, _moveRelative.z * _moveSpeed);
        }

        // processing mouse input looking
        Quaternion rotX = Quaternion.Euler(0, _rotationX, 0);
        _rb.MoveRotation(rotX);
        PlayerCam.transform.localRotation = Quaternion.Euler(_rotation, 0, 0);
    }
    void OnMove()
    {
        _movementInput = _moveReference.action.ReadValue<Vector2>();
    }
    void OnLook()
    {
        _lookInput = _lookReference.action.ReadValue<Vector2>();
    }
    private void OnDash()
    {
        if (CanDash && _rb.velocity != Vector3.zero && _isGrounded)
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            Vector3 dashDir = new Vector3(_movementInput.x, 0, _movementInput.y);
            StartCoroutine(Dash(dashDir));
        }
    }
    IEnumerator Dash(Vector3 direction)
    {
        CanDash = false;
        _isDashing = true;
        LastTimeSinceDash = 0;
        Physics.IgnoreLayerCollision(3, 7, true);
        Vector3 dashVelocity = transform.TransformDirection(direction.normalized) * _propulsionForce;
        float timer = 0;
        while(timer < _dashDuraton)
        {
            _rb.velocity += dashVelocity;
            timer += Time.deltaTime;
            yield return null;
        }
        _isDashing = false;
        Physics.IgnoreLayerCollision(3, 7, false);
    }
    bool IsOnSlope(out Vector3 slopeDirection)
    {
        slopeDirection = Vector3.zero;
        RaycastHit hit;
        Vector3 origin = transform.position - Vector3.up * 0.9f;
        if(Physics.Raycast(origin, Vector3.down, out hit, 0.3f, _groundLayer))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if(angle > _AngleUntilSlope)
            {
                slopeDirection = Vector3.ProjectOnPlane(_moveRelative, hit.normal).normalized;
                Debug.Log("geht");
                return true;
            }
        }
        return false;
    }
    private void OnEnable()
    {
        _moveReference.action.Enable();
        _lookReference.action.Enable();
    }
    private void OnDisable()
    {
        _moveReference.action.Disable();
        _lookReference.action.Disable();
    }
}
