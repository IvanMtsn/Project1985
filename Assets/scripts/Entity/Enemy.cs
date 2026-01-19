using Assets.scripts.Entity;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageableEntity
{
    protected Rigidbody _rb;
    protected bool _isGrounded;
    protected bool _isStunned;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected EnemyClass _enemyClass;
    [SerializeField] protected float _stunDuration;
    [SerializeField] protected float _stunPushStrength;
    [SerializeField] protected float _stunDamageMultiplier = 1.25f;
    public EnemyClass EnemyClass => _enemyClass;
    [SerializeField] protected float _health; 
    public float Health => _health;
    [SerializeField] protected float _maxHealth;
    public float MaxHealth => _maxHealth;
    [SerializeField] protected float _damage;
    public float Damage => _damage;
    [SerializeField] protected float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void TakeDamage(float damage)
    {
        damage *= _isStunned ? _stunDamageMultiplier : 1;
        _health -= damage;
        //Debug.Log($"Damage,{_health}");
        if (_health <= 0) { Die(); }
    }
    protected virtual void Update()
    {
        if (_isStunned)
        {
            return;
        }
    }
    protected virtual void FixedUpdate()
    {
        if (_isStunned)
        {
            _rb.linearVelocity = Vector3.MoveTowards(_rb.linearVelocity, Vector3.zero, 0.5f);
            return;
        }
    }
    public void HeavyHit(Vector3 pushDirection = default, bool launchUp = false)
    {
        switch (_enemyClass)
        {
            case EnemyClass.light: GetComponent<IDamageableEntity>().Die();
                break;
            case EnemyClass.heavy: StartCoroutine(Stun(false,pushDirection,launchUp));
                break;
            case EnemyClass.superheavy: StartCoroutine(Stun(true,pushDirection));
                break;
        }
    }
    public virtual void Die()
    {
        Destroy(gameObject);
    }
    protected IEnumerator Stun(bool isSuperHeavy, Vector3 pushDirection, bool launchUp = false)
    {
        _isStunned = true;
        if (!isSuperHeavy)
        {
            Vector3 verticalPushDir = launchUp ? Vector3.up * 0.25f : Vector3.zero;
            Vector3 horizontalPushDir = pushDirection.normalized * 0.01f;
            Vector3 finalDir = horizontalPushDir + verticalPushDir;
            EffectsManager.Instance.SlowDownTime();
            _rb.linearVelocity = finalDir / Time.fixedDeltaTime;
        }
        yield return new WaitForSeconds(_stunDuration);
        _isStunned = false;
    }
}
