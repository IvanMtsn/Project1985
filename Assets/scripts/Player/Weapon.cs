using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
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
    [Header("Ammo")]
    [SerializeField] protected float _maxReserveAmmo;
    [SerializeField] protected float _currentReserveAmmo;
    [SerializeField] protected float _maxLoadedAmmo;
    [SerializeField] protected float _currentLoadedAmmo;
    public float CurrentLoadedAmmo => _currentLoadedAmmo;
    public float MaxLoadedAmmo => _maxLoadedAmmo;
    public float CurrentReserveAmmo => _currentReserveAmmo;
    public float MaxReserveAmmo => _maxReserveAmmo;
    protected Animator _animator;
    [Header("orientation")]
    [SerializeField] protected Transform _visualFirePoint;
    [SerializeField] protected FireMode _fireMode;
    [SerializeField] protected bool _isDroppable;
    public bool IsDroppable => _isDroppable;
    public ItemSide WeaponsideOfWeapon;
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
        if (_currentLoadedAmmo == 0 && _currentReserveAmmo > 0)
        {
            StartCoroutine(Reload());
        }
        HandleGunControls();
    }
    protected void HandleGunControls()
    {
        bool firing = (WeaponsideOfWeapon == ItemSide.left) ? InputManager.Instance.FiringLeft : InputManager.Instance.FiringRight;
        bool firePressed = (WeaponsideOfWeapon == ItemSide.left) ? InputManager.Instance.FirePressedLeft : InputManager.Instance.FirePressedRight;
        bool reloadPressed = (WeaponsideOfWeapon == ItemSide.left) ? InputManager.Instance.ReloadLeft : InputManager.Instance.ReloadRight;
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
    public void PickupReload(float loadedAmmo, float reserveAmmo, WeaponPickup weaponPickup)
    {
        if (IsReloading) { return; }

        loadedAmmo = Mathf.RoundToInt(loadedAmmo);
        reserveAmmo = Mathf.RoundToInt(reserveAmmo);

        float diffNeededForLoaded = _maxLoadedAmmo - _currentLoadedAmmo;
        float loadedAmmoToUse = Mathf.Min(diffNeededForLoaded, loadedAmmo);
        _currentLoadedAmmo += loadedAmmoToUse;

        float diffNeededForReserve = _maxReserveAmmo - _currentReserveAmmo;
        float reserveAmmoToUse = Mathf.Min(diffNeededForReserve, reserveAmmo);
        _currentReserveAmmo += reserveAmmoToUse;

        weaponPickup.AddAmmo(-loadedAmmoToUse,-reserveAmmoToUse);
    }
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
        if (_currentLoadedAmmo == _maxLoadedAmmo  || _currentReserveAmmo == 0)
        {
            yield break;
        }
        IsReloading = true;
        _animator.ResetTrigger("shoot");
        _animator.SetTrigger("reload");
        
        yield return new WaitForSeconds(_reloadTime);

        float ammoNeeded = _maxLoadedAmmo - _currentLoadedAmmo;
        if (_currentReserveAmmo >= ammoNeeded)
        {
            _currentLoadedAmmo = _maxLoadedAmmo;
            _currentReserveAmmo -= ammoNeeded;
        }
        else
        {
            _currentLoadedAmmo += _currentReserveAmmo;
            _currentReserveAmmo = 0;
        }
        IsReloading = false;
    }
    public void SetAmmo(float currentLoadedAmmo, float currentMagAmmo)
    {
        _currentLoadedAmmo = currentLoadedAmmo;
        _currentReserveAmmo = currentMagAmmo;
    }
}
