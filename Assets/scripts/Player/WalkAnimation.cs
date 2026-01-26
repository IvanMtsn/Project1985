using System.Collections;
using UnityEngine;

public class WalkBobLookSway : MonoBehaviour
{
    Animator _animator;
    PlayerMovement _player;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = FindAnyObjectByType<PlayerMovement>();
    }
    void Update()
    {
        _animator.SetBool("moving", InputManager.Instance.Move.magnitude > Mathf.Epsilon && _player.IsGrounded && !_player.IsDashing);
    }
}
