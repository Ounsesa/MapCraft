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


    public int INVALID_TILE = -1;

    public float MaxCameraSize = 20f;
    public float MinCameraSize = 5f;

    public string GameDataPath = "Assets/GameAssets/GameData/";

    public float LootInterval = 5.0f; // Interval in seconds between looting

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
}
