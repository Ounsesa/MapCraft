using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.DefaultInputActions;

public class InputManager : Manager
{

    public InputActions RegisterInputActionsPlayer(PlayerInput playerInput, Player player)
    {
        InputActions _playerActions = new InputActions();
        

        ActionsName _actionsName = GameManager.Instance.ActionsName;
        _playerActions.RightClickAction = playerInput.actions.FindAction(_actionsName.RightClickAction);
        _playerActions.MoveCameraAction = playerInput.actions.FindAction(_actionsName.MoveCameraAction);
        _playerActions.PlayerAction = playerInput.actions.FindAction(_actionsName.PlayerAction);
        _playerActions.MouseWheelAction = playerInput.actions.FindAction(_actionsName.MouseWheelAction);

        return _playerActions;
    }
}
public class InputActions
{
    #region Properties
    public InputAction RightClickAction;
    public InputAction MoveCameraAction;
    public InputAction PlayerAction;
    public InputAction MouseWheelAction;
    #endregion
}
[System.Serializable]
public struct ActionsName
{
    public string RightClickAction;
    public string MoveCameraAction;
    public string PlayerAction;
    public string MouseWheelAction;
}