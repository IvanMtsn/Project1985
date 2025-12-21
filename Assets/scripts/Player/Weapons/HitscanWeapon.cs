using System.Collections;
using UnityEngine;

public abstract class HitscanWeapon : BaseWeapon
{
    [SerializeField] protected float _maxShotDistance;
    [SerializeField] protected float _spreadAngle;
    [SerializeField] protected LayerMask _hitMask;
    protected override void Shoot()
    {
        if (_currentLoadedAmmo == 0)
        {
            //Debug.Log("No ammo for " + _weaponName);
            return;
        }

        Vector3 shotDirection = _visualFirePoint.forward +
            new Vector3(
                Random.Range(-_spreadAngle, _spreadAngle),
                Random.Range(-_spreadAngle, _spreadAngle),
                0
            );

        if (Physics.Raycast(_firePoint.position, shotDirection, out RaycastHit hit, _maxShotDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<IDamageableEntity>().TakeDamage(_damage);
            }
        }
        TrailPool.Instance.StartCoroutine(TrailPool.Instance.ShootTrail(_visualFirePoint.position, hit.point, 0.3f));
        _animator.SetTrigger("shoot");

        _currentLoadedAmmo -= _ammoCost;
        if(_currentLoadedAmmo < 0) { _currentLoadedAmmo = 0; }
        _lastTimeSinceFire = 0;
    }
}
