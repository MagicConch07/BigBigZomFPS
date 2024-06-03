using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, FPSInput.IPlayerActions, FPSInput.IUIActions
{
    public FPSInput FPSInputInstance;

    //* UI Zone
    public event Action<bool> SettingsEvent;
    public event Action<bool> SprintEvent;
    public event Action<bool> SitEvent;
    public event Action<bool> JumpEvent;

    private void OnEnable()
    {
        if (FPSInputInstance == null)
        {
            FPSInputInstance = new FPSInput();
            FPSInputInstance.Player.SetCallbacks(this);
            FPSInputInstance.UI.SetCallbacks(this);
        }

        FPSInputInstance.Player.Enable();
        FPSInputInstance.UI.Enable();
    }

    public void OnFire(InputAction.CallbackContext context)
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpEvent?.Invoke(true);
        }
        else
        {
            JumpEvent?.Invoke(false);
        }
    }

    public void OnMouseView(InputAction.CallbackContext context)
    {

    }

    public void OnMovement(InputAction.CallbackContext context)
    {

    }

    public void OnSit(InputAction.CallbackContext context)
    {
        //! Hold로 할꺼면 이렇게 구현하면 안됨
        if (context.started || context.performed)
        {
            SitEvent?.Invoke(true);
        }
        else
        {
            SitEvent?.Invoke(false);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //TODO : 애니메이션이나 카메라 흔들림 추가해야함

        if (context.started || context.performed)
        {
            SprintEvent?.Invoke(true);
        }
        else
        {
            SprintEvent?.Invoke(false);
        }
    }

    private bool _isSettings = false;

    //* UI Zone
    public void OnSettings(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (_isSettings == false)
            {
                _isSettings = true;
                SettingsEvent?.Invoke(_isSettings);
            }
            else
            {
                _isSettings = false;
                SettingsEvent?.Invoke(_isSettings);
            }
        }
    }
}
