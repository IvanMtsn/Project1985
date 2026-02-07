using System.Collections;
using UnityEngine;

public class HitScanWeaponComponent : Gun
{
    [SerializeField] float _maxShotDistance;
    [SerializeField] LayerMask _hitMask;
    TrailPool _trailPool;
    protected override void Shoot()
    {
        if (_ammoComponent.CurrentLoadedAmmo == 0)
        {
            //Debug.Log("No ammo for " + _weaponName);
            return;
        }

        Vector3 shotDirection = _visualFirePoint.forward + new Vector3(
            Random.Range(-_spreadAngle, _spreadAngle),
            Random.Range(-_spreadAngle, _spreadAngle),
            0);

        if (Physics.Raycast(_firePoint.position, shotDirection, out RaycastHit hit, _maxShotDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<IDamageableEntity>().TakeDamage(Damage);
            }
        }
        if (_trailPool == null)
        {
            _trailPool = FindFirstObjectByType<TrailPool>();
        }
        _trailPool.ShootTrail(_visualFirePoint.position, hit.point, 0.3f);
        _animator.SetTrigger("shoot");

        _ammoComponent.CurrentLoadedAmmo -= _ammoCost;
        if (_ammoComponent.CurrentLoadedAmmo < 0) { _ammoComponent.CurrentLoadedAmmo = 0; }
        _lastTimeSinceFire = 0;
    }
}
