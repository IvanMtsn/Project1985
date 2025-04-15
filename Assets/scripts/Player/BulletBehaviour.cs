using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    float _projectileLifespan = 3;
    float _bulletDamage = 4f;
    Rigidbody _rb;
    void Start()
    {
        if(FindObjectsOfType<SimpleWeapon>().Length == 2)
        {
            _bulletDamage = 3f;
        }
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject,_projectileLifespan);
    }
    void FixedUpdate()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 7 || other.gameObject.layer == 8))
        {
            if (other.CompareTag("Enemy"))
            {
                IDamageableEntity entity = other.gameObject.GetComponent<IDamageableEntity>();
                entity.TakeDamage(_bulletDamage);
            }
            EndBulletLife();
        }
    }
    void EndBulletLife()
    {
        Destroy(gameObject);
    }
}
