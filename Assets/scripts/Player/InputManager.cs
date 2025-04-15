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
    public bool ShootingLeft => InputActions.Player.FireLeft.ReadValue<float>() > 0;
    public bool ShootingRight => InputActions.Player.FireRight.ReadValue<float>() > 0;
    public bool ReloadLeft  => InputActions.Player.ReloadLeft.WasPressedThisFrame();
    public bool ReloadRight  => InputActions.Player.ReloadRight.WasPressedThisFrame();

    private void OnDisable()
    {
        if(InputActions != null)
        {
            InputActions.Disable();
        }
    }

}
