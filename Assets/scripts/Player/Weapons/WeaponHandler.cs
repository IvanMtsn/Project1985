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
            if (InputManager.Instance.SwitchWeaponLeft) EquipWeapon(_weaponReadyToEquip, Weaponside.left);
            if (InputManager.Instance.SwitchWeaponRight) EquipWeapon(_weaponReadyToEquip, Weaponside.right);
        }
        Debug.Log(_isStandingAboveEquipableWeapon);
    }
    void EquipWeapon(GameObject weapon, Weaponside side)
    {
        //additional logic for dropping the weapon as an equipable weapon COMES LATER
        if (side == Weaponside.left)
        {
            if (LeftWeapon != null)
            {
                Destroy(LeftWeapon);
            }
            LeftWeapon = Instantiate(weapon, _leftWeaponHolder);
            LeftWeapon.GetComponent<BaseWeapon>().Weaponside = side;
        }
        else
        {
            if(RightWeapon != null)
            {
                Destroy(RightWeapon);
            }
            RightWeapon = Instantiate(weapon, _rightWeaponHolder);
            RightWeapon.GetComponent<BaseWeapon>().Weaponside = side;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponPickup"))
        {
            _isStandingAboveEquipableWeapon = true;
            _weaponReadyToEquip = other.GetComponent<WeaponPickup>().WeaponPrefab;
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
