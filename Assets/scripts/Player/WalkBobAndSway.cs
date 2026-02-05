using System.Collections;
using System.Threading;
using UnityEngine;

public class WalkBobAndSway : MonoBehaviour
{
    Animator _animator;
    PlayerMovementV2 _pMov;
    Vector3 _move;
    Vector3 _airMove;
    [SerializeField] Transform _wSway;
    Vector3 _ogPosition;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _pMov = GetComponentInParent<PlayerMovementV2>();
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
        if (!_pMov.GroundCheck.IsGrounded)
        {
            Vector3 leck = transform.InverseTransformDirection(_pMov.Rb.linearVelocity);
            _airMove = new Vector3(-leck.x / 140, -leck.y / 110, -leck.z / 75);
            _wSway.localPosition = Vector3.Lerp(_wSway.localPosition, _airMove, Time.deltaTime * 10f);
        }
        else if (_pMov.GroundCheck.IsGrounded && Vector3.Distance(_wSway.localPosition, _ogPosition) >= 0.005f)
        {
            _wSway.localPosition = Vector3.MoveTowards(_wSway.localPosition, _ogPosition, Time.deltaTime );
        }
    }
    void Land()
    {

    }
}