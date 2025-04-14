using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBulletBehaviour : MonoBehaviour
{
    int _bulletDamage = 10;
    float _pushStrength = 40f;
    Rigidbody _rb;
    [SerializeField] LayerMask _projectileMask;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 7 || other.gameObject.layer == 8))
        {
            if (other.CompareTag("Enemy"))
            {
                IDamageableEntity entity = other.gameObject.GetComponent<IDamageableEntity>();

                Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(_rb.velocity.normalized * _pushStrength, ForceMode.Impulse);

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
