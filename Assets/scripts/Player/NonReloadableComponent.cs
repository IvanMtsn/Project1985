using UnityEngine;

public class NonReloadableComponent : MonoBehaviour, IAmmo
{
    [SerializeField] float _maxLoadedAmmo;
    [SerializeField] float _currentLoadedAmmo;
    public float CurrentLoadedAmmo { get { return _currentLoadedAmmo; } set { _currentLoadedAmmo = value; } }
    public float MaxLoadedAmmo { get { return _maxLoadedAmmo; } set { _maxLoadedAmmo = value; } }

    public void PickupAmmo(float loadedAmmo, WeaponPickup weaponPickup)
    {
        loadedAmmo = Mathf.RoundToInt(loadedAmmo);

        float diffNeededForLoaded = _maxLoadedAmmo - _currentLoadedAmmo;
        float loadedAmmoToUse = Mathf.Min(diffNeededForLoaded, loadedAmmo);
        _currentLoadedAmmo += loadedAmmoToUse;

        weaponPickup.AddAmmo(-loadedAmmoToUse);
    }
    public void SetAmmo(float loadedAmmo)
    {
        _currentLoadedAmmo = loadedAmmo;
    }
}
