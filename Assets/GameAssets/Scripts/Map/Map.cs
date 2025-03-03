using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Map : WorldMatrix
{
    #region Variables
    public MapRender mapRender;

    [SerializeField]
    private PieceController m_pieceController;
    [SerializeField]
    private string m_mapName = "InitialMap";
    #endregion

    void Start()
    {
        InitializeMap();      

        // Print the matrix
        PrintMatrix();
    }
   

    private void InitializeMap()
    {
        CSVParser.ParseCSVToMatrix(m_mapName, out matrix);
        mapRender.RenderMap(worldPosition,matrix);
    }

   
    public bool AddPieceToMap(Piece piece)
    {
        if (m_pieceController.IsPieceOverlapping(piece, m_pieceController.placedPiecesList))
        {
            Debug.Log("There is a piece in the spot");
            return false;
        }

        // Add the piece to the map matrix
        List<List<int>> PieceMatrix = piece.matrix;
        Vector2Int position = piece.worldPosition;

        if (position.x < worldPosition.x || //Left
            position.y > worldPosition.y || //Top
            position.y - PieceMatrix.Count < worldPosition.y - matrix.Count || //Bottom
            position.x + PieceMatrix[0].Count > worldPosition.x + matrix[0].Count) //Right
            return false;

        for (int i = 0; i < PieceMatrix.Count; i++)
        {
            for (int j = 0; j < PieceMatrix[i].Count; j++)
            {
                int col = (position.x - worldPosition.x) + j;
                int row = (worldPosition.y - position.y) + i;

                if (matrix[row][col] == GameManager.INVALID_TILE && PieceMatrix[i][j] != GameManager.INVALID_TILE)
                {
                    return false;
                }
            }
        }


        

        return true;
    }

    public bool ExtendMap(Piece mapExtension)
    {
        // Add the piece to the map matrix
        List<List<int>> MapExtensionMatrix = mapExtension.matrix;
        Vector2Int MapExtensionPosition = mapExtension.worldPosition;

        Debug.Log("Try extend map");

        if (!CheckAdjacency(mapExtension))
        {
            return false;
        }


        Vector2Int mapTile = MapExtensionPosition - worldPosition;

        for(int i = 0; i < MapExtensionMatrix.Count; i++)
        {
            if(MapExtensionPosition.y - i > worldPosition.y)
            {
                continue;
            }
            if (Mathf.Abs(MapExtensionPosition.y - i) >= matrix.Count - worldPosition.y)
            {
                break;
            }
            for (int j = 0; j < MapExtensionMatrix[i].Count; j++)
            {
                if (MapExtensionPosition.x + j < worldPosition.x)
                {
                    continue;
                }
                if(MapExtensionPosition.x + j >= matrix[i].Count + worldPosition.x)
                {
                    break;
                }
                if (matrix[Mathf.Abs(mapTile.y - i)][mapTile.x + j] != GameManager.INVALID_TILE && MapExtensionMatrix[i][j] != GameManager.INVALID_TILE)
                {
                    return false;
                }
            }
        }


        if (m_pieceController.IsOverlapingOtherMapPieces(mapExtension))
        {
            return false;
        }


        //Add from the top
        if (MapExtensionPosition.y > worldPosition.y)
        {
            for(int i = 0; i < mapTile.y; i++)
            {
                List<int> NewRow = Enumerable.Repeat(GameManager.INVALID_TILE, matrix[0].Count).ToList();
                matrix.Insert(0, NewRow);
                worldPosition.y++;
            }
        }

        //Add from the bottom
        int rowsToAdd = (worldPosition.y - matrix.Count + 1) - (MapExtensionPosition.y - MapExtensionMatrix.Count + 1);
        if(rowsToAdd > 0) 
        {
            for (int i = 0; i < rowsToAdd; i++)
            {
                List<int> NewRow = Enumerable.Repeat(GameManager.INVALID_TILE, matrix[0].Count).ToList();
                matrix.Add(NewRow);
            }
        }

        //Add from the left
        if(mapTile.x < 0) 
        {
            
            for (int j = 0; j < Mathf.Abs(mapTile.x); j++)
            {
                for (int i = 0; i < matrix.Count; i++)
                {
                    matrix[i].Insert(0, GameManager.INVALID_TILE);
                }

                worldPosition.x--;
            }
            
        }
        //Add from the right
        int columnsToAdd = (MapExtensionPosition.x + MapExtensionMatrix[0].Count - 1) - (worldPosition.x + matrix[0].Count - 1);
        if (columnsToAdd > 0)
        {
            for(int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < columnsToAdd; j++)
                {
                    matrix[i].Add(GameManager.INVALID_TILE);
                }
            }
            
        }

        Vector2Int NewMapTile = MapExtensionPosition - worldPosition;

        for (int i = 0; i < MapExtensionMatrix.Count; i++)
        {
            
            for (int j = 0; j < MapExtensionMatrix[i].Count; j++)
            {
                if(matrix[Mathf.Abs(NewMapTile.y - i)][NewMapTile.x + j] == GameManager.INVALID_TILE)
                {
                    matrix[Mathf.Abs(NewMapTile.y - i)][NewMapTile.x + j] = MapExtensionMatrix[i][j];                
                }
            }
        }




        m_pieceController.IsAdjacentToMapExtension(mapExtension);

        mapRender.RenderMap(mapExtension.worldPosition, mapExtension.matrix);

        if (mapExtension.matrix[0][0] == 4)
        {
            Tutorial.instance.ShowFinalFinalTutorial();
            m_pieceController.finalMatrix = mapExtension.matrix;
            m_pieceController.finalPosition = mapExtension.worldPosition;
            GameManager.instance.gameEnded = true;
            m_pieceController.EndGame();
        }

        return true;
    }



    bool CheckAdjacency(Piece mapExtension)
    {
        return CheckAdjacency(mapExtension, worldPosition, matrix);        
    }
    
    public bool CheckAdjacency(Piece mapExtension, Vector2Int OtherPosition, List<List<int>> OtherMatrix)
    {        

        // Add the piece to the map matrix
        List<List<int>> MapExtensionMatrix = mapExtension.matrix;
        Vector2Int MapExtensionPosition = mapExtension.worldPosition;
        int INVALID_TILE = GameManager.INVALID_TILE;

        // Get the dimensions of both matrices
        int mainMatrixWidth = OtherMatrix[0].Count;
        int mainMatrixHeight = OtherMatrix.Count;
        int extensionMatrixWidth = MapExtensionMatrix[0].Count;
        int extensionMatrixHeight = MapExtensionMatrix.Count;

        // Adjacency with the top of the main matrix
        for (int i = 0; i < extensionMatrixWidth; i++)
        {
            for(int j = 0; j < extensionMatrixHeight; j++)
            {
                if (MapExtensionMatrix[extensionMatrixHeight - 1 - j][i] != INVALID_TILE)
                {
                    Vector2Int tilePosition = MapExtensionPosition + new Vector2Int(i, -(extensionMatrixHeight - 1 - j)); ;
                    Vector2Int tileToCheck = tilePosition + new Vector2Int(0, -1);

                    if (tileToCheck.x >= OtherPosition.x && tileToCheck.x <= OtherPosition.x + mainMatrixWidth - 1 &&
                        tileToCheck.y <= OtherPosition.y && tileToCheck.y > OtherPosition.y - mainMatrixHeight)
                    {
                        if (OtherMatrix[OtherPosition.y - tileToCheck.y][tileToCheck.x - OtherPosition.x] != INVALID_TILE)
                        {
                            return true;
                        }
                    }
                    break;
                }
            }
            
        }

        // Adjacency with the bottom of the main matrix
        for (int i = 0; i < extensionMatrixWidth; i++)
        {
            for( int j = 0; j < extensionMatrixHeight; j++)
            {
                if (MapExtensionMatrix[j][i] != INVALID_TILE)
                {
                    Vector2Int tilePosition = MapExtensionPosition + new Vector2Int(i, -j);
                    Vector2Int tileToCheck = tilePosition + new Vector2Int(0, 1);

                    if (tileToCheck.x >= OtherPosition.x && tileToCheck.x <= OtherPosition.x + mainMatrixWidth - 1 &&
                        tileToCheck.y <= OtherPosition.y && tileToCheck.y > OtherPosition.y - mainMatrixHeight)
                    {
                        if (OtherMatrix[OtherPosition.y - tileToCheck.y][tileToCheck.x - OtherPosition.x] != INVALID_TILE)
                        {
                            return true;
                        }
                    }
                    break;
                }
            }
            
        }

        // Adjacency with the left of the main matrix
        for (int i = 0; i < extensionMatrixHeight; i++)
        {
            for (int j = 0; j < extensionMatrixHeight; j++)
            {
                if (MapExtensionMatrix[i][extensionMatrixWidth - 1 - j] != INVALID_TILE)
                {
                    Vector2Int tilePosition = MapExtensionPosition + new Vector2Int(extensionMatrixWidth - 1 - j, -i);
                    Vector2Int tileToCheck = tilePosition + new Vector2Int(1, 0);

                    if (tileToCheck.x >= OtherPosition.x && tileToCheck.x <= OtherPosition.x + mainMatrixWidth - 1 &&
                        tileToCheck.y <= OtherPosition.y && tileToCheck.y > OtherPosition.y - mainMatrixHeight)
                    {
                        if (OtherMatrix[OtherPosition.y - tileToCheck.y][tileToCheck.x - OtherPosition.x] != INVALID_TILE)
                        {
                            return true;
                        }
                    }
                    break;
                }
            }
        }

        // Adjacency with the right of the main matrix
        for (int i = 0; i < extensionMatrixHeight; i++)
        {
            for (int j = 0; j < extensionMatrixHeight; j++)
            {
                if (MapExtensionMatrix[i][j] != INVALID_TILE)
                {
                    Vector2Int tilePosition = MapExtensionPosition + new Vector2Int(j, -i);
                    Vector2Int tileToCheck = tilePosition + new Vector2Int(-1, 0);

                    if (tileToCheck.x >= OtherPosition.x && tileToCheck.x <= OtherPosition.x + mainMatrixWidth - 1 &&
                        tileToCheck.y <= OtherPosition.y && tileToCheck.y > OtherPosition.y - mainMatrixHeight)
                    {
                        if (OtherMatrix[OtherPosition.y - tileToCheck.y][tileToCheck.x - OtherPosition.x] != INVALID_TILE)
                        {
                            return true;
                        }
                    }
                    break;
                }
            }
        }

        return false;
    }

}
