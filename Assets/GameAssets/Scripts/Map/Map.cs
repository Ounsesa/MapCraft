using System.Collections;
using System.Collections.Generic;
using System.IO;
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

                if (Matrix[row][col] == -1 && PieceMatrix[i][j] != 0)
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

}
