using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.DefaultInputActions;

public class InputManager : MonoBehaviour
{

    public InputActions RegisterInputActionsPlayer(PlayerInput playerInput, Player player)
    {
        InputActions _playerActions = new InputActions();
        

        ActionsName _actionsName = GameManager.instance.actionsName;
        _playerActions.rightClickAction = playerInput.actions.FindAction(_actionsName.rightClickAction);
        _playerActions.moveCameraAction = playerInput.actions.FindAction(_actionsName.moveCameraAction);
        _playerActions.playerAction = playerInput.actions.FindAction(_actionsName.playerAction);
        _playerActions.mouseWheelAction = playerInput.actions.FindAction(_actionsName.mouseWheelAction);

        return _playerActions;
    }
}
public class InputActions
{
    #region Properties
    public InputAction rightClickAction;
    public InputAction moveCameraAction;
    public InputAction playerAction;
    public InputAction mouseWheelAction;
    #endregion
}
[System.Serializable]
public struct ActionsName
{
    public string rightClickAction;
    public string moveCameraAction;
    public string playerAction;
    public string mouseWheelAction;
}