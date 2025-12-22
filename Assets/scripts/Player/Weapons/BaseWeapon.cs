using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseWeapon : MonoBehaviour
{
    protected float _lastTimeSinceFire;
    public bool IsReloading { get; protected set; } = false;
    protected bool _isBursting = false;
    protected bool _shotQueued = false;
    [SerializeField] GameObject _weaponPickupPrefab;
    public GameObject WeaponPickupPrefab => _weaponPickupPrefab;
    [Header("Reload Time and Shooting values")]
    [SerializeField] protected float _firingCooldown;
    [SerializeField] protected float _burstCooldown;
    [SerializeField] protected float _burstAmount;
    [SerializeField] protected float _reloadTime;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _ammoCost;
    [Header("Mag and Loaded Ammo")]
    [SerializeField] protected float _maxMagAmmo;
    [SerializeField] protected float _currentMagAmmo;
    [SerializeField] protected float _maxLoadedAmmo;
    [SerializeField] protected float _currentLoadedAmmo;
    public float CurrentLoadedAmmo => _currentLoadedAmmo;
    public float MaxLoadedAmmo => _maxLoadedAmmo;
    public float CurrentMagAmmo => _currentMagAmmo;
    public float MaxMagAmmo => _maxMagAmmo;
    protected Animator _animator;
    [Header("Firepoint and orientation")]
    [SerializeField] protected Transform _visualFirePoint;
    [SerializeField] protected FireMode _fireMode;
    [SerializeField] public Weaponside Weaponside;
    protected Transform _firePoint;


    protected virtual void Start()
    {
        _firePoint = GameObject.Find("ActualFirePoint").transform;
        _animator = GetComponent<Animator>();
    }
    protected void Update()
    {
        if (_lastTimeSinceFire < _firingCooldown)
        {
            _lastTimeSinceFire += Time.deltaTime;
        }
        if (IsReloading) { return; }
        if (_currentLoadedAmmo == 0 && _currentMagAmmo > 0)
        {
            StartCoroutine(Reload());
        }
        HandleGunControls();
    }
    protected void HandleGunControls()
    {
        bool firing = (Weaponside == Weaponside.left) ? InputManager.Instance.FiringLeft : InputManager.Instance.FiringRight;
        bool firePressed = (Weaponside == Weaponside.left) ? InputManager.Instance.FirePressedLeft : InputManager.Instance.FirePressedRight;
        bool reloadPressed = (Weaponside == Weaponside.left) ? InputManager.Instance.ReloadLeft : InputManager.Instance.ReloadRight;
        //Lord help me
        switch (_fireMode)
        {
            case FireMode.Fullauto:
                if (firing && _lastTimeSinceFire >= _firingCooldown) Shoot();
                break;
            case FireMode.Semiauto:
                if (firePressed)
                    /**NTS: because spamming the mousebutton is slower than rythmically clicking for SOME REASON?????????
                     Yes I do know the reason shut up*/
                    _shotQueued = true;

                if (_shotQueued && _lastTimeSinceFire >= _firingCooldown)
                {
                    Shoot();
                    _shotQueued = false;
                }
                break;
            case FireMode.Burst:
                if (firePressed && !_isBursting && _lastTimeSinceFire >= _firingCooldown) StartCoroutine(ShootBurst());
                break;
        }
        if (reloadPressed) StartCoroutine(Reload());
    }

    abstract protected void Shoot();
    protected IEnumerator ShootBurst()
    {
        _isBursting = true;
        for(int i = 0; i < _burstAmount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(_burstCooldown);
        }
        _isBursting= false;
    }
    protected virtual IEnumerator Reload()
    {
        if (_currentLoadedAmmo == _maxLoadedAmmo  || _currentMagAmmo == 0)
        {
            yield break;
        }
        IsReloading = true;
        _animator.ResetTrigger("shoot");
        _animator.SetTrigger("reload");
        
        yield return new WaitForSeconds(_reloadTime);

        float ammoNeeded = _maxLoadedAmmo - _currentLoadedAmmo;
        if (_currentMagAmmo >= ammoNeeded)
        {
            _currentLoadedAmmo = _maxLoadedAmmo;
            _currentMagAmmo -= ammoNeeded;
        }
        else
        {
            _currentLoadedAmmo += _currentMagAmmo;
            _currentMagAmmo = 0;
        }
        IsReloading = false;
    }
    public void SetAmmo(float currentLoadedAmmo, float currentMagAmmo)
    {
        _currentLoadedAmmo = currentLoadedAmmo;
        _currentMagAmmo = currentMagAmmo;
    }
}
