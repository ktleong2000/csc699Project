using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MoveEvent;

    public event Action<bool> AttackEvent;
    private Controls controls;

    private void OnEnable()
    {
        if(controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        controls.Player.Enable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //Listens for an attack key press and duration held
        if(context.performed)
        {
            AttackEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            AttackEvent?.Invoke(false);
        }
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
}
