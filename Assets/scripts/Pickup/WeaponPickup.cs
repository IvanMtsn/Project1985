using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : Pickup
{
    [SerializeField] float _currentLoadedAmmo;
    [SerializeField] float _currentReserveAmmo;
    public float CurrentLoadedAmmo => _currentLoadedAmmo;
    public float CurrentReserveAmmo => _currentReserveAmmo;
    public void SetAmmo(float currentLoadedAmmo, float currentReserveAmmo)
    {
        _currentLoadedAmmo = currentLoadedAmmo;
        _currentReserveAmmo = currentReserveAmmo;
    }
    public void AddAmmo(float currentLoadedAmmo, float currentReserveAmmo)
    {
        _currentLoadedAmmo += currentLoadedAmmo;
        _currentReserveAmmo += currentReserveAmmo;
    }
    public void AddAmmo(float currentLoadedAmmo)
    {
        _currentLoadedAmmo += currentLoadedAmmo;
    }
    private void Update()
    {
        if(_currentLoadedAmmo <= 0 && _currentReserveAmmo <= 0)
        {
            UnityEngine.GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponHandler>().TriggerWeaponHoverExit();
            Destroy(gameObject);
        }
    }
}
