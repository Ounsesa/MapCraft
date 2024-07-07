using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

enum PlayerState
{
    None,
    Card,
    MapExtension
}


public class Player : MonoBehaviour
{
    private InputActions InputAction;
    public Piece CurrentPiece;
    public Map Map;
    public float CameraSpeed = 3f; 

    private void Start()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputAction = GameManager.Instance.InputManager.RegisterInputActionsPlayer(playerInput, this);


        CurrentPiece.PieceMatrix = new int[2][];
        CurrentPiece.PieceMatrix[0] = new int[3] { 1, 1, 1 };
        CurrentPiece.PieceMatrix[1] = new int[3] { 1, 0, 1 };
        CurrentPiece.CreatePiece();

    }

    // Update is called once per frame
    void Update()
    {
        ReceiveInput();
        if(CurrentPiece != null)
        {
            CurrentPiece.Position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        }
    }

    private void ReceiveInput()
    {
        
        if (InputAction.RightClickAction.IsPressed())
        {
            MoveCameraAction();
        }
        else if (InputAction.PlayerAction.triggered)
        {
            PerformAction();
        }
        else if(InputAction.MouseWheelAction.triggered)
        {        
            if(CurrentPiece != null)
            {
                CurrentPiece.RotatePiece(InputAction.MouseWheelAction.ReadValue<float>() > 0 ? true : false);
            }
        }
    }

    private void MoveCameraAction()
    {
        Vector2 MouseMovement = InputAction.MoveCameraAction.ReadValue<Vector2>();
        Vector3 NewPosition = new Vector3(-MouseMovement.x, -MouseMovement.y, 0) * CameraSpeed * Time.deltaTime;
        Camera.main.transform.position += NewPosition;
    }

    private void PerformAction()
    {
        //Debug.Log($"PiecePosition {CurrentPiece.Position.x}, {CurrentPiece.Position.y}");
        if (CurrentPiece == null)
        {
            return;
        }
        if (Map.AddPieceToMap(CurrentPiece))
        {
            //CurrentPiece.CreatePiece();

            Debug.Log("Piece placed");
            CurrentPiece = null;
        }
        else
        {
            Debug.Log("Cannot place piece here");
        }
    }

}
