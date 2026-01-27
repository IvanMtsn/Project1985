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
    [SerializeField] TMP_Text _leftWeaponReserveAmmo;
    [SerializeField] TMP_Text _rightWeaponReserveAmmo;
    [SerializeField] Image _dashIndicator;
    [SerializeField] Image _healthIndicator;
    [SerializeField] GameObject SwitchPromptL;
    [SerializeField] GameObject SwitchPromptR;
    PlayerStats _playerStats;
    PlayerMovementV2 _pMov;
    WeaponHandler _weaponHandler;
    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        _pMov = GetComponent<PlayerMovementV2>();
        _weaponHandler = GetComponent<WeaponHandler>();
        _healthIndicator.fillAmount = _playerStats.Health * 0.01f;
        _weaponHandler.OnWeaponHoverEnter += DisplayWeaponSwitchPrompt;
        _weaponHandler.OnWeaponHoverExit += HideWeaponSwitchPrompt;
    }
    void Update()
    {
        DisplayStats();
        DisplayAmmo(_weaponHandler.LeftWeapon.GetComponent<Weapon>(), _leftWeaponCurrentAmmo, _leftWeaponIsReloading, _leftWeaponReserveAmmo);
        DisplayAmmo(_weaponHandler.RightWeapon.GetComponent<Weapon>(), _rightWeaponCurrentAmmo, _rightWeaponIsReloading, _rightWeaponReserveAmmo);
    }
    void DisplayStats()
    {
        _healthText.text = _playerStats.Health.ToString();
        _healthIndicator.fillAmount = Mathf.Clamp(_playerStats.Health * 0.01f, 0, 1);
        _dashIndicator.fillAmount = Mathf.MoveTowards(_dashIndicator.fillAmount, (_pMov.LastTimeSinceDash / _pMov.DashCooldown), 6f * Time.deltaTime);
    }
    void DisplayAmmo(Weapon weapon, TMP_Text loadedAmmoText, TMP_Text isReloadingText, TMP_Text reserveAmmoText)
    {
        if (weapon != null)
        {
            loadedAmmoText.text = $"{weapon.CurrentLoadedAmmo}/{weapon.MaxLoadedAmmo}";
            isReloadingText.gameObject.SetActive(weapon.IsReloading);
            reserveAmmoText.text = weapon.CurrentReserveAmmo.ToString();
        }
        else
        {
            loadedAmmoText.text = "--/--";
            reserveAmmoText.text = "---";
        }
    }
    void DisplayWeaponSwitchPrompt(WeaponPickup weaponPickup, bool leftWeapon, bool rightWeapon)
    {
        if(leftWeapon)
        {
            SwitchPromptL.SetActive(true);
            SwitchPromptL.GetComponentInChildren<TMP_Text>().text = $"{InputManager.Instance.GetBind("SwitchWeaponLeft")} to switch weapon to:";
            SwitchPromptL.GetComponentInChildren<Image>().sprite = weaponPickup.WeaponIcon;
        }
        if (rightWeapon)
        {
            SwitchPromptR.SetActive(true);
            SwitchPromptR.GetComponentInChildren<TMP_Text>().text = $"{InputManager.Instance.GetBind("SwitchWeaponRight")} to switch weapon to:";
            SwitchPromptR.GetComponentInChildren<Image>().sprite = weaponPickup.WeaponIcon;
        }
    }
    void HideWeaponSwitchPrompt()
    {
        SwitchPromptL.SetActive(false);
        SwitchPromptR.SetActive(false);
    }
    private void OnDisable()
    {
        _weaponHandler.OnWeaponHoverEnter -= DisplayWeaponSwitchPrompt;
        _weaponHandler.OnWeaponHoverExit -= HideWeaponSwitchPrompt;
    }
}
