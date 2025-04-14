using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]TMP_Text _healthText;
    [SerializeField]TMP_Text _leftWeaponAmmo;
    [SerializeField]TMP_Text _rightWeaponAmmo;
    [SerializeField]TMP_Text _leftWeaponReload;
    [SerializeField]TMP_Text _rightWeaponReload;
    [SerializeField]TMP_Text _leftWeaponReserve;
    [SerializeField]TMP_Text _rightWeaponReserve;
    [SerializeField] Image _dashIndicator;
    [SerializeField] Image _healthIndicator;
    PlayerStats _playerStats;
    PlayerMovement _playerMovement;
    WeaponManager _weaponManager;
    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        _playerMovement = GetComponent<PlayerMovement>();
        _weaponManager = GetComponent<WeaponManager>();
        _healthIndicator.fillAmount = _playerStats.Health * 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayStats();
        DisplayAmmo(_weaponManager.LeftWeapon,_leftWeaponAmmo, _leftWeaponReload, _leftWeaponReserve);
        DisplayAmmo(_weaponManager.RightWeapon, _rightWeaponAmmo, _rightWeaponReload, _rightWeaponReserve);
    }
    void DisplayStats()
    {
        _healthText.text = _playerStats.Health.ToString();
        _healthIndicator.fillAmount = Mathf.Clamp(_playerStats.Health * 0.01f, 0, 1);
        _dashIndicator.fillAmount = Mathf.MoveTowards(_dashIndicator.fillAmount, (_playerMovement.LastTimeSinceDash / _playerMovement.DashCooldown), 6f * Time.deltaTime);
    }
    void DisplayAmmo(IWeapon weapon, TMP_Text ammoText, TMP_Text reloadText, TMP_Text reserveText)
    {
        if (weapon != null)
        {
            ammoText.text = $"{weapon.CurrentAmmo}/{weapon.MagSize}";
            reloadText.gameObject.SetActive(weapon.IsReloading);
            reserveText.text = weapon.MaxAmmo.ToString();
        }
        else
        {
            ammoText.text = "--/--";
            reserveText.text = "---";
        }
    }
}
