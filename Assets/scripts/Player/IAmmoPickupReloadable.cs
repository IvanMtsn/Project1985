using UnityEngine;

public interface IAmmoPickupReloadable
{
    public void PickupAmmo(float loadedAmmo, float reserveAmmo, WeaponPickup weaponPickup);
}
