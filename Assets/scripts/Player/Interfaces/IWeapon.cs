using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    int CurrentAmmo { get; }
    int MaxAmmo { get; }
    int MagSize { get; }
    int MaxReserveSize { get; }
    string WeaponName { get; }
    bool IsLeftWeapon { get; }
    bool IsReloading { get; }
}
