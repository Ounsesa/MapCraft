using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Manager
{
    public Map Map;
    public ResourcesManager ResourcesManager;
    public Inventory Inventory;

    public float LootInterval = 5.0f; // Interval in seconds between looting

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartLootingWithDelay", LootInterval);
    }
    void StartLootingWithDelay()
    {
        StartCoroutine(LootResourcesRoutine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void LootResources()
    {
        int[][] MapMatrix = Map.MapMatrix;

        for(int i = 0; i < MapMatrix.Length; i++)
        {
            for(int j = 0; j < MapMatrix[i].Length; j++)
            {
                ResourcesManager.ParseIntToResource(MapMatrix[i][j]);
            }
        }
    }
    IEnumerator LootResourcesRoutine()
    {
        while (true)
        {
            LootResources();
            yield return new WaitForSeconds(LootInterval);
        }
    }
}
