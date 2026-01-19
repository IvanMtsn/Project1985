using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBulletBehaviour : MonoBehaviour
{
    int _bulletDamage = 10;
    Rigidbody _rb;
    [SerializeField] LayerMask _projectileMask;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (CollisionUtils.IsLayerInMask(other.gameObject.layer, _projectileMask))
        {
            if (other.CompareTag("Enemy"))
            {
                IDamageableEntity enemy = other.gameObject.GetComponent<IDamageableEntity>();
                other.gameObject.GetComponent<Enemy>().HeavyHit(_rb.linearVelocity.normalized);
                enemy.TakeDamage(_bulletDamage);
            }
            EndBulletLife();
        }
    }
    void EndBulletLife()
    {
        Destroy(gameObject);
    }
}
