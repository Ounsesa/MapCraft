using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager
{
    public static GameManager Instance;

    [HideInInspector]
    public InputManager InputManager;

    [Header("Inputs")]
    public ActionsName ActionsName;

    public bool TutorialOpen = false;


    public int INVALID_TILE = -1;

    public float MaxCameraSize = 20f;
    public float MinCameraSize = 5f;

    public string GameDataPath = "Assets/GameAssets/GameData/";

    [SerializeField]
    private float ResourcesLootInterval = 5.0f; // Interval in seconds between looting
    [SerializeField]
    private float MaterialsLootInterval = 6.0f; // Interval in seconds between looting


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        Instance = this;

        InputManager = new InputManager();
    }

    public void SetLootInterval(PieceType pieceType, float LootInterval)
    {
        if(pieceType == PieceType.Resource)
        {
            ResourcesLootInterval = LootInterval;
        }
        else if(pieceType == PieceType.Material)
        {
            MaterialsLootInterval = LootInterval;
        }
    }
    public float GetLootInterval(PieceType pieceType)
    {
        if(pieceType == PieceType.Resource)
        {
            return ResourcesLootInterval;
        }
        else if(pieceType == PieceType.Material)
        {
            return MaterialsLootInterval;
        }
        return -1;
    }
}
