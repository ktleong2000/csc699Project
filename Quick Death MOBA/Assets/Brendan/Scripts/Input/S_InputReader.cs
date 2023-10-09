using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static S_Controls;

[CreateAssetMenu(fileName = "New S InputReader", menuName = "Input/Input Reader")]
public class S_InputReader : ScriptableObject, IS_manActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MoveEvent;

    public Vector2 AimPosition {get; private set;}
    private S_Controls s_controls;
    private void OnEnable() {
        if(s_controls == null){
            s_controls = new S_Controls();
            s_controls.S_man.SetCallbacks(this);
        }
        s_controls.S_man.Enable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed){
            PrimaryFireEvent?.Invoke(true);
        }else if(context.canceled){
            PrimaryFireEvent?.Invoke(false);
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }
}
