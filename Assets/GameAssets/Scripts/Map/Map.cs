using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Map : WorldMatrix
{
    public MapRender MapRender;
    public PieceController PieceController;

    public int InitialRows = 3;
    public int InitialColumns = 5;

    void Start()
    {
        InitializeMap();      

        // Print the matrix
        PrintMatrix();
    }
   

    private void InitializeMap()
    {
        Matrix = CSVParser.ParseCSVToMatrix("Assets/GameAssets/Scripts/Map/InitialMap.csv");
        MapRender.RenderMap();
    }

   
    public bool AddPieceToMap(Piece piece)
    {
        if(PieceController.IsPieceOverlapping(piece))
        {
            Debug.Log("There is a piece in the spot");
            return false;
        }
        // Add the piece to the map matrix
        List<List<int>> PieceMatrix = piece.Matrix;
        Vector2Int position = piece.WorldPosition;

        if( position.x < WorldPosition.x || //Left
            position.y > WorldPosition.y || //Top
            position.y - PieceMatrix.Count < WorldPosition.y - Matrix.Count || //Bottom
            position.x + PieceMatrix[0].Count > WorldPosition.x + Matrix[0].Count) //Right
            return false;

        for (int i = 0; i < PieceMatrix.Count; i++)
        {
            for (int j = 0; j < PieceMatrix[i].Count; j++)
            {
                int col = Mathf.Abs(position.x + j);
                int row = Mathf.Abs(position.y - i);

                if (Matrix[row][col] == GameplayManager.INVALID_TILE && PieceMatrix[i][j] != GameplayManager.INVALID_TILE)
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < PieceMatrix.Count; i++)
        {
            for (int j = 0; j < PieceMatrix[i].Count; j++)
            {
                int col = Mathf.Abs(position.x + j);
                int row = Mathf.Abs(position.y - i);
                
                Matrix[row][col] += PieceMatrix[i][j] * (( (int)piece.Type + 1 ) * 4);                
            }
        }

        return true;
    }

    public bool ExtendMap(Piece mapExtension)
    {
        // Add the piece to the map matrix
        List<List<int>> MapExtensionMatrix = mapExtension.Matrix;
        Vector2Int MapExtensionPosition = mapExtension.WorldPosition;

        Debug.Log("Try extend map");

        if (!CheckAdjacency(mapExtension))
        {
            return false;
        }


        Vector2Int mapTile = MapExtensionPosition - WorldPosition;

        for(int i = 0; i < MapExtensionMatrix.Count; i++)
        {
            if(MapExtensionPosition.y - i > WorldPosition.y)
            {
                continue;
            }
            if (Mathf.Abs(MapExtensionPosition.y - i) >= Matrix.Count - WorldPosition.y)
            {
                break;
            }
            for (int j = 0; j < MapExtensionMatrix[i].Count; j++)
            {
                if (MapExtensionPosition.x + j < WorldPosition.x)
                {
                    continue;
                }
                if(MapExtensionPosition.x + j >= Matrix[i].Count + WorldPosition.x)
                {
                    break;
                }
                if (Matrix[Mathf.Abs(mapTile.y - i)][mapTile.x + j] != GameplayManager.INVALID_TILE && MapExtensionMatrix[i][j] != GameplayManager.INVALID_TILE)
                {
                    return false;
                }
            }
        }

        //Add from the top
        if(MapExtensionPosition.y > WorldPosition.y)
        {
            for(int i = 0; i < mapTile.y; i++)
            {
                List<int> NewRow = Enumerable.Repeat(GameplayManager.INVALID_TILE, Matrix[0].Count).ToList();
                Matrix.Insert(0, NewRow);
                WorldPosition.y++;
            }
        }

        //Add from the bottom
        int rowsToAdd = (WorldPosition.y - Matrix.Count + 1) - (MapExtensionPosition.y - MapExtensionMatrix.Count + 1);
        if(rowsToAdd > 0) 
        {
            for (int i = 0; i < rowsToAdd; i++)
            {
                List<int> NewRow = Enumerable.Repeat(GameplayManager.INVALID_TILE, Matrix[0].Count).ToList();
                Matrix.Add(NewRow);
            }
        }

        //Add from the left
        if(mapTile.x < 0) 
        {
            
            for (int j = 0; j < Mathf.Abs(mapTile.x); j++)
            {
                for (int i = 0; i < Matrix.Count; i++)
                {
                    Matrix[i].Insert(0, GameplayManager.INVALID_TILE);
                }

                WorldPosition.x--;
            }
            
        }
        //Add from the right
        int columnsToAdd = (MapExtensionPosition.x + MapExtensionMatrix[0].Count - 1) - (WorldPosition.x + Matrix[0].Count - 1);
        if (columnsToAdd > 0)
        {
            for(int i = 0; i < Matrix.Count; i++)
            {
                for (int j = 0; j < columnsToAdd; j++)
                {
                    Matrix[i].Add(GameplayManager.INVALID_TILE);
                }
            }
            
        }

        Vector2Int NewMapTile = MapExtensionPosition - WorldPosition;

        for (int i = 0; i < MapExtensionMatrix.Count; i++)
        {
            
            for (int j = 0; j < MapExtensionMatrix[i].Count; j++)
            {

                Matrix[Mathf.Abs(NewMapTile.y - i)][NewMapTile.x + j] = MapExtensionMatrix[i][j];                
            }
        }

        MapRender.RenderMap();

        return true;
    }


    bool CheckAdjacency(Piece mapExtension)
    {
        //TODO: no funciona colocar una pieza justo en la esquina
        //hay que hacer que si el valor de un extremo donde se comprueba es -1, mover la comprobación a la siguiente tile


        // Add the piece to the map matrix
        List<List<int>> MapExtensionMatrix = mapExtension.Matrix;
        Vector2Int MapExtensionPosition = mapExtension.WorldPosition;
        int INVALID_TILE = GameplayManager.INVALID_TILE;

        // Get the dimensions of both matrices
        int mainMatrixWidth = Matrix[0].Count;
        int mainMatrixHeight = Matrix.Count;
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

                    if (tileToCheck.x >= WorldPosition.x && tileToCheck.x <= WorldPosition.x + mainMatrixWidth - 1 &&
                        tileToCheck.y <= WorldPosition.y && tileToCheck.y > WorldPosition.y - mainMatrixHeight)
                    {
                        if (Matrix[WorldPosition.y - tileToCheck.y][tileToCheck.x - WorldPosition.x] != INVALID_TILE)
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

                    if (tileToCheck.x >= WorldPosition.x && tileToCheck.x <= WorldPosition.x + mainMatrixWidth - 1 &&
                        tileToCheck.y <= WorldPosition.y && tileToCheck.y > WorldPosition.y - mainMatrixHeight)
                    {
                        if (Matrix[WorldPosition.y - tileToCheck.y][tileToCheck.x - WorldPosition.x] != INVALID_TILE)
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

                    if (tileToCheck.x >= WorldPosition.x && tileToCheck.x <= WorldPosition.x + mainMatrixWidth - 1 &&
                        tileToCheck.y <= WorldPosition.y && tileToCheck.y > WorldPosition.y - mainMatrixHeight)
                    {
                        if (Matrix[WorldPosition.y - tileToCheck.y][tileToCheck.x - WorldPosition.x] != INVALID_TILE)
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

                    if (tileToCheck.x >= WorldPosition.x && tileToCheck.x <= WorldPosition.x + mainMatrixWidth - 1 &&
                        tileToCheck.y <= WorldPosition.y && tileToCheck.y > WorldPosition.y - mainMatrixHeight)
                    {
                        if (Matrix[WorldPosition.y - tileToCheck.y][tileToCheck.x - WorldPosition.x] != INVALID_TILE)
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
