using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public Controls InputActions { get; private set; }
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InputActions = new Controls();
        InputActions.Enable();
    }
    public Vector2 Move => InputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 Look => InputActions.Player.Look.ReadValue<Vector2>();
    public bool Dash => InputActions.Player.Dash.WasPressedThisFrame();
    public bool ShootSpecial => InputActions.Player.FireSpecial.WasPressedThisFrame();
    public bool FiringLeft => InputActions.Player.FireLeft.ReadValue<float>() > 0;
    public bool FirePressedLeft => InputActions.Player.FirePressedLeft.WasPressedThisFrame();
    public bool FiringRight => InputActions.Player.FireRight.ReadValue<float>() > 0;
    public bool FirePressedRight => InputActions.Player.FirePressedRight.WasPressedThisFrame();
    public bool ReloadLeft  => InputActions.Player.ReloadLeft.WasPressedThisFrame();
    public bool ReloadRight  => InputActions.Player.ReloadRight.WasPressedThisFrame();
    public bool SwitchWeaponLeft => InputActions.Player.SwitchWeaponLeft.WasPressedThisFrame();
    public bool SwitchWeaponRight => InputActions.Player.SwitchWeaponRight.WasPressedThisFrame();
    public bool DropWeaponLeft => InputActions.Player.DropWeaponLeft.WasPressedThisFrame();
    public bool DropWeaponRight => InputActions.Player.DropWeaponRight.WasPressedThisFrame();
    public string GetBind(string action)
    {
        return InputActions.FindAction(action).GetBindingDisplayString();
    }
    private void OnDisable()
    {
        if(InputActions != null)
        {
            InputActions.Disable();
        }
    }
}
