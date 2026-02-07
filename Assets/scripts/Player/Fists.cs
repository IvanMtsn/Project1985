using UnityEngine;

public class Fists : Weapon
{
    [SerializeField] LayerMask _whatCanIPunchMask;
    [SerializeField] float _maxHitDistance;
    string _hitAnimStateName;
    protected override void Start()
    {
        base.Start();
        transform.localScale *= WeaponsideOfWeapon == ItemSide.right ? -1 : 1;
        _hitAnimStateName = WeaponsideOfWeapon == ItemSide.right ? "Attack_R" : "Attack_L";
    }
    protected override void Shoot()
    {
        //Debug.DrawRay(_firePoint.position, transform.forward * _maxHitDistance, Color.red, 2);
        if (Physics.Raycast(_firePoint.position, transform.forward, out RaycastHit hit, _maxHitDistance, _whatCanIPunchMask))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.gameObject.GetComponent<Enemy>().HeavyHit(hit.transform.position - _firePoint.position);
                hit.collider.gameObject.GetComponent<IDamageableEntity>()?.TakeDamage(_damage);
            }
        }
        //_animator.Play(_hitAnimStateName, 0, 0);
        _lastTimeSinceFire = 0;
    }
}
