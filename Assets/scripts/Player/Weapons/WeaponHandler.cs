using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public GameObject LeftWeapon;
    public GameObject RightWeapon;
    [SerializeField] Transform _leftWeaponHolder;
    [SerializeField] Transform _rightWeaponHolder;
    [SerializeField] GameObject _weaponPickupPrefab;
    GameObject _weaponReadyToEquip;
    bool _isStandingAboveEquipableWeapon = false;
    void Start()
    {
        if(_leftWeaponHolder.childCount > 0)
        {
            LeftWeapon = _leftWeaponHolder.GetChild(0).gameObject;
            LeftWeapon.GetComponent<BaseWeapon>().Weaponside = Weaponside.left;
        }
        if(_rightWeaponHolder.childCount > 0)
        {
            RightWeapon = _rightWeaponHolder.GetChild(0).gameObject;
            RightWeapon.GetComponent<BaseWeapon>().Weaponside = Weaponside.right;
        }
    }
    void Update()
    {
        if (_isStandingAboveEquipableWeapon)
        {
            if (InputManager.Instance.SwitchWeaponLeft && _weaponReadyToEquip != null)
            {
                EquipWeapon(_weaponReadyToEquip.GetComponent<WeaponPickup>().WeaponPrefab, Weaponside.left);
                LeftWeapon.GetComponent<BaseWeapon>()
                    .SetAmmo(_weaponReadyToEquip.GetComponent<WeaponPickup>().CurrentLoadedAmmo, _weaponReadyToEquip.GetComponent<WeaponPickup>().CurrentMagAmmo);
            }
            else if (InputManager.Instance.SwitchWeaponRight && _weaponReadyToEquip != null)
            {
                EquipWeapon(_weaponReadyToEquip.GetComponent<WeaponPickup>().WeaponPrefab, Weaponside.right);
                RightWeapon.GetComponent<BaseWeapon>()
                    .SetAmmo(_weaponReadyToEquip.GetComponent<WeaponPickup>().CurrentLoadedAmmo, _weaponReadyToEquip.GetComponent<WeaponPickup>().CurrentMagAmmo);
            }
        }
        //Debug.Log(_isStandingAboveEquipableWeapon);
    }
    void EquipWeapon(GameObject weapon, Weaponside side)
    {
        GameObject droppedWeapon;
        if (side == Weaponside.left)
        {
            if (LeftWeapon != null)
            {
                droppedWeapon = 
                    Instantiate(LeftWeapon.GetComponent<BaseWeapon>().WeaponPickupPrefab, LeftWeapon.transform.position, Quaternion.identity);

                droppedWeapon.GetComponent<WeaponPickup>()
                    .SetAmmo(LeftWeapon.GetComponent<BaseWeapon>().CurrentLoadedAmmo, LeftWeapon.GetComponent<BaseWeapon>().CurrentMagAmmo);

                Destroy(LeftWeapon);
            }
            LeftWeapon = Instantiate(weapon, _leftWeaponHolder);
            LeftWeapon.GetComponent<BaseWeapon>().Weaponside = side;
            Destroy(_weaponReadyToEquip);
        }
        else
        {
            if(RightWeapon != null)
            {
                droppedWeapon =
                    Instantiate(RightWeapon.GetComponent<BaseWeapon>().WeaponPickupPrefab, RightWeapon.transform.position, Quaternion.identity);

                droppedWeapon.GetComponent<WeaponPickup>()
                    .SetAmmo(RightWeapon.GetComponent<BaseWeapon>().CurrentLoadedAmmo, RightWeapon.GetComponent<BaseWeapon>().CurrentMagAmmo);

                Destroy(RightWeapon);
            }
            RightWeapon = Instantiate(weapon, _rightWeaponHolder);
            RightWeapon.GetComponent<BaseWeapon>().Weaponside = side;
            Destroy(_weaponReadyToEquip);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponPickup"))
        {
            _isStandingAboveEquipableWeapon = true;
            _weaponReadyToEquip = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WeaponPickup"))
        {
            _isStandingAboveEquipableWeapon = false;
            _weaponReadyToEquip = null;
        }
    }
}
