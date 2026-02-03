using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementV2 : MonoBehaviour
{
    Vector2 _movementInput;
    Vector3 _moveDir;
    Vector3 _moveRelative;
    [HideInInspector] public Rigidbody Rb;
    float _moveSpeed = 13f;
    public PlayerGroundCheck GroundCheck;
    public PlayerGroundCheck SlopeCheck;
    Vector3 _velocityChange;
    bool _canDash = true;
    bool _isDashing = false;
    public bool CanDash => _canDash;
    public bool IsDashing => _isDashing;
    float _dashDuraton = 0.5f;
    float _lastTimeSinceDash = 0;
    float _currentDashTime = 0;
    float _dashForce = 25f;
    float _dashInvincibilityDuration = 0.25f;
    float _dashCooldown = 3.33f;
    public float DashCooldown => _dashCooldown;
    public float LastTimeSinceDash => _lastTimeSinceDash;
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        _lastTimeSinceDash = _dashCooldown;
    }
    void Update()
    {
        /* What its supposed to do:
         * Input reading for movement
         * into making sure diagonal movement isnt faster
         * into slopeMovement checking
         * into dash cooldown and input reading and dash triggering
         */

        _movementInput = InputManager.Instance.Move;
        _moveDir = new Vector3(_movementInput.x, 0, _movementInput.y);
        if (_moveDir.magnitude > 1)
        {
            _moveDir = _moveDir.normalized;
        }
        if (SlopeCheck.IsGrounded && _moveDir.magnitude == 0)
        {
            Rb.linearVelocity = Vector3.zero;
            Rb.useGravity = false;
        }
        else
        {
            Rb.useGravity = true;
        }
        if (!_canDash && _lastTimeSinceDash < _dashCooldown)
        {
            _lastTimeSinceDash += Time.deltaTime;
            if (_lastTimeSinceDash >= _dashCooldown)
            {
                _canDash = true;
            }
        }
        if (InputManager.Instance.Dash && _canDash && Rb.linearVelocity != Vector3.zero)
        {
            Rb.linearVelocity = new Vector3(0, 0, 0);
            Vector3 dashDir = new Vector3(_movementInput.x, 0, _movementInput.y);
            StartCoroutine(Dash(dashDir));
        }
    }
    void FixedUpdate()
    {
        if(_isDashing)
        {
            return;
        }
        _moveRelative = transform.TransformDirection(_moveDir);
        //Debug.Log(_moveDir.magnitude);
        _moveRelative = AlignToSurface(_moveRelative);
        Vector3 targetVelocity = _moveRelative * _moveSpeed;
        Vector3 horizontalRbVelocity = new Vector3(Rb.linearVelocity.x, 0, Rb.linearVelocity.z);
        if (GroundCheck.IsGrounded)
        {
            if (SlopeCheck.IsGrounded && Rb.linearVelocity.y > 0)
            {
                Rb.linearVelocity = horizontalRbVelocity;
        }
            _velocityChange = targetVelocity - horizontalRbVelocity;
            Rb.AddForce(_velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 airVelocity = (targetVelocity - horizontalRbVelocity) * 0.15f;
            Vector3 airVelocityDragThing = Vector3.ClampMagnitude(airVelocity, 0.5f);
            Rb.AddForce(airVelocityDragThing, ForceMode.VelocityChange);
            Rb.AddForce(Physics.gravity * 3f, ForceMode.Acceleration);
        }
    }
    IEnumerator Dash(Vector3 direction)
    {
        _canDash = false;
        _isDashing = true;
        GetComponent<PlayerStats>().Invicibility(_dashInvincibilityDuration);
        _lastTimeSinceDash = 0;
        Vector3 dashVelocity = transform.TransformDirection(direction.normalized) * _dashForce;
        _currentDashTime = 0;
        while (_currentDashTime < _dashDuraton)
        {
            Rb.linearVelocity = dashVelocity;
            _currentDashTime += Time.deltaTime;
            yield return null;
        }
        _isDashing = false;
    }
    public void EndDash()
    {
        StopCoroutine(Dash(Vector3.zero));
        _isDashing = false;

    }
    public Vector3 AlignToSurface(Vector3 movementDir)
    {
        if (SlopeCheck.IsGrounded && Physics.Raycast(GroundCheck.transform.position, -transform.up, out RaycastHit hit, 0.25f, GroundCheck.GroundLayer))
        {
            Vector3 slopeDir = Vector3.ProjectOnPlane(movementDir, hit.normal).normalized;
            return slopeDir * movementDir.magnitude;
        }
        return movementDir;
    }
}