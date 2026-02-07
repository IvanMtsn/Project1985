using UnityEngine;

public class ProjectileWeaponComponent : Gun
{
    [SerializeField] UnityEngine.GameObject _projectilePrefab;
    [SerializeField] float _projectileForce;
    [SerializeField] bool _hasBulletDropoff;
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

        var bullet = Instantiate(_projectilePrefab, _firePoint.transform.position, Quaternion.Euler(shotDirection));
        if (!_hasBulletDropoff) { bullet.GetComponent<Rigidbody>().useGravity = false; }
        bullet.GetComponent<Rigidbody>().AddForce(shotDirection * _projectileForce, ForceMode.Impulse);
        _animator.SetTrigger("shoot");

        _ammoComponent.CurrentLoadedAmmo -= _ammoCost;
        if (_ammoComponent.CurrentLoadedAmmo < 0) { _ammoComponent.CurrentLoadedAmmo = 0; }
        _lastTimeSinceFire = 0;
    }
}
