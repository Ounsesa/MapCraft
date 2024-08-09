using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceController : MonoBehaviour
{
    public List<Piece> PlacedPiecesList;
    public Map Map;
    public Inventory Inventory;
    public Player Player;
    public GameObject PiecePrefab;
    public BuffsController BuffsController;


    public List<GameObject> OtherBiomesPiecesList;
    public int NumberOfOtherBiomes = 10;
    public int DistanceFromMap = 10;
    public int DistanceBetweenLayers = 10;
    public int RandomOffset = 5;
    public int RandomValueForNotPlacePiece = 3;

    public int RandomValueForFinalMapExtension = 1;

    private List<List<List<int>>> MapExtensionsFormsList;


    // Start is called before the first frame update
    void Start()
    {
        InitializeOtherBiomes();
        
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
            GameManager.Instance.SetLootInterval(PieceType.Resource, GameManager.Instance.GetLootInterval(PieceType.Resource) * 0.9f);
        }
        else if(Piece.Type == PieceType.BiomeBuff)
        {
            BuffsController.AddMultiplier(GetBiomeType(Piece), 1);
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

                                    if(Random.Range(0, 100) <= RandomValueForFinalMapExtension)
                                    {
                                        Inventory.AddFinalExtensionTile();
                                        Debug.Log("Final Extension Looted");
                                    }
                                }
                            }
                        }
                    }
                }                
            }
            
        }
        
    }

    public bool CreateBuffPiece(PieceType piece)
    {
        if(Player.CurrentPiece != null)
        {
            return false;
        }
        // Initialize CurrentPiece.Matrix using List<List<int>>
        List<List<int>> Matrix = new List<List<int>>()
                {
                    new List<int> { 1 }
                };

        GameObject tile = Instantiate(PiecePrefab);
        Piece auxPiece = tile.GetComponent<Piece>();
        auxPiece.InitPiece(piece, Matrix);
        auxPiece.CreatePiece();
        Player.CurrentPiece = auxPiece;

        return true;
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

    public bool IsOverlapingOtherMapPieces(Piece MapExtensionPiece)
    {
        List<Piece> pieces = new List<Piece>();

        for(int i = 0; i < OtherBiomesPiecesList.Count; i++) 
        {
            if(OtherBiomesPiecesList[i].GetComponent<Piece>() != MapExtensionPiece)
            {
                pieces.Add(OtherBiomesPiecesList[i].GetComponent<Piece>());
            }
        }
        return IsPieceOverlapping(MapExtensionPiece, pieces);
    }

    public bool IsPieceOverlapping(Piece piece, List<Piece> PieceListToIterate) //pieceType 0 placed pieces / 1 map extension pieces 
    {    
        for (int i = 0; i < PieceListToIterate.Count; i++)
        {
            Piece OtherPiece = PieceListToIterate[i];
            if (OtherPiece.WorldPosition.x + OtherPiece.Matrix[0].Count - 1 < piece.WorldPosition.x ||
               OtherPiece.WorldPosition.y - OtherPiece.Matrix.Count - 1 > piece.WorldPosition.y ||
               piece.WorldPosition.x + piece.Matrix[0].Count - 1 < OtherPiece.WorldPosition.x ||
               piece.WorldPosition.y - piece.Matrix.Count - 1 > OtherPiece.WorldPosition.y)
            {
                continue;
            }

            Vector2Int Direction = piece.WorldPosition - OtherPiece.WorldPosition;

            for(int j = 0; j < piece.Matrix.Count; j ++) 
            { 
                for(int k = 0; k < piece.Matrix[j].Count; k ++)
                {
                    Vector2Int AuxPosition = Direction + new Vector2Int(k, -j);
                    if (AuxPosition.x < 0 ||
                        AuxPosition.y > 0 ||
                        AuxPosition.x >= OtherPiece.Matrix[0].Count ||
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

    void InitializeOtherBiomes()
    {
        InitilizaOtherBiomesForms();

        List<Vector2Int> Directions = new List<Vector2Int>()
        {
            new Vector2Int (0, 1), //Up
            new Vector2Int (0, -1), // Down
            new Vector2Int (1, 0), //Right
            new Vector2Int (-1, 0), //Left
            new Vector2Int (-1, -1), //BottomLeft
            new Vector2Int (1, -1), // BottomRight
            new Vector2Int (1, 1), //Top Right
            new Vector2Int (-1, 1), // TopLeft
        };

        int ExtensionsPlaced = 0;

        while (ExtensionsPlaced < NumberOfOtherBiomes)
        {
            for (int j = 0; j < Directions.Count; j++)
            {
                if (Random.Range(0, RandomValueForNotPlacePiece) == 0)
                {
                    continue;
                }

                GameObject tile = Instantiate(PiecePrefab);
                Piece BiomePiece = tile.GetComponent<Piece>();

                int RandomBiome = Random.Range(0, ResourcesManager.GetEnumLength<BiomeType>());


                List<List<int>> Matrix = MapExtensionsFormsList[Random.Range(0, MapExtensionsFormsList.Count)]
                                        .Select(innerList => innerList.ToList()) // Creates a shallow copy of each inner list
                                        .ToList(); // Creates a shallow copy of the outer list

                for (int i = 0; i < Matrix.Count; i++)
                {
                    for (int k = 0; k < Matrix[i].Count; k++)
                    {
                        if (Matrix[i][k] == 0)
                        {
                            Matrix[i][k] = RandomBiome;
                        }
                    }
                }

                Vector2Int WorldPosition = Directions[j] * DistanceFromMap;
                WorldPosition += new Vector2Int(1, 1) * Random.Range(-RandomOffset, RandomOffset);

                BiomePiece.InitPiece(PieceType.MapExtension, Matrix);
                BiomePiece.CreatePiece();
                BiomePiece.WorldPosition = WorldPosition;
                ExtensionsPlaced++;

                OtherBiomesPiecesList.Add(tile);

                if(ExtensionsPlaced >= NumberOfOtherBiomes)
                {
                    break;
                }


            }

            DistanceFromMap += DistanceBetweenLayers;
            RandomOffset += DistanceBetweenLayers / 2;
        }

    }

    void InitilizaOtherBiomesForms()
    {
        MapExtensionsFormsList = new List<List<List<int>>>()
        {
            new List<List<int>>
            { 
                new List<int> { GameManager.Instance.INVALID_TILE, 0, GameManager.Instance.INVALID_TILE },
                new List<int> { 0, 0 , 0 },
                new List<int> { GameManager.Instance.INVALID_TILE, 0, GameManager.Instance.INVALID_TILE }
            },

            new List<List<int>>
            { 
                new List<int> { 0, 0 , 0 },
                new List<int> {0, GameManager.Instance.INVALID_TILE, GameManager.Instance.INVALID_TILE }
            },

            new List<List<int>>
            {
                new List<int> { 0, 0},
                new List<int> {0, GameManager.Instance.INVALID_TILE },
                new List<int> {0, 0}
            },

            new List<List<int>>
            {
                new List<int> {0, GameManager.Instance.INVALID_TILE, 0 },
                new List<int> { 0, 0, 0},
                new List<int> { 0, 0, 0 }
            },

            new List<List<int>>
            {
                new List<int> {0, GameManager.Instance.INVALID_TILE, GameManager.Instance.INVALID_TILE },
                new List<int> { 0, 0, GameManager.Instance.INVALID_TILE},
                new List<int> { 0, 0, 0 }
            },

            new List<List<int>>
            {
                new List<int> {0, 0 , 0 },
                new List<int> { GameManager.Instance.INVALID_TILE, 0, 0},
                new List<int> { GameManager.Instance.INVALID_TILE, GameManager.Instance.INVALID_TILE, 0 }
            },

            new List<List<int>>
            {
                new List<int> { GameManager.Instance.INVALID_TILE, 0, 0},
                new List<int> { 0, 0, 0 }
            },

            new List<List<int>>
            {
                new List<int> { 0, 0, 0},
                new List<int> { 0, GameManager.Instance.INVALID_TILE, 0 },
                new List<int> { 0, 0, 0 }
            },
        };
    }

    public void IsAdjacentToMapExtension(Piece piece)
    {
        for (int i = OtherBiomesPiecesList.Count - 1; i >= 0; i--)
        {
            GameObject AuxGameObject = OtherBiomesPiecesList[i];
            Piece OtherPiece = AuxGameObject.GetComponent<Piece>();
            if (Map.CheckAdjacency(piece, OtherPiece.WorldPosition, OtherPiece.Matrix))
            {
                OtherBiomesPiecesList.RemoveAt(i);
                if (Map.ExtendMap(OtherPiece))
                {
                    Destroy(AuxGameObject);
                }
                else
                {
                    OtherBiomesPiecesList.Add(AuxGameObject);
                }
            }
        }
    }
       
}
