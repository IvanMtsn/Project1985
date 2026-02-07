using Assets.scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy
{
    Transform _playerPos;
    Vector3 _playerDirection;
    [SerializeField] float _pushStrength = 20;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _velocityRevertingSpeed;
    Vector3 _knockbackVelocity = Vector3.zero;

    protected override void Start()
    {
        base.Start();
        _playerPos = UnityEngine.GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _playerDirection = (_playerPos.position - transform.position).normalized;
        _playerDirection.y = 0;
        Vector3 playerPosition = new Vector3(_playerPos.position.x, 0, _playerPos.position.z);
        if (Physics.CheckSphere(transform.position - Vector3.up * 0.9f, 0.45f, _groundLayer))
        {
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(_playerDirection), _rotationSpeed ));
        }
        Vector3 flatVelocity = transform.forward * _moveSpeed;
        flatVelocity.y = _rb.linearVelocity.y;

        _knockbackVelocity = Vector3.MoveTowards(_knockbackVelocity, Vector3.zero, 0.2f);

        _rb.linearVelocity = flatVelocity + _knockbackVelocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageableEntity>().TakeDamage(_damage);
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            pushDirection.y = 0;
            _knockbackVelocity = pushDirection * _pushStrength;
        }
    }
}
