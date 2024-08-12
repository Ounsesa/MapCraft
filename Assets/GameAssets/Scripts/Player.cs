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
    [HideInInspector]
    public Piece CurrentPiece;
    public Map Map;
    public float CameraSpeed = 3f;
    public float CameraZoomedInSpeed = 3f;
    public float CameraZoomedOutSpeed = 12f;
    public float CameraZoomSpeed = 200f;
    public PieceController PieceController;
    public MapCraftingButton MapCraftingButton;
    public bool CanPlacePiece;

    private bool FirstPiecePlaced = false;

    public void StartPiece()
    {
        // Initialize CurrentPiece.Matrix using List<List<int>>
        List<List<int>> Matrix = new List<List<int>>()
        {
            new List<int> { 0, 0, 0 },
            new List<int> { 0, GameManager.Instance.INVALID_TILE, GameManager.Instance.INVALID_TILE }
        };

        GameObject tile = Instantiate(PieceController.PiecePrefab);
        CurrentPiece = tile.GetComponent<Piece>();
        CurrentPiece.InitPiece(PieceType.Material, Matrix);
        CurrentPiece.CreatePiece();

        CanPlacePiece = true;
    }

    private void Start()
    {
        CanPlacePiece = false;
        PlayerInput playerInput = GetComponent<PlayerInput>();
        InputAction = GameManager.Instance.InputManager.RegisterInputActionsPlayer(playerInput, this);        
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
                CameraZoomAction();
                Debug.Log("Acercar/alejar la c�mara");
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
        if (CurrentPiece != null && CanPlacePiece && !GameManager.Instance.GameEnded)
        { 
            if (CurrentPiece.Type != PieceType.MapExtension && Map.AddPieceToMap(CurrentPiece))
            {
                Debug.Log("Piece placed");
                PieceController.AddPiece(CurrentPiece);
                CurrentPiece = null;

                if(!FirstPiecePlaced)
                {
                    FirstPiecePlaced = true;
                    Tutorial.Instance.StartTutorialFirstPiecePlaced();
                }
            }
            else if (CurrentPiece.Type == PieceType.MapExtension && Map.ExtendMap(CurrentPiece))
            {
                Debug.Log("Adjacent map piece");
                Destroy(CurrentPiece.gameObject);
                CurrentPiece = null;
            }
        }
    }

    private void CameraZoomAction()
    {
        float MouseWheelValue = InputAction.MouseWheelAction.ReadValue<float>();
        if(MouseWheelValue < 0) 
        {
            float CurrentSize = Camera.main.GetComponent<Camera>().orthographicSize;
            float NewSize = Mathf.Min(CurrentSize + CameraZoomSpeed * Time.deltaTime, GameManager.Instance.MaxCameraSize);
            CameraSpeed = Mathf.Min(CameraSpeed + CameraZoomSpeed * Time.deltaTime, CameraZoomedOutSpeed);
            Debug.Log(NewSize);
            Camera.main.GetComponent<Camera>().orthographicSize = NewSize;
        }
        else 
        {
            float CurrentSize = Camera.main.GetComponent<Camera>().orthographicSize;
            float NewSize = Mathf.Max(CurrentSize - CameraZoomSpeed * Time.deltaTime, GameManager.Instance.MinCameraSize);
            CameraSpeed = Mathf.Max(CameraSpeed - CameraZoomSpeed * Time.deltaTime, CameraZoomedInSpeed);
            Camera.main.GetComponent<Camera>().orthographicSize = NewSize;
        }
    }

}
