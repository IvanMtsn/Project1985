using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleWeapon : MonoBehaviour, IWeapon
{
    float _projectileSpeed = 100;
    float _firingCooldown = 0.1f;
    float _lastTimeSinceFire;
    float _playerPosX;
    float _bulletNumber = 0;
    bool _isLeftWeapon;
    bool _isReloading = false;
    int _currentAmmo;
    int _maxReserveSize = 300;
    int _currentReserveSize;
    int _magSize = 25;
    string _weaponName;
    [SerializeField] GameObject _projectile;
    [SerializeField] GameObject _firingPoint;
    Animator _animator;
    public int CurrentAmmo => _currentAmmo;
    public int MaxAmmo => _currentReserveSize;
    public int MagSize => _magSize;
    public int MaxReserveSize => _maxReserveSize;
    public string WeaponName => _weaponName;
    public bool IsLeftWeapon => _isLeftWeapon;
    public bool IsReloading => _isReloading;

    void Start()
    {
        _playerPosX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
        if(transform.position.x < _playerPosX)
        {
            _isLeftWeapon = true;
        }
        else
        {
            _isLeftWeapon=false;
        }
        _weaponName = "SimpleWeapon" + (_isLeftWeapon ? "L" : "R");
        _currentReserveSize = _maxReserveSize;
        _currentAmmo = _magSize;
        _lastTimeSinceFire = _firingCooldown;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(_lastTimeSinceFire < _firingCooldown)
        {
            _lastTimeSinceFire += Time.deltaTime;
        }
        if (_isReloading) { return; }
        if(_currentAmmo == 0 && _currentReserveSize > 0)
        {
            StartCoroutine(Reload());
        }


        if(InputManager.Instance.ShootingLeft && _isLeftWeapon && _lastTimeSinceFire >= _firingCooldown)
        {
            Fire();
        }
        if (InputManager.Instance.ShootingRight && !_isLeftWeapon && _lastTimeSinceFire >= _firingCooldown)
        {
            Fire();
        }
        if (InputManager.Instance.ReloadLeft && _isLeftWeapon && _currentAmmo < _magSize && _currentReserveSize > 0)
        {
            StartCoroutine(Reload());
        }
        if (InputManager.Instance.ReloadRight && !_isLeftWeapon && _currentAmmo < _magSize && _currentReserveSize > 0)
        {
            StartCoroutine(Reload());
        }
    }
    void Fire()
    {
        if (_currentAmmo == 0)
        {
            //Debug.Log("No ammo for " + _weaponName);
            return;
        }
        _animator.SetTrigger("shoot");

        Quaternion rotationOfBullet = _firingPoint.transform.rotation * Quaternion.Euler(0, GetBulletRotationY(), 0);

        float offsetX = ((_bulletNumber + 1) % 2 == 0) ? -0.1f : 0.2f;
        Vector3 firePointPos = new Vector3(_firingPoint.transform.position.x + offsetX, _firingPoint.transform.position.y, _firingPoint.transform.position.z);

        GameObject projectile = Instantiate(_projectile, firePointPos, rotationOfBullet);
        //projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * _projectileSpeed;
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * _projectileSpeed, ForceMode.VelocityChange);
        Debug.Log("Projectile Fired");

        _bulletNumber++;
        _currentAmmo--;
        _lastTimeSinceFire = 0;
    }
    float GetBulletRotationY()
    {
        float temp;
        if (_isLeftWeapon)
        {
            temp = Random.Range(-2, 8);
        }
        else
        {
            temp = Random.Range(-8, 2);
        }
        return temp;
    }
    IEnumerator Reload()
    {
        _isReloading = true;
        _animator.SetTrigger("reload");
        yield return new WaitForSeconds(1.5f);
        int ammoNeeded = _magSize - _currentAmmo;
        if(_currentReserveSize >= ammoNeeded)
        {
            _currentAmmo = _magSize;
            _currentReserveSize -= ammoNeeded;
        }
        else
        {
            _currentAmmo += _currentReserveSize;
            _currentReserveSize = 0;
        }
        _isReloading = false;
    }
}
