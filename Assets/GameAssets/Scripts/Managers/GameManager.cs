using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance;
    public const int INVALID_TILE = -1;
    [HideInInspector]
    public InputManager inputManager;
    [Header("Inputs")]
    public ActionsName actionsName;
    [HideInInspector]
    public bool tutorialOpen = false;
    [HideInInspector]
    public bool gameEnded = false;
    public float maxCameraSize = 20f;
    public float minCameraSize = 5f;

    [SerializeField]
    private float m_resourcesLootInterval = 5.0f; // Interval in seconds between looting
    [SerializeField]
    private float m_materialsLootInterval = 6.0f; // Interval in seconds between looting
    #endregion

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        instance = this;

        inputManager = new InputManager();
    }

    public void SetLootInterval(PieceType pieceType, float LootInterval)
    {
        if(pieceType == PieceType.Resource)
        {
            m_resourcesLootInterval = LootInterval;
        }
        else if(pieceType == PieceType.Material)
        {
            m_materialsLootInterval = LootInterval;
        }
    }
    public float GetLootInterval(PieceType pieceType)
    {
        if(pieceType == PieceType.Resource)
        {
            return m_resourcesLootInterval;
        }
        else if(pieceType == PieceType.Material)
        {
            return m_materialsLootInterval;
        }
        return -1;
    }
}
