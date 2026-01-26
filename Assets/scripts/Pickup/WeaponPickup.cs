using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] float _currentLoadedAmmo;
    [SerializeField] float _currentReserveAmmo;
    [SerializeField] Sprite _weaponIcon;
    public float CurrentLoadedAmmo => _currentLoadedAmmo;
    public float CurrentReserveAmmo => _currentReserveAmmo;
    public GameObject WeaponPrefab;
    public Sprite WeaponIcon => _weaponIcon;
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
    private void Update()
    {
        if(_currentLoadedAmmo <= 0 && _currentReserveAmmo <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponHandler>().TriggerWeaponHoverExit();
            Destroy(gameObject);
        }
    }
}
