using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Map : WorldMatrix
{
    public MapRender MapRender;

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

                if (Matrix[row][col] == -1 && PieceMatrix[i][j] != -1)
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

        //If the extension is not adyacent to the map
        if (MapExtensionPosition.x > (WorldPosition.x + Matrix[0].Count) || //Right
            MapExtensionPosition.y < (WorldPosition.y - Matrix.Count) || //Bottom
            (MapExtensionPosition.y - MapExtensionMatrix.Count) > (WorldPosition.y) || //Top
            (MapExtensionPosition.x + MapExtensionMatrix[0].Count) < (WorldPosition.x)) //Left
            return false;

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
                if (Matrix[Mathf.Abs(mapTile.y - i)][mapTile.x + j] != -1 && MapExtensionMatrix[i][j] != -1)
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
                List<int> NewRow = Enumerable.Repeat(-1, Matrix[0].Count).ToList();
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
                List<int> NewRow = Enumerable.Repeat(-1, Matrix[0].Count).ToList();
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
                    Matrix[i].Insert(0, -1);
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
                    Matrix[i].Add(-1);
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

}
