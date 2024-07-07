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
