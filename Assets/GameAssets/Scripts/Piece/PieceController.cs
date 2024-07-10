using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public List<Piece> PiecesList;
    public Map Map;
    public Inventory Inventory;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartLootingWithDelay", GameplayManager.LootInterval);
    }
    void StartLootingWithDelay()
    {
        StartCoroutine(LootResourcesRoutine());
    }

    public void AddPiece(Piece Piece)
    {
        PiecesList.Add(Piece);
    }

    void LootResources()
    {
        Debug.Log("Loot");
        if(PiecesList.Count > 0)
        {
            List<List<int>> MapMatrix = Map.Matrix;

            for(int i = 0; i < PiecesList.Count; i++) 
            { 
                Vector2Int MapPosition = PiecesList[i].WorldPosition - Map.WorldPosition;
                MapPosition.y = Mathf.Abs(MapPosition.y);

                for (int j = 0; j < PiecesList[i].Matrix.Count; j++)
                {
                    for (int k = 0; k < PiecesList[i].Matrix[j].Count; k++)
                    {
                        if (PiecesList[i].Matrix[j][k] != GameplayManager.INVALID_TILE)
                        {
                            int value = Map.Matrix[MapPosition.y + j][MapPosition.x + k];

                            if (PiecesList[i].Type == PieceType.Resource)
                            {
                                ResourceType resource = (ResourceType)(value);
                                Inventory.AddResource((value), 1);
                                Debug.Log($"Resource: {resource}");
                            }
                            else if (PiecesList[i].Type == PieceType.Material)
                            {
                                MaterialType material = (MaterialType)(value);
                                Inventory.AddMaterial((value), 1);
                                Debug.Log($"Material: {material}");
                            }                                
                        }
                    }
                }
            }

            
        }
        
    }
    IEnumerator LootResourcesRoutine()
    {
        while (true)
        {
            LootResources();
            yield return new WaitForSeconds(GameplayManager.LootInterval);
        }
    }
}
