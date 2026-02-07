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
    float _dashDuraton = 0.8f;
    [HideInInspector] public float LastTimeSinceDash = 0;
    float _currentDashTime = 0;
    float _dashForce = 22f;
    float _dashInvincibilityDuration = 0.25f;
    float _dashCooldown = 3.33f;
    bool _wasGrounded;
    float _landingForce;
    float _dashNoNoTimer = 0.2f;
    float _lastTimeSinceDashNoNo = 0;
    public float DashCooldown => _dashCooldown;
    [SerializeField] AudioClip _lightFallClip;
    [SerializeField] AudioClip _mediumFallClip;
    [SerializeField] AudioClip _heavyFallClip;
    [SerializeField] AudioClip _dashClip;
    [SerializeField] AudioClip _dashNotReadyClip;
    [SerializeField] AudioClip _dashChargingClip;
    public AudioSource PlayerSource;
    Coroutine _dashCoroutine;
    bool _bTOOOOM = false;
    bool _CanRechargeSoundBeTriggered = false;
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        PlayerSource = GetComponent<AudioSource>();
        LastTimeSinceDash = _dashCooldown;
    }
    void Update()
    {
        /* What its supposed to do:
         * Input reading for movement
         * into making sure diagonal movement isnt faster
         * into slopeMovement checking
         * into dash cooldown and input reading and dash triggering
         * into landing impact
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
        if (!_canDash && LastTimeSinceDash < _dashCooldown)
        {
            LastTimeSinceDash += Time.deltaTime;
            if(LastTimeSinceDash / _dashCooldown >= 0.3f && _CanRechargeSoundBeTriggered)
            {
                _CanRechargeSoundBeTriggered = false;
                AudioManager.Instance.PlaySound(_dashChargingClip, 0.9f);
            }
            if (LastTimeSinceDash >= _dashCooldown)
            {
                _canDash = true;
            }
            if(InputManager.Instance.Dash)
            {
                if (Time.time - _lastTimeSinceDashNoNo >= _dashNoNoTimer)
                {
                    AudioManager.Instance.PlaySound(_dashNotReadyClip, 0.6f);
                    _lastTimeSinceDashNoNo = Time.time;
                }
            }
        }
        if (InputManager.Instance.Dash && _canDash && Rb.linearVelocity != Vector3.zero)
        {
            Rb.linearVelocity = new Vector3(0, 0, 0);
            Vector3 dashDir = new Vector3(_movementInput.x, 0, _movementInput.y);
            _dashCoroutine = StartCoroutine(Dash(dashDir));
            _CanRechargeSoundBeTriggered = true;
        }
        if (GroundCheck.IsGrounded && !_wasGrounded)
        {
            _bTOOOOM = true;
            if(_landingForce < 1.2)
            {
                AudioManager.Instance.PlaySound(_lightFallClip, 1.2f);
            }
            else if (_landingForce < 2.5)
            {
                AudioManager.Instance.PlaySound(_mediumFallClip, 1.2f);
            }
            else if(_landingForce <= 3)
            {
                AudioManager.Instance.PlaySound(_heavyFallClip, 1.5f);
            }
        }
        _wasGrounded = GroundCheck.IsGrounded;
        if (!_wasGrounded)
        {
            _landingForce = Mathf.Min(Mathf.Abs(Rb.linearVelocity.y) / 10, 3f);
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
        if (_bTOOOOM)
        {
            _bTOOOOM = false;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
            float distance = 0;
            foreach(Collider collider in colliders)
            {
                if(collider.gameObject.GetComponent<Rigidbody>() != null && collider.gameObject != transform.gameObject)
                {
                    distance = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                    collider.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * (_landingForce * (1.3f/(distance * 0.3f))), ForceMode.Impulse);
                }
            }
        }
    }
    IEnumerator Dash(Vector3 direction)
    {
        _canDash = false;
        _isDashing = true;
        GetComponent<PlayerStats>().Invicibility(_dashInvincibilityDuration);
        LastTimeSinceDash = 0;
        Vector3 dashVelocity = transform.TransformDirection(direction.normalized) * _dashForce;
        _currentDashTime = 0;
        PlayerSource.clip = _dashClip;
        PlayerSource.volume = 1f;
        PlayerSource.pitch = 1f;
        PlayerSource.Play();
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
        StopCoroutine(_dashCoroutine);
        _isDashing = false;
        AudioManager.Instance.FadeOutSound(PlayerSource, 0.1f);
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