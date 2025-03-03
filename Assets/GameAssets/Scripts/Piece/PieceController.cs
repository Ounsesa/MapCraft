using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceController : MonoBehaviour
{
    #region Variables
    public GameObject piecePrefab;
    public List<List<int>> finalMatrix;
    [HideInInspector]
    public Vector2Int finalPosition;
    [HideInInspector]
    public List<Piece> placedPiecesList;

    [SerializeField]
    private int m_numberOfOtherBiomes = 10;
    [SerializeField]
    private int DistanceFromMap = 10;
    [SerializeField]
    private int DistanceBetweenLayers = 10;
    [SerializeField]
    private int RandomOffset = 5;
    [SerializeField]
    private int RandomValueForNotPlacePiece = 3;
    [SerializeField]
    private int RandomValueForFinalMapExtension = 1;
    private List<List<List<int>>> MapExtensionsFormsList;
    [SerializeField]
    private Map m_map;
    [SerializeField]
    private Inventory m_inventory;
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private BuffsController m_buffsController;
    private List<GameObject> m_otherBiomesPiecesList = new List<GameObject>();
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        InitializeOtherBiomes();

        Invoke("StartResourcesLootingWithDelay", GameManager.instance.GetLootInterval(PieceType.Resource));
        Invoke("StartMaterialsLootingWithDelay", GameManager.instance.GetLootInterval(PieceType.Material));

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
        placedPiecesList.Add(Piece);

        if(Piece.type == PieceType.MaterialBuff)
        {
            GameManager.instance.SetLootInterval(PieceType.Material, GameManager.instance.GetLootInterval(PieceType.Material) * 0.9f);
        }
        else if(Piece.type == PieceType.ResourceBuff)
        {
            GameManager.instance.SetLootInterval(PieceType.Resource, GameManager.instance.GetLootInterval(PieceType.Resource) * 0.9f);
        }
        else if(Piece.type == PieceType.BiomeBuff)
        {
            m_buffsController.AddMultiplier(GetBiomeType(Piece), 1);
        }        
    }

    private BiomeType GetBiomeType(Piece Piece) 
    {
        Vector2Int Direction = Piece.worldPosition - m_map.worldPosition;
        Direction.y = -Direction.y;

        return (BiomeType)m_map.matrix[Direction.y][Direction.x];      
    }

    public void SavePiece(Piece Piece)
    {
        if(m_player.currentPiece == null)
        {
            m_player.currentPiece = Piece;
            Piece.CreatePiece();
        }
    }

    void LootResources(PieceType typeToLoot)
    {
        Debug.Log("Loot" + typeToLoot);
        if(placedPiecesList.Count > 0)
        {
            List<List<int>> MapMatrix = m_map.matrix;

            for(int i = 0; i < placedPiecesList.Count; i++) 
            {
                if (placedPiecesList[i].type == typeToLoot ) 
                {
                    Vector2Int MapPosition = placedPiecesList[i].worldPosition - m_map.worldPosition;
                    MapPosition.y = Mathf.Abs(MapPosition.y);

                    for (int j = 0; j < placedPiecesList[i].matrix.Count; j++)
                    {
                        for (int k = 0; k < placedPiecesList[i].matrix[j].Count; k++)
                        {
                            if (placedPiecesList[i].matrix[j][k] != GameManager.INVALID_TILE)
                            {
                                int value = m_map.matrix[MapPosition.y + j][MapPosition.x + k];

                                if (placedPiecesList[i].type == PieceType.Resource)
                                {
                                    ResourceType resource = (ResourceType)(value);
                                    m_inventory.AddResource((value), 1);
                                    Debug.Log($"Resource: {resource}");
                                }
                                else if (placedPiecesList[i].type == PieceType.Material)
                                {
                                    MaterialType material = (MaterialType)(value);
                                    m_inventory.AddMaterial((value), 1);
                                    Debug.Log($"Material: {material}");

                                    int RandomValue = Random.Range(0, 1500);
                                    Debug.Log(RandomValue);
                                    if (RandomValue <= RandomValueForFinalMapExtension)
                                    {
                                        m_inventory.AddFinalExtensionTile();
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

    public void EndGame()
    {

        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {

            for(int i = 0; i < finalMatrix.Count; i++)
            {
                for(int j = 0; j < finalMatrix[i].Count; j++)
                {
                    finalMatrix[i][j] = (finalMatrix[i][j] + 1) % 5;
                }
            }
            m_map.mapRender.RenderMap(finalPosition, finalMatrix);
            yield return new WaitForSeconds(1f);
        }
    }


    public bool CreateBuffPiece(PieceType piece)
    {
        if(m_player.currentPiece != null)
        {
            return false;
        }
        // Initialize CurrentPiece.Matrix using List<List<int>>
        List<List<int>> Matrix = new List<List<int>>()
                {
                    new List<int> { 1 }
                };

        GameObject tile = Instantiate(piecePrefab);
        Piece auxPiece = tile.GetComponent<Piece>();
        auxPiece.InitPiece(piece, Matrix);
        auxPiece.CreatePiece();
        m_player.currentPiece = auxPiece;

        return true;
    }
   

    IEnumerator LootResourcesRoutine()
    {
        while (true)
        {
            LootResources(PieceType.Resource);
            yield return new WaitForSeconds(GameManager.instance.GetLootInterval(PieceType.Resource));
        }
    }
    IEnumerator LootMaterialsRoutine()
    {
        while (true)
        {
            LootResources(PieceType.Material);
            yield return new WaitForSeconds(GameManager.instance.GetLootInterval(PieceType.Material));
        }
    }

    public bool IsOverlapingOtherMapPieces(Piece MapExtensionPiece)
    {
        List<Piece> pieces = new List<Piece>();

        for(int i = 0; i < m_otherBiomesPiecesList.Count; i++) 
        {
            if(m_otherBiomesPiecesList[i].GetComponent<Piece>() != MapExtensionPiece)
            {
                pieces.Add(m_otherBiomesPiecesList[i].GetComponent<Piece>());
            }
        }
        return IsPieceOverlapping(MapExtensionPiece, pieces);
    }

    public bool IsPieceOverlapping(Piece piece, List<Piece> PieceListToIterate) //pieceType 0 placed pieces / 1 map extension pieces 
    {    
        for (int i = 0; i < PieceListToIterate.Count; i++)
        {
            Piece OtherPiece = PieceListToIterate[i];
            if (OtherPiece.worldPosition.x + OtherPiece.matrix[0].Count - 1 < piece.worldPosition.x ||
               OtherPiece.worldPosition.y - OtherPiece.matrix.Count - 1 > piece.worldPosition.y ||
               piece.worldPosition.x + piece.matrix[0].Count - 1 < OtherPiece.worldPosition.x ||
               piece.worldPosition.y - piece.matrix.Count - 1 > OtherPiece.worldPosition.y)
            {
                continue;
            }

            Vector2Int Direction = piece.worldPosition - OtherPiece.worldPosition;

            for(int j = 0; j < piece.matrix.Count; j ++) 
            { 
                for(int k = 0; k < piece.matrix[j].Count; k ++)
                {
                    Vector2Int AuxPosition = Direction + new Vector2Int(k, -j);
                    if (AuxPosition.x < 0 ||
                        AuxPosition.y > 0 ||
                        AuxPosition.x >= OtherPiece.matrix[0].Count ||
                        Mathf.Abs(AuxPosition.y) >= OtherPiece.matrix.Count)
                    {
                        continue;
                    }
                    if (OtherPiece.matrix[Mathf.Abs(AuxPosition.y)][AuxPosition.x] != GameManager.INVALID_TILE && piece.matrix[j][k] != GameManager.INVALID_TILE)
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

        while (ExtensionsPlaced < m_numberOfOtherBiomes)
        {
            for (int j = 0; j < Directions.Count; j++)
            {
                if (Random.Range(0, RandomValueForNotPlacePiece) == 0)
                {
                    continue;
                }

                GameObject tile = Instantiate(piecePrefab);
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
                BiomePiece.worldPosition = WorldPosition;
                ExtensionsPlaced++;

                m_otherBiomesPiecesList.Add(tile);

                if(ExtensionsPlaced >= m_numberOfOtherBiomes)
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
                new List<int> { GameManager.INVALID_TILE, 0, GameManager.INVALID_TILE },
                new List<int> { 0, 0 , 0 },
                new List<int> { GameManager.INVALID_TILE, 0, GameManager.INVALID_TILE }
            },

            new List<List<int>>
            { 
                new List<int> { 0, 0 , 0 },
                new List<int> {0, GameManager.INVALID_TILE, GameManager.INVALID_TILE }
            },

            new List<List<int>>
            {
                new List<int> { 0, 0},
                new List<int> {0, GameManager.INVALID_TILE },
                new List<int> {0, 0}
            },

            new List<List<int>>
            {
                new List<int> {0, GameManager.INVALID_TILE, 0 },
                new List<int> { 0, 0, 0},
                new List<int> { 0, 0, 0 }
            },

            new List<List<int>>
            {
                new List<int> {0, GameManager.INVALID_TILE, GameManager.INVALID_TILE },
                new List<int> { 0, 0, GameManager.INVALID_TILE},
                new List<int> { 0, 0, 0 }
            },

            new List<List<int>>
            {
                new List<int> {0, 0 , 0 },
                new List<int> { GameManager.INVALID_TILE, 0, 0},
                new List<int> { GameManager.INVALID_TILE, GameManager.INVALID_TILE, 0 }
            },

            new List<List<int>>
            {
                new List<int> { GameManager.INVALID_TILE, 0, 0},
                new List<int> { 0, 0, 0 }
            },

            new List<List<int>>
            {
                new List<int> { 0, 0, 0},
                new List<int> { 0, GameManager.INVALID_TILE, 0 },
                new List<int> { 0, 0, 0 }
            },
        };
    }

    public void IsAdjacentToMapExtension(Piece piece)
    {
        for (int i = m_otherBiomesPiecesList.Count - 1; i >= 0; i--)
        {
            GameObject AuxGameObject = m_otherBiomesPiecesList[i];
            Piece OtherPiece = AuxGameObject.GetComponent<Piece>();
            if (m_map.CheckAdjacency(piece, OtherPiece.worldPosition, OtherPiece.matrix))
            {
                m_otherBiomesPiecesList.RemoveAt(i);
                if (m_map.ExtendMap(OtherPiece))
                {
                    Destroy(AuxGameObject);
                }
                else
                {
                    m_otherBiomesPiecesList.Add(AuxGameObject);
                }
            }
        }
    }
       
}
