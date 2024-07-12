using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Manager
{
    public static GameplayManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        Instance = this;
    }
   


}
