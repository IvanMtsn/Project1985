using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public UnityEngine.GameObject LeftItem;
    public UnityEngine.GameObject RightItem;
    [SerializeField] Transform _leftItemHolder;
    [SerializeField] Transform _rightItemHolder;
    [SerializeField] UnityEngine.GameObject _fistsPrefab;
    UnityEngine.GameObject _itemReadyToEquip;
    bool _leftItemIsChangeable = false;
    bool _rightItemIsChangeable = false;
    public event Action<WeaponPickup, bool, bool> OnItemHoverEnter;
    public event Action OnItemHoverExit;
    void Start()
    {
        if(_leftItemHolder.childCount > 0)
        {
            LeftItem = _leftItemHolder.GetChild(0).gameObject;
            LeftItem.GetComponent<IItem>().Itemside = ItemSide.left;
        }
        if(_rightItemHolder.childCount > 0)
        {
            RightItem = _rightItemHolder.GetChild(0).gameObject;
            RightItem.GetComponent<IItem>().Itemside = ItemSide.right;
        }
    }
    void Update()
    {
        if (_leftItemIsChangeable)
        {
            if (InputManager.Instance.SwitchWeaponLeft && _itemReadyToEquip != null)
            {
                var itemPickup = _itemReadyToEquip;
                if(_itemReadyToEquip.GetComponent<IAmmo>() != null)
                {
                    float wpLoadedAmmo = itemPickup.GetComponent<IAmmo>().CurrentLoadedAmmo;
                    if (_itemReadyToEquip.GetComponent<ReloadableComponent>() != null)
                    {
                        float wpReserveAmmo = itemPickup.GetComponent<ReloadableComponent>().CurrentReserveAmmo;
                        //itemPickup.GetComponent<IAmmoPickupReloadable>().SetAmmo(wpLoadedAmmo, wpReserveAmmo);
                    }
                    else
                    {
                        //itemPickup.GetComponent<IAmmoPickupNonReloadable>().SetAmmo(wpLoadedAmmo);
                    }
                }

                //EquipWeapon(itemPickup.WeaponPrefab, ItemSide.left);
                //LeftItem.GetComponent<Weapon>().SetAmmo(wpLoadedAmmo, wpReserveAmmo);
                UnassignWeapon();
            }
        }
        if(_rightItemIsChangeable)
        {
            if (InputManager.Instance.SwitchWeaponRight && _itemReadyToEquip != null)
            {
                var weaponPickup = _itemReadyToEquip.GetComponent<WeaponPickup>();
                float wpLoadedAmmo = weaponPickup.CurrentLoadedAmmo;
                float wpReserveAmmo = weaponPickup.CurrentReserveAmmo;

                //EquipWeapon(weaponPickup.WeaponPrefab, ItemSide.right);
                RightItem.GetComponent<Weapon>().SetAmmo(wpLoadedAmmo, wpReserveAmmo);
                UnassignWeapon();
            }
        }
        if (InputManager.Instance.DropWeaponLeft)
        {
            _itemReadyToEquip = null;
            EquipWeapon(_fistsPrefab, ItemSide.left);
        }
        if (InputManager.Instance.DropWeaponRight && ! RightItem.name.Contains(_fistsPrefab.name))
        {
            _itemReadyToEquip = null;
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
            if (LeftItem != null && LeftItem.GetComponent<Weapon>().IsDroppable)
            {
                droppedWeapon = 
                    Instantiate(LeftItem.GetComponent<Weapon>().WeaponPickupPrefab, LeftItem.transform.position, Quaternion.identity);

                droppedWeapon.GetComponent<WeaponPickup>()
                    .SetAmmo(LeftItem.GetComponent<Weapon>().CurrentLoadedAmmo, LeftItem.GetComponent<Weapon>().CurrentReserveAmmo);

            }
            if (LeftItem)
            {
                Destroy(LeftItem);
                LeftItem = null;
            }
            LeftItem = Instantiate(weapon, _leftItemHolder);
            LeftItem.GetComponent<IItem>().Itemside = side;
        }
        else
        {
            if(RightItem != null && RightItem.GetComponent<Weapon>().IsDroppable)
            {
                droppedWeapon =
                    Instantiate(RightItem.GetComponent<Weapon>().WeaponPickupPrefab, RightItem.transform.position, Quaternion.identity);

                droppedWeapon.GetComponent<WeaponPickup>()
                    .SetAmmo(RightItem.GetComponent<Weapon>().CurrentLoadedAmmo, RightItem.GetComponent<Weapon>().CurrentReserveAmmo);
            }
            if (RightItem)
            {
                Destroy(RightItem);
                RightItem = null;
            }
            RightItem = Instantiate(weapon, _rightItemHolder);
            RightItem.GetComponent<IItem>().Itemside = side;
        }
        if(_itemReadyToEquip)
        {
            Destroy(_itemReadyToEquip);
            _itemReadyToEquip = null;
        }
    }
    void UnassignWeapon()
    {
        _rightItemIsChangeable = false;
        _leftItemIsChangeable = false;
        _itemReadyToEquip = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("WeaponPickup")) { return; }

        OnItemHoverExit?.Invoke();

        _itemReadyToEquip = other.gameObject;
        var pickup = _itemReadyToEquip.GetComponent<WeaponPickup>();
        //_leftItemIsChangeable = !LeftItem.name.Contains(pickup.WeaponPrefab.name);
        //_rightItemIsChangeable = !RightItem.name.Contains(pickup.WeaponPrefab.name);
        OnItemHoverEnter?.Invoke(pickup, _leftItemIsChangeable, _rightItemIsChangeable);

        if(!_leftItemIsChangeable && !_rightItemIsChangeable)
        {
            bool isLeftWeaponFull = LeftItem.GetComponent<Weapon>().CurrentLoadedAmmo == LeftItem.GetComponent<Weapon>().MaxLoadedAmmo;
            bool isRightWeaponFull = RightItem.GetComponent<Weapon>().CurrentLoadedAmmo == RightItem.GetComponent<Weapon>().MaxLoadedAmmo;
            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
            if(pickup.CurrentLoadedAmmo == 1)
            {
                //Debug.Log("case 1");
                var weaponToRefill = LeftItem.GetComponent<Weapon>().CurrentLoadedAmmo < LeftItem.GetComponent<Weapon>().MaxLoadedAmmo
                    ? LeftItem : RightItem;
                weaponToRefill.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
            }
            if ((!isLeftWeaponFull && !isRightWeaponFull )|| ( isLeftWeaponFull && isRightWeaponFull))
            {
                //Debug.Log("case 2");

                float splitLoadedAmmo = pickup.CurrentLoadedAmmo / 2;
                float splitReserveAmmo = pickup.CurrentReserveAmmo / 2;
                LeftItem.GetComponent<Weapon>().PickupReload(splitLoadedAmmo, splitReserveAmmo, pickup);
                RightItem.GetComponent<Weapon>().PickupReload(splitLoadedAmmo, splitReserveAmmo, pickup);
                //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
            }
            else if(!isLeftWeaponFull)
            {
                //Debug.Log("case 3");

                LeftItem.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
                //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");

            }
            else if(!isRightWeaponFull)
            {
                //Debug.Log("case 4");

                RightItem.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
                //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");

            }
        }
        else if(!_leftItemIsChangeable)
        {
            //Debug.Log("case 5");

            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
            LeftItem.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
        }
        else if(!_rightItemIsChangeable)
        {
            //Debug.Log("case 6");

            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
            RightItem.GetComponent<Weapon>().PickupReload(pickup.CurrentLoadedAmmo, pickup.CurrentReserveAmmo, pickup);
            //Debug.Log($"loaded {pickup.CurrentLoadedAmmo} reserve {pickup.CurrentReserveAmmo}");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WeaponPickup"))
        {
            if (other.gameObject == _itemReadyToEquip)
            {
                UnassignWeapon();
                OnItemHoverExit?.Invoke();
            }
        }
    }
    public void TriggerWeaponHoverExit()
    {
        OnItemHoverExit?.Invoke();
    }
}
