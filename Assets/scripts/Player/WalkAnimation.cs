using System.Collections;
using UnityEngine;

public class WalkBobLookSway : MonoBehaviour
{
    Animator _animator;
    PlayerGroundCheck _gc;
    PlayerMovementV2 _pMov;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _gc = GetComponentInParent<PlayerGroundCheck>();
        _pMov = GetComponentInParent<PlayerMovementV2>();
        _pMov = GetComponentInParent<PlayerMovementV2>();
    }
    void Update()
    {
        //Add isDashing check later!!!
        _animator.SetBool("moving", InputManager.Instance.Move.magnitude > Mathf.Epsilon && _gc.IsGrounded);
    }
}
