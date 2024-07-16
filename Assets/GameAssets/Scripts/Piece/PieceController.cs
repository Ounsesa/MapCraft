using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public List<Piece> PlacedPiecesList;
    public List<Piece> PiecesToPlaceList;
    public Map Map;
    public Inventory Inventory;
    public Player Player;
    public GameObject PiecePrefab;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartLootingWithDelay", GameManager.Instance.LootInterval);
    }
    void StartLootingWithDelay()
    {
        StartCoroutine(LootResourcesRoutine());
    }

    public void AddPiece(Piece Piece)
    {
        PlacedPiecesList.Add(Piece);
        if(Player.CurrentPiece == null && PiecesToPlaceList.Count > 0 ) 
        {
            Piece NewPiece = PiecesToPlaceList[0];
            PiecesToPlaceList.Remove(NewPiece);
            Player.CurrentPiece = NewPiece;
            NewPiece.CreatePiece();
        }
    }

    public void SavePiece(Piece Piece)
    {
        if(Player.CurrentPiece == null)
        {
            Player.CurrentPiece = Piece;
            Piece.CreatePiece();
        }
    }

    void LootResources()
    {
        Debug.Log("Loot");
        if(PlacedPiecesList.Count > 0)
        {
            List<List<int>> MapMatrix = Map.Matrix;

            for(int i = 0; i < PlacedPiecesList.Count; i++) 
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
    IEnumerator LootResourcesRoutine()
    {
        while (true)
        {
            LootResources();
            yield return new WaitForSeconds(GameManager.Instance.LootInterval);
        }
    }

    public bool IsPieceOverlapping(Piece piece)
    {
        for(int i = 0; i < PlacedPiecesList.Count; i++)
        {
            Piece OtherPiece = PlacedPiecesList[i];
            if(OtherPiece.WorldPosition.x < piece.WorldPosition.x && OtherPiece.WorldPosition.x + OtherPiece.Matrix[0].Count > piece.WorldPosition.x ||
               OtherPiece.WorldPosition.x > piece.WorldPosition.x + piece.Matrix[0].Count && OtherPiece.WorldPosition.x + OtherPiece.Matrix[0].Count < piece.WorldPosition.x + piece.Matrix[0].Count)
            {
                Debug.Log("Overlapping");
            }
        }
        return true;
    }
}
