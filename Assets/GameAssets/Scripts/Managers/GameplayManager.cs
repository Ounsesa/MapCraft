using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Manager
{
    public static int INVALID_TILE = -1;

    public float MaxCameraSize = 5f;
    public float MinCameraSize = 20f;

    public Map Map;
    public Inventory Inventory;

    public static float LootInterval = 5.0f; // Interval in seconds between looting

}
