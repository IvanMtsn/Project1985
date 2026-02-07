using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public UnityEngine.GameObject LeftWeapon;
    public UnityEngine.GameObject RightWeapon;
    [SerializeField] Transform _leftWeaponHolder;
    [SerializeField] Transform _rightWeaponHolder;
    [SerializeField] UnityEngine.GameObject _weaponPickupPrefab;
    [SerializeField] UnityEngine.GameObject _fistsPrefab;
    UnityEngine.GameObject _weaponReadyToEquip;
    bool _leftWeaponIsChangeable = false;
    bool _rightWeaponIsChangeable = false;
    public event Action<WeaponPickup, bool, bool> OnWeaponHoverEnter;
    public event Action OnWeaponHoverExit;
    void Start()
    {
        if(_leftWeaponHolder.childCount > 0)
        {
            LeftWeapon = _leftWeaponHolder.GetChild(0).gameObject;
            LeftWeapon.GetComponent<IItemSide>().Itemside = ItemSide.left;
        }
        if(_rightWeaponHolder.childCount > 0)
        {
            RightWeapon = _rightWeaponHolder.GetChild(0).gameObject;
            RightWeapon.GetComponent<IItemSide>().Itemside = ItemSide.right;
        }
    }
    void Update()
    {
        if (_leftWeaponIsChangeable)
        {
            if (InputManager.Instance.SwitchWeaponLeft && _weaponReadyToEquip != null)
            {
                var weaponPickup = _weaponReadyToEquip.GetComponent<WeaponPickup>();
                float wpLoadedAmmo = weaponPickup.CurrentLoadedAmmo;
                float wpReserveAmmo = weaponPickup.CurrentReserveAmmo;

                EquipWeapon(weaponPickup.WeaponPrefab, ItemSide.left);
                LeftWeapon.GetComponent<Weapon>().SetAmmo(wpLoadedAmmo, wpReserveAmmo);
                UnassignWeapon();
            }
        }
        if(_rightWeaponIsChangeable)
        {
            if (InputManager.Instance.SwitchWeaponRight && _weaponReadyToEquip != null)
            {
                var weaponPickup = _weaponReadyToEquip.GetComponent<WeaponPickup>();
                float wpLoadedAmmo = weaponPickup.CurrentLoadedAmmo;
                float wpReserveAmmo = weaponPickup.CurrentReserveAmmo;

                EquipWeapon(weaponPickup.WeaponPrefab, ItemSide.right);
                RightWeapon.GetComponent<Weapon>().SetAmmo(wpLoadedAmmo, wpReserveAmmo);
                UnassignWeapon();
            }
        }
        if (InputManager.Instance.DropWeaponLeft)
        {
            _weaponReadyToEquip = null;
            EquipWeapon(_fistsPrefab, ItemSide.left);
        }
        if (InputManager.Instance.DropWeaponRight && ! RightWeapon.name.Contains(_fistsPrefab.name))
        {
            _weaponReadyToEquip = null;
            EquipWeapon(_fistsPrefab, ItemSide.right);
        }
        //Debug.Log(LeftWeapon);
        //Debug.Log(_isStandingAboveEquipableWeapon);
    }
    void EquipWeapon(UnityEngine.GameObject weapon, ItemSide side)
    {
        UnityEngine.GameObject droppedWeapon;
        if (side == ItemSide.left)
        {
            if (LeftWeapon != null && LeftWeapon.GetComponent<Weapon>().IsDroppable)
            {
                droppedWeapon = 
                    Instantiate(LeftWeapon.GetComponent<Weapon>().WeaponPickupPrefab, LeftWeapon.transform.position, Quaternion.identity);

                droppedWeapon.GetComponent<WeaponPickup>()
                    .SetAmmo(LeftWeapon.GetComponent<Weapon>().CurrentLoadedAmmo, LeftWeapon.GetComponent<Weapon>().CurrentReserveAmmo);

            }
            if (LeftWeapon)
            {
                Destroy(LeftWeapon);
                LeftWeapon = null;
            }
            LeftWeapon = Instantiate(weapon, _leftWeaponHolder);
            LeftWeapon.GetComponent<IItemSide>().Itemside = side;
        }
        else
        {
            if(RightWeapon != null && RightWeapon.GetComponent<Weapon>().IsDroppable)
            {
                droppedWeapon =
                    Instantiate(RightWeapon.GetComponent<Weapon>().WeaponPickupPrefab, RightWeapon.transform.position, Quaternion.identity);

                droppedWeapon.GetComponent<WeaponPickup>()
                    .SetAmmo(RightWeapon.GetComponent<Weapon>().CurrentLoadedAmmo, RightWeapon.GetComponent<Weapon>().CurrentReserveAmmo);
            }
            if (RightWeapon)
            {
                Destroy(RightWeapon);
                RightWeapon = null;
            }
            RightWeapon = Instantiate(weapon, _rightWeaponHolder);
            RightWeapon.GetComponent<IItemSide>().Itemside = side;
        }
        if(_weaponReadyToEquip)
        {
            Destroy(_weaponReadyToEquip);
            _weaponReadyToEquip = null;
        }
    }
    void UnassignWeapon()
    {
        _rightWeaponIsChangeable = false;
        _leftWeaponIsChangeable = false;
        _weaponReadyToEquip = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("WeaponPickup")) { return; }

        OnWeaponHoverExit?.Invoke();

        _weaponReadyToEquip = other.gameObject;
        var pickup = _weaponReadyToEquip.GetComponent<WeaponPickup>();
        _leftWeaponIsChangeable = !LeftWeapon.name.Contains(pickup.WeaponPrefab.name);
        _rightWeaponIsChangeable = !RightWeapon.name.Contains(pickup.WeaponPrefab.name);
        OnWeaponHoverEnter?.Invoke(pickup, _leftWeaponIsChangeable, _rightWeaponIsChangeable);

        if(!_leftWeaponIsChangeable && !_rightWeaponIsChangeable)
        {
            bool isLeftWeaponFull = LeftWeapon.GetComponent<Weapon>().CurrentLoadedAmmo == LeftWeapon.GetComponent<Weapon>().MaxLoadedAmmo;
            bool isRightWeaponFull = RightWeapon.GetComponent<Weapon>().CurrentLoadedAmmo == RightWeapon.GetComponent<Weapon>().MaxLoadedAmmo;
            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
            if(pickup.CurrentLoadedAmmo == 1)
            {
                //Debug.Log("case 1");
                var weaponToRefill = LeftWeapon.GetComponent<Weapon>().CurrentLoadedAmmo < LeftWeapon.GetComponent<Weapon>().MaxLoadedAmmo
                    ? LeftWeapon : RightWeapon;
                weaponToRefill.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
            }
            if ((!isLeftWeaponFull && !isRightWeaponFull )|| ( isLeftWeaponFull && isRightWeaponFull))
            {
                //Debug.Log("case 2");

                float splitLoadedAmmo = pickup.CurrentLoadedAmmo / 2;
                float splitReserveAmmo = pickup.CurrentReserveAmmo / 2;
                LeftWeapon.GetComponent<Weapon>().PickupReload(splitLoadedAmmo, splitReserveAmmo, pickup);
                RightWeapon.GetComponent<Weapon>().PickupReload(splitLoadedAmmo, splitReserveAmmo, pickup);
                //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
            }
            else if(!isLeftWeaponFull)
            {
                //Debug.Log("case 3");

                LeftWeapon.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
                //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");

            }
            else if(!isRightWeaponFull)
            {
                //Debug.Log("case 4");

                RightWeapon.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
                //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");

            }
        }
        else if(!_leftWeaponIsChangeable)
        {
            //Debug.Log("case 5");

            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
            LeftWeapon.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
        }
        else if(!_rightWeaponIsChangeable)
        {
            //Debug.Log("case 6");

            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
            RightWeapon.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WeaponPickup"))
        {
            if (other.gameObject == _weaponReadyToEquip)
            {
                UnassignWeapon();
                OnWeaponHoverExit?.Invoke();
            }
        }
    }
    public void TriggerWeaponHoverExit()
    {
        OnWeaponHoverExit?.Invoke();
    }
}
