using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleSpecialWeapon : MonoBehaviour, ISpecialWeapon
{
    int _projectileSpeed = 120;
    float _firingCooldown = 2;
    float _lastTimeSinceFire = 2;
    bool _isFiring = false;
    [SerializeField] InputActionReference _fireSpecialRef;
    [SerializeField] UnityEngine.GameObject _firingPoint;
    [SerializeField] UnityEngine.GameObject _projectile;
    [SerializeField] Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if( _lastTimeSinceFire < _firingCooldown)
        {
            _lastTimeSinceFire += Time.deltaTime;
        }
        if((InputManager.Instance.ShootSpecial && _lastTimeSinceFire >= _firingCooldown) && !_isFiring)
        {
            _lastTimeSinceFire = 0;
            Fire();
        }
    }
    public void Fire()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        _isFiring = true;
        _animator.SetTrigger("shoot");
        yield return new WaitForSeconds(0.42f);
        UnityEngine.GameObject projectile = Instantiate(_projectile, _firingPoint.transform.position, _firingPoint.transform.rotation);
        //projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * _projectileSpeed;
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * _projectileSpeed, ForceMode.VelocityChange);
        _isFiring = false;
    }
}
