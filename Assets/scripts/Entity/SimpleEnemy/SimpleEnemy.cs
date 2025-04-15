using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour, IDamageableEntity
{
    Rigidbody _rb;
    Transform _playerPos;
    float _moveSpeed;
    float _minSpeed = 8.5f; //8.5
    float _maxSpeed = 13f; //13
    float _rageSpeed = 16f;
    float _pushStrength = 10;
    float _timeUntilJump = 1;
    float _lastJumpTime = 0;
    float _jumpPower = 20;
    float _dashPower = 10;
    int _damage = 17;
    bool _isHealing = false;
    Vector3 _playerDirection;
    [SerializeField]bool _canMove = true;
    [SerializeField] EnemyStatsSO _stats;
    [HideInInspector] public float Health { get; protected set; }
    void Start()
    {
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        Health = _stats.MaxHealth;
        _moveSpeed = _minSpeed;
    }
    private void Update()
    {
        if(_moveSpeed < _maxSpeed)
        {
            _moveSpeed += Time.deltaTime;
        }
        //Debug.Log(Health);
    }
    void FixedUpdate()
    {
        if (!_canMove) { return; }

        _playerDirection = (_playerPos.position - transform.position).normalized;
        _playerDirection.y = 0;
        Vector3 playerPosition = new Vector3(_playerPos.position.x, 0, _playerPos.position.z);

        // 1/3 health - rage mode
        if (Health <= _stats.MaxHealth / 2 && Health > _stats.MaxHealth / 4 && !_isHealing)
        {
            _rb.AddForce(_playerDirection * _rageSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

            _rb.MoveRotation(Quaternion.LookRotation(_playerDirection));
            return;
        }
        // 1/4 health - retreat
        //if (Health <= _stats.MaxHealth / 4 || _isHealing)
        //{
        //    Vector3 retreatDir = (transform.position - playerPosition).normalized;
        //    retreatDir.y = 0;
        //    _rb.AddForce(retreatDir * (_maxSpeed+2) * Time.fixedDeltaTime, ForceMode.VelocityChange);

        //    _rb.MoveRotation(Quaternion.LookRotation(-lookAt));
        //    if (!_isHealing)
        //    {
        //        StartCoroutine(Heal());
        //    }
        //    return;
        //}
        _rb.AddForce(_playerDirection * _moveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        _rb.MoveRotation(Quaternion.LookRotation(_playerDirection));

        WaitForJump();
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log($"Damage,{Health}");
        if (Health <= 0) { Die(); }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(_damage);
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            pushDirection.y = 0;
            _rb.AddForce(pushDirection * _pushStrength, ForceMode.Impulse);
            _moveSpeed = _minSpeed;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(_rb.velocity.magnitude >= 3)
            {
                Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
                pushDirection.y = 0;
                _rb.AddForce(pushDirection * _pushStrength / 2, ForceMode.Impulse);
            }
        }
    }
    void WaitForJump()
    {
        if(_playerPos.position.y > transform.position.y && _rb.velocity.magnitude <= 2f)
        {
            if(_lastJumpTime < _timeUntilJump)
            {
                _lastJumpTime += Time.deltaTime;
            }
            else { StartCoroutine(JumpToPlayer()); } 
        }
        else
        {
            _lastJumpTime = 0;
        }
    }
    IEnumerator JumpToPlayer()
    {
        _rb.AddForce(-transform.forward.normalized * 7, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.15f);
        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.35f);
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.AddForce(transform.forward.normalized * _dashPower, ForceMode.VelocityChange);
    }
}
