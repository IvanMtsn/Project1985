//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class SimpleWeapon : MonoBehaviour, 
//{
//    float _firingCooldown = 0.1f;
//    float _lastTimeSinceFire;
//    float _playerPosX;
//    bool _isLeftWeapon;
//    bool _isReloading = false;
//    int _currentAmmo;
//    int _maxReserveSize = 300;
//    int _currentReserveSize;
//    int _magSize = 25;
//    int _damage = 1;
//    string _weaponName;
//    [SerializeField] Transform _firingPoint;
//    [SerializeField] LayerMask _hitMask;
//    Animator _animator;
//    public int CurrentAmmo => _currentAmmo;
//    public int MaxAmmo => _currentReserveSize;
//    public int MagSize => _magSize;
//    public int MaxReserveSize => _maxReserveSize;
//    public string WeaponName => _weaponName;
//    public bool IsLeftWeapon => _isLeftWeapon;
//    public bool IsReloading => _isReloading;

//    void Start()
//    {
//        _playerPosX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
//        if(transform.position.x < _playerPosX)
//        {
//            _isLeftWeapon = true;
//        }
//        else
//        {
//            _isLeftWeapon=false;
//        }
//        _weaponName = "SimpleWeapon" + (_isLeftWeapon ? "L" : "R");
//        _currentReserveSize = _maxReserveSize;
//        _currentAmmo = _magSize;
//        _lastTimeSinceFire = _firingCooldown;
//        _animator = GetComponent<Animator>();
//    }

//    void Update()
//    {
//        if(_lastTimeSinceFire < _firingCooldown)
//        {
//            _lastTimeSinceFire += Time.deltaTime;
//        }
//        if (_isReloading) { return; }
//        if(_currentAmmo == 0 && _currentReserveSize > 0)
//        {
//            StartCoroutine(Reload());
//        }


//        if(InputManager.Instance.FiringLeft && _isLeftWeapon && _lastTimeSinceFire >= _firingCooldown)
//        {
//            Fire();
//        }
//        if (InputManager.Instance.FiringRight && !_isLeftWeapon && _lastTimeSinceFire >= _firingCooldown)
//        {
//            Fire();
//        }
//        if (InputManager.Instance.ReloadLeft && _isLeftWeapon && _currentAmmo < _magSize && _currentReserveSize > 0)
//        {
//            StartCoroutine(Reload());
//        }
//        if (InputManager.Instance.ReloadRight && !_isLeftWeapon && _currentAmmo < _magSize && _currentReserveSize > 0)
//        {
//            StartCoroutine(Reload());
//        }
//    }
//    void Fire()
//    {
//        if (_currentAmmo == 0)
//        {
//            //Debug.Log("No ammo for " + _weaponName);
//            return;
//        }
//        _animator.SetTrigger("shoot");

//        if(Physics.Raycast(_firingPoint.position, _firingPoint.forward, out RaycastHit hit, 100, _hitMask))
//        {
//            if (hit.collider.CompareTag("Enemy"))
//            {
//                hit.collider.GetComponent<IDamageableEntity>().TakeDamage(_damage);
//            }
//        }
//        StartCoroutine(ShootTrail(_firingPoint.position, hit.point));

//        _currentAmmo--;
//        _lastTimeSinceFire = 0;
//    }
//    IEnumerator ShootTrail(Vector3 start, Vector3 end)
//    {
//        LineRenderer line = TrailPool.Instance.Get();
//        line.SetPosition(0, start);
//        line.SetPosition(1, end);

//        float t = 0;
//        while (t<0.3f)
//        {
//            t += Time.deltaTime;
//            line.widthMultiplier = 1 - (t/0.2f);
//            yield return null;
//        }
//        TrailPool.Instance.Return(line);
//    }
//    IEnumerator Reload()
//    {
//        _isReloading = true;
//        _animator.SetTrigger("reload");
//        yield return new WaitForSeconds(1.5f);
//        int ammoNeeded = _magSize - _currentAmmo;
//        if(_currentReserveSize >= ammoNeeded)
//        {
//            _currentAmmo = _magSize;
//            _currentReserveSize -= ammoNeeded;
//        }
//        else
//        {
//            _currentAmmo += _currentReserveSize;
//            _currentReserveSize = 0;
//        }
//        _isReloading = false;
//    }
//}
