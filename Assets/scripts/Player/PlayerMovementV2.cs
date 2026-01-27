using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementV2 : MonoBehaviour
{
    Vector2 _movementInput;
    Vector3 _moveDir;
    Vector3 _moveRelative;
    Rigidbody _rb;
    float _moveSpeed = 13f;
    public PlayerGroundCheck Gc;
    Vector3 _velocityChange;
    bool _canDash = true;
    bool _isDashing = false;
    public bool CanDash => _canDash;
    public bool IsDashing => _isDashing;
    float _dashDuraton = 0.5f;
    float _lastTimeSinceDash = 0;
    float _dashForce = 25f;
    float _dashInvincibilityDuration = 0.25f;
    float _dashCooldown = 3.33f;
    public float DashCooldown => _dashCooldown;
    public float LastTimeSinceDash => _lastTimeSinceDash;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Gc = GetComponentInChildren<PlayerGroundCheck>();
        _lastTimeSinceDash = _dashCooldown;
    }
    void Update()
    {
        /* What its supposed to do:
         * Input reading for movement
         * into making sure diagonal movement isnt faster
         * into dash cooldown and input reading and dash triggering
         */
        _movementInput = InputManager.Instance.Move;
        _moveDir = new Vector3(_movementInput.x, 0, _movementInput.y);
        if (_moveDir.magnitude > 1)
        {
            _moveDir = _moveDir.normalized;
        }
        if (!_canDash && _lastTimeSinceDash < _dashCooldown)
        {
            _lastTimeSinceDash += Time.deltaTime;
            if (_lastTimeSinceDash >= _dashCooldown)
            {
                _canDash = true;
            }
        }
        if (InputManager.Instance.Dash && _canDash && _rb.linearVelocity != Vector3.zero)
        {
            _rb.linearVelocity = new Vector3(0, 0, 0);
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
        Vector3 targetVelocity = _moveRelative * _moveSpeed;
        Vector3 horizontalRbVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        if (Gc.IsGrounded)
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
    IEnumerator Dash(Vector3 direction)
    {
        _canDash = false;
        _isDashing = true;
        GetComponent<PlayerStats>().Invicibility(_dashInvincibilityDuration);
        _lastTimeSinceDash = 0;
        Vector3 dashVelocity = transform.TransformDirection(direction.normalized) * _dashForce;
        float timer = 0;
        while (timer < _dashDuraton)
        {
            _rb.linearVelocity = dashVelocity;
            timer += Time.deltaTime;
            yield return null;
        }
        _isDashing = false;
        //OnDashEnd?.Invoke();
    }
}