using System.Collections;
using System.Threading;
using UnityEngine;

public class WalkBobAndSway : MonoBehaviour
{
    Animator _animator;
    PlayerMovementV2 _pMov;
    CameraController _camController;
    Vector3 _move;
    [SerializeField] Transform _wSway;
    Vector3 _ogPosition;
    bool _wasGrounded;
    float _landingForce;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _pMov = GetComponentInParent<PlayerMovementV2>();
        _camController = GetComponentInParent<CameraController>();
        _ogPosition = _wSway.localPosition;
    }
    void Update()
    {
        _move = InputManager.Instance.Move;
        if (Mathf.Abs(_move.x) < 0.1f) { _move.x = 0; }
        if(Mathf.Abs(_move.y) < 0.1f) { _move.y = 0; }
        _animator.SetBool("moving", _move != Vector3.zero && _pMov.GroundCheck.IsGrounded);
        _animator.SetFloat("velX", _move.x,0.05f, Time.deltaTime);
        _animator.SetFloat("velZ", _move.y,0.05f, Time.deltaTime);
        //weapon sway stuff:
        //Debug.Log(_pMov.Rb.linearVelocity.magnitude);
        //Debug.Log(_pMov.Rb.linearVelocity.y/10);
        if (!_pMov.GroundCheck.IsGrounded || (_pMov.GroundCheck.IsGrounded && _pMov.IsDashing))
        {
            Vector3 leck = transform.InverseTransformDirection(_pMov.Rb.linearVelocity);
            Vector3 sway = new Vector3(-leck.x / 140, -leck.y / 110, -leck.z / 75);
            _wSway.localPosition = Vector3.Lerp(_wSway.localPosition, sway, Time.deltaTime * 12f);
        }
        else if (_pMov.GroundCheck.IsGrounded)
        {
            _landingForce = Mathf.Lerp(_landingForce,0,Time.deltaTime * 22);
            Vector3 swayTarget = _ogPosition + Vector3.down * _landingForce;
            _wSway.localPosition = Vector3.Lerp(_wSway.localPosition, swayTarget, Time.deltaTime * 4);
            _camController.ShakeCamera(_landingForce * 2);
        }
        _wasGrounded = _pMov.GroundCheck.IsGrounded;
        if (!_wasGrounded)
        {
            _landingForce = Mathf.Min(Mathf.Abs(_pMov.Rb.linearVelocity.y) / 10, 3f);
        }
    }
}