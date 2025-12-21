using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TMP_Text _healthText;
    [SerializeField] TMP_Text _leftWeaponCurrentAmmo;
    [SerializeField] TMP_Text _rightWeaponCurrentAmmo;
    [SerializeField] TMP_Text _leftWeaponIsReloading;
    [SerializeField] TMP_Text _rightWeaponIsReloading;
    [SerializeField] TMP_Text _leftWeaponMagAmmo;
    [SerializeField] TMP_Text _rightWeaponMagAmmo;
    [SerializeField] Image _dashIndicator;
    [SerializeField] Image _healthIndicator;
    PlayerStats _playerStats;
    PlayerMovement _playerMovement;
    WeaponHandler _weaponHandler;
    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        _playerMovement = GetComponent<PlayerMovement>();
        _weaponHandler = GetComponent<WeaponHandler>();
        _healthIndicator.fillAmount = _playerStats.Health * 0.01f;
    }
    void Update()
    {
        DisplayStats();
        DisplayAmmo(_weaponHandler.LeftWeapon.GetComponent<BaseWeapon>(), _leftWeaponCurrentAmmo, _leftWeaponIsReloading, _leftWeaponMagAmmo);
        DisplayAmmo(_weaponHandler.RightWeapon.GetComponent<BaseWeapon>(), _rightWeaponCurrentAmmo, _rightWeaponIsReloading, _rightWeaponMagAmmo);
    }
    void DisplayStats()
    {
        _healthText.text = _playerStats.Health.ToString();
        _healthIndicator.fillAmount = Mathf.Clamp(_playerStats.Health * 0.01f, 0, 1);
        _dashIndicator.fillAmount = Mathf.MoveTowards(_dashIndicator.fillAmount, (_playerMovement.LastTimeSinceDash / _playerMovement.DashCooldown), 6f * Time.deltaTime);
    }
    void DisplayAmmo(BaseWeapon weapon, TMP_Text loadedAmmoText, TMP_Text isReloadingText, TMP_Text magAmmoText)
    {
        if (weapon != null)
        {
            loadedAmmoText.text = $"{weapon.CurrentLoadedAmmo}/{weapon.MaxLoadedAmmo}";
            isReloadingText.gameObject.SetActive(weapon.IsReloading);
            magAmmoText.text = weapon.CurrentMagAmmo.ToString();
        }
        else
        {
            loadedAmmoText.text = "--/--";
            magAmmoText.text = "---";
        }
    }
    void DisplayWeaponSwitchPrompt(bool leftWeapon, bool rightWeapon)
    {

    }
}
