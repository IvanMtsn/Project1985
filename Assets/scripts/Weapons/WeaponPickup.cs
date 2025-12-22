using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField]float _currentLoadedAmmo;
    [SerializeField]float _currentMagAmmo;
    public float CurrentLoadedAmmo => _currentLoadedAmmo;
    public float CurrentMagAmmo => _currentMagAmmo;
    public GameObject WeaponPrefab;
    public void SetAmmo(float currentLoadedAmmo, float currentMagAmmo)
    {
        _currentLoadedAmmo = currentLoadedAmmo;
        _currentMagAmmo = currentMagAmmo;
    }
}
