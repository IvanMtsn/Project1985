using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ReloadableComponent : MonoBehaviour, IAmmo, IAmmoPickupReloadable
{
    [SerializeField] float _maxReserveAmmo;
    [SerializeField] float _currentReserveAmmo;
    [SerializeField] float _maxLoadedAmmo;
    [SerializeField] float _currentLoadedAmmo;
    [SerializeField] float _reloadTime;
    public bool IsReloading { get; protected set; } = false;
    public float CurrentLoadedAmmo { get { return _currentLoadedAmmo;  } set { _currentLoadedAmmo = value; } }
    public float MaxLoadedAmmo { get { return _maxLoadedAmmo; } set { _maxLoadedAmmo = value; } }
    public float CurrentReserveAmmo => _currentReserveAmmo;
    public float MaxReserveAmmo => _maxReserveAmmo;
    Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (_currentLoadedAmmo == 0 && _currentReserveAmmo > 0)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }
    public void PickupAmmo(float loadedAmmo, float reserveAmmo, WeaponPickup weaponPickup)
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

        weaponPickup.AddAmmo(-loadedAmmoToUse, -reserveAmmoToUse);
    }
    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }
    IEnumerator ReloadCoroutine()
    {
        if (_currentLoadedAmmo == _maxLoadedAmmo || _currentReserveAmmo == 0)
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
}
