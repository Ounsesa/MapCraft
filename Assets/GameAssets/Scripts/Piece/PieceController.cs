using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceController : MonoBehaviour
{
    public List<Piece> PlacedPiecesList;
    public List<Piece> PiecesToPlaceList;
    public Map Map;
    public Inventory Inventory;
    public Player Player;
    public GameObject PiecePrefab;
    public BuffsController BuffsController;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartResourcesLootingWithDelay", GameManager.Instance.GetLootInterval(PieceType.Resource));
        Invoke("StartMaterialsLootingWithDelay", GameManager.Instance.GetLootInterval(PieceType.Material));
    }
    void StartResourcesLootingWithDelay()
    {
        StartCoroutine(LootResourcesRoutine());
    }
    void StartMaterialsLootingWithDelay()
    {
        StartCoroutine(LootMaterialsRoutine());
    }

    public void AddPiece(Piece Piece)
    {
        PlacedPiecesList.Add(Piece);

        if(Piece.Type == PieceType.MaterialBuff)
        {
            GameManager.Instance.SetLootInterval(PieceType.Material, GameManager.Instance.GetLootInterval(PieceType.Material) * 0.9f);
        }
        else if(Piece.Type == PieceType.ResourceBuff)
        {
            GameManager.Instance.SetLootInterval(PieceType.Material, GameManager.Instance.GetLootInterval(PieceType.Material) * 0.9f);
        }
        else if(Piece.Type == PieceType.BiomeBuff)
        {
            BuffsController.AddMultiplier(GetBiomeType(Piece), 2);
        }

        if(Player.CurrentPiece == null && PiecesToPlaceList.Count > 0 ) 
        {
            Piece NewPiece = PiecesToPlaceList[0];
            PiecesToPlaceList.Remove(NewPiece);
            Player.CurrentPiece = NewPiece;
            NewPiece.CreatePiece();
        }
    }

    private BiomeType GetBiomeType(Piece Piece) 
    {
        Vector2Int Direction = Piece.WorldPosition - Map.WorldPosition;
        Direction.y = -Direction.y;

        return (BiomeType)Map.Matrix[Direction.y][Direction.x];      
    }

    public void SavePiece(Piece Piece)
    {
        if(Player.CurrentPiece == null)
        {
            Player.CurrentPiece = Piece;
            Piece.CreatePiece();
        }
    }

    void LootResources(PieceType typeToLoot)
    {
        Debug.Log("Loot" + typeToLoot);
        if(PlacedPiecesList.Count > 0)
        {
            List<List<int>> MapMatrix = Map.Matrix;

            for(int i = 0; i < PlacedPiecesList.Count; i++) 
            {
                if (PlacedPiecesList[i].Type == typeToLoot ) 
                {
                    Vector2Int MapPosition = PlacedPiecesList[i].WorldPosition - Map.WorldPosition;
                    MapPosition.y = Mathf.Abs(MapPosition.y);

                    for (int j = 0; j < PlacedPiecesList[i].Matrix.Count; j++)
                    {
                        for (int k = 0; k < PlacedPiecesList[i].Matrix[j].Count; k++)
                        {
                            if (PlacedPiecesList[i].Matrix[j][k] != GameManager.Instance.INVALID_TILE)
                            {
                                int value = Map.Matrix[MapPosition.y + j][MapPosition.x + k];

                                if (PlacedPiecesList[i].Type == PieceType.Resource)
                                {
                                    ResourceType resource = (ResourceType)(value);
                                    Inventory.AddResource((value), 1);
                                    Debug.Log($"Resource: {resource}");
                                }
                                else if (PlacedPiecesList[i].Type == PieceType.Material)
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
        
    }
    IEnumerator LootResourcesRoutine()
    {
        while (true)
        {
            LootResources(PieceType.Resource);
            yield return new WaitForSeconds(GameManager.Instance.GetLootInterval(PieceType.Resource));
        }
    }
    IEnumerator LootMaterialsRoutine()
    {
        while (true)
        {
            LootResources(PieceType.Material);
            yield return new WaitForSeconds(GameManager.Instance.GetLootInterval(PieceType.Material));
        }
    }

    public bool IsPieceOverlapping(Piece piece)
    {
        for(int i = 0; i < PlacedPiecesList.Count; i++)
        {
            Piece OtherPiece = PlacedPiecesList[i];
            if(OtherPiece.WorldPosition.x + OtherPiece.Matrix[0].Count < piece.WorldPosition.x ||
               OtherPiece.WorldPosition.y - OtherPiece.Matrix.Count > piece.WorldPosition.y)
            {
                continue;
            }

            Vector2Int Direction = piece.WorldPosition - OtherPiece.WorldPosition;

            for(int j = 0; j < piece.Matrix.Count; j ++) 
            { 
                for(int k = 0; k < piece.Matrix[j].Count; k ++)
                {
                    Vector2Int AuxPosition = Direction + new Vector2Int(k, -j);
                    if(AuxPosition.x >= OtherPiece.Matrix[0].Count ||
                       Mathf.Abs(AuxPosition.y) >= OtherPiece.Matrix.Count)
                    {
                        continue;
                    }
                    if (OtherPiece.Matrix[Mathf.Abs(AuxPosition.y)][AuxPosition.x] != GameManager.Instance.INVALID_TILE && piece.Matrix[j][k] != GameManager.Instance.INVALID_TILE)
                    {
                        Debug.Log("Overlapping");
                        return true;
                    }

                }
            }

        }
        return false;
    }
}
