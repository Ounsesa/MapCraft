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
    #region Variables
    public PieceController pieceController;
    [HideInInspector]
    public Piece currentPiece;    
    [HideInInspector]
    public bool canPlacePiece;

    private bool m_firstPiecePlaced = false;
    private InputActions m_inputAction;
    [SerializeField]
    private Map m_map;
    [SerializeField]
    private float m_cameraSpeed = 3f;
    [SerializeField]
    private float m_cameraZoomedInSpeed = 3f;
    [SerializeField]
    private float m_cameraZoomedOutSpeed = 12f;
    [SerializeField]
    private float m_cameraZoomSpeed = 200f;
    #endregion

    public void StartPiece()
    {
        // Initialize CurrentPiece.Matrix using List<List<int>>
        List<List<int>> Matrix = new List<List<int>>()
        {
            new List<int> { 0, 0, 0 },
            new List<int> { 0, GameManager.INVALID_TILE, GameManager.INVALID_TILE }
        };

        GameObject Tile = Instantiate(pieceController.piecePrefab);
        currentPiece = Tile.GetComponent<Piece>();
        currentPiece.InitPiece(PieceType.Material, Matrix);
        currentPiece.CreatePiece();

        canPlacePiece = true;
    }

    private void Start()
    {
        canPlacePiece = false;
        PlayerInput PlayerInput = GetComponent<PlayerInput>();
        m_inputAction = GameManager.instance.inputManager.RegisterInputActionsPlayer(PlayerInput, this);        
    }

    // Update is called once per frame
    void Update()
    {
        ReceiveInput();
        if(currentPiece != null)
        {
            currentPiece.worldPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        }
    }

    private void ReceiveInput()
    {
        
        if (m_inputAction.rightClickAction.IsPressed())
        {
            if (m_inputAction.mouseWheelAction.triggered)
            {
                CameraZoomAction();
                Debug.Log("Acercar/alejar la cámara");
            }
            MoveCameraAction();
        }
        else if (m_inputAction.playerAction.triggered)
        {
            PerformAction();
        }
        else if(m_inputAction.mouseWheelAction.triggered)
        {        
            if(currentPiece != null)
            {
                currentPiece.RotatePiece(m_inputAction.mouseWheelAction.ReadValue<float>() > 0 ? true : false);
            }
        }

    }

    private void MoveCameraAction()
    {
        Vector2 MouseMovement = m_inputAction.moveCameraAction.ReadValue<Vector2>();
        Vector3 NewPosition = new Vector3(-MouseMovement.x, -MouseMovement.y, 0) * m_cameraSpeed * Time.deltaTime;
        Camera.main.transform.position += NewPosition;
    }

    private void PerformAction()
    {
        if (currentPiece != null && canPlacePiece && !GameManager.instance.gameEnded)
        { 
            if (currentPiece.type != PieceType.MapExtension && m_map.AddPieceToMap(currentPiece))
            {
                Debug.Log("Piece placed");
                pieceController.AddPiece(currentPiece);
                currentPiece = null;

                if(!m_firstPiecePlaced)
                {
                    m_firstPiecePlaced = true;
                    Tutorial.instance.StartTutorialFirstPiecePlaced();
                }
            }
            else if (currentPiece.type == PieceType.MapExtension && m_map.ExtendMap(currentPiece))
            {
                Debug.Log("Adjacent map piece");
                Destroy(currentPiece.gameObject);
                currentPiece = null;
            }
        }
    }

    private void CameraZoomAction()
    {
        float MouseWheelValue = m_inputAction.mouseWheelAction.ReadValue<float>();
        if(MouseWheelValue < 0) 
        {
            float CurrentSize = Camera.main.GetComponent<Camera>().orthographicSize;
            float NewSize = Mathf.Min(CurrentSize + m_cameraZoomSpeed * Time.deltaTime, GameManager.instance.maxCameraSize);
            m_cameraSpeed = Mathf.Min(m_cameraSpeed + m_cameraZoomSpeed * Time.deltaTime, m_cameraZoomedOutSpeed);
            Debug.Log(NewSize);
            Camera.main.GetComponent<Camera>().orthographicSize = NewSize;
        }
        else 
        {
            float CurrentSize = Camera.main.GetComponent<Camera>().orthographicSize;
            float NewSize = Mathf.Max(CurrentSize - m_cameraZoomSpeed * Time.deltaTime, GameManager.instance.minCameraSize);
            m_cameraSpeed = Mathf.Max(m_cameraSpeed - m_cameraZoomSpeed * Time.deltaTime, m_cameraZoomedInSpeed);
            Camera.main.GetComponent<Camera>().orthographicSize = NewSize;
        }
    }

}
