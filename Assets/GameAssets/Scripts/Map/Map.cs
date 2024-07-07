using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapRender MapRender;
    public int[][] MapMatrix;
    public Vector2 MapPosition = new Vector2(0, 0);

    public int InitialRows = 3;
    public int InitialColumns = 5;

    void Start()
    {
        InitializeMap();      

        // Print the matrix
        PrintMatrix();


    }
    void PrintMatrix()
    {
        for (int i = 0; i < MapMatrix.Length; i++)
        {
            string row = "";
            for (int j = 0; j < MapMatrix[i].Length; j++)
            {
                row += MapMatrix[i][j] + " ";
            }
            Debug.Log(row);
        }
    }

    private void InitializeMap()
    {
        MapMatrix = CSVParser.ParseCSVToMatrix("Assets/GameAssets/Scripts/Map/InitialMap.csv");
        MapRender.RenderMap();
    }

   
    public bool AddPieceToMap(Piece piece)
    {
        // Add the piece to the map matrix
        int[][] pieceMatrix = piece.PieceMatrix;
        Vector2Int position = piece.Position;

        if( position.x < MapPosition.x || //Left
            position.y > MapPosition.y || //Top
            position.y - pieceMatrix.Length < MapPosition.y - MapMatrix.Length || //Bottom
            position.x + pieceMatrix[0].Length > MapPosition.x + MapMatrix[0].Length) //Right
            return false;

        for (int i = 0; i < pieceMatrix.Length; i++)
        {
            for (int j = 0; j < pieceMatrix[i].Length; j++)
            {
                int col = Mathf.Abs(position.x + j);
                int row = Mathf.Abs(position.y - i);

                if (MapMatrix[row][col] == -1 && pieceMatrix[i][j] != 0)
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < pieceMatrix.Length; i++)
        {
            for (int j = 0; j < pieceMatrix[i].Length; j++)
            {
                int col = Mathf.Abs(position.x + j);
                int row = Mathf.Abs(position.y - i);
                
                MapMatrix[row][col] += pieceMatrix[i][j] * (( (int)piece.Type + 1 ) * 4);                
            }
        }

        return true;
    }

}
