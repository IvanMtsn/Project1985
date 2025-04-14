using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public IWeapon LeftWeapon { get; private set; }
    public IWeapon RightWeapon { get; private set; }
    bool _areWeaponsAssigned = false;
    void Start()
    {
    }
    void Update()
    {
        if (!_areWeaponsAssigned)
        {
            AssignWeaponry();
            return;
        }
    }
    void AssignWeaponry()
    {
        IWeapon[] weapons = GetComponentsInChildren<IWeapon>();
        if(weapons.Length == 0)
        {
            return;
        }

        foreach (IWeapon weapon in weapons)
        {
            if (weapon.IsLeftWeapon && LeftWeapon == null) { LeftWeapon = weapon; }
            else if (!weapon.IsLeftWeapon && RightWeapon == null) { RightWeapon = weapon; }
        }
        if(LeftWeapon != null && RightWeapon != null)
        {
            _areWeaponsAssigned = true;
        }
    }
}
