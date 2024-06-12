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
    public FPSInput.PlayerActions PlayerActionsInstance;

    //* UI Zone
    public event Action<bool> OnSettingsEvent;
    public event Action<bool> OnSprintEvent;
    public event Action<bool> OnSitEvent;
    public event Action OnJumpEvent;
    public event Action OnReloadEvent;
    public event Action<bool> OnFireEvent;

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
        PlayerActionsInstance = FPSInputInstance.Player;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
            OnFireEvent?.Invoke(true);
        if (context.canceled)
            OnFireEvent?.Invoke(false);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
            OnReloadEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
            OnJumpEvent?.Invoke();
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
            OnSitEvent?.Invoke(true);
        }
        else
        {
            OnSitEvent?.Invoke(false);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        //TODO : 애니메이션이나 카메라 흔들림 추가해야함

        if (context.started || context.performed)
        {
            OnSprintEvent?.Invoke(true);
        }
        else
        {
            OnSprintEvent?.Invoke(false);
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
                OnSettingsEvent?.Invoke(_isSettings);
            }
            else
            {
                _isSettings = false;
                OnSettingsEvent?.Invoke(_isSettings);
            }
        }
    }
}
