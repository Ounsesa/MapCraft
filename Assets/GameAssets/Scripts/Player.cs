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


        // Initialize CurrentPiece.Matrix using List<List<int>>
        CurrentPiece.Matrix = new List<List<int>>()
        {
            new List<int> { 0, 0, 0 },
            new List<int> { 0, GameplayManager.INVALID_TILE, GameplayManager.INVALID_TILE }
        };
        CurrentPiece.CreatePiece();

    }

    // Update is called once per frame
    void Update()
    {
        ReceiveInput();
        if(CurrentPiece != null)
        {
            CurrentPiece.WorldPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        }
    }

    private void ReceiveInput()
    {
        
        if (InputAction.RightClickAction.IsPressed())
        {
            if (InputAction.MouseWheelAction.triggered)
            {
                Debug.Log("Acercar/alejar la cámara");
            }
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
        if (CurrentPiece != null)
        { 
            //Debug.Log($"PiecePosition {CurrentPiece.Position.x}, {CurrentPiece.Position.y}");
            if (CurrentPiece.Type != PieceType.MapExtension && Map.AddPieceToMap(CurrentPiece))
            {
                Debug.Log("Piece placed");
                CurrentPiece = null;
            }
            else if (CurrentPiece.Type == PieceType.MapExtension && Map.ExtendMap(CurrentPiece))
            {
                Debug.Log("Adjacent map piece");
            }
        }
    }

}
