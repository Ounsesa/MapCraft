using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceType Type;
    public int[][] PieceMatrix;
    public Vector2Int Position;
    public GameObject PiecePrefab;
    List<GameObject> Tiles = new List<GameObject>();

    public void CreatePiece()
    {
        for (int i = 0; i < PieceMatrix.Length; i++)
        {
            for (int j = 0; j < PieceMatrix[i].Length; j++)
            {
                if (PieceMatrix[i][j] == 1)
                {
                    Vector3 Position = new Vector3(this.Position.x + j, this.Position.y - i, 0);
                    GameObject tile = Instantiate(PiecePrefab, Position, Quaternion.identity);
                    tile.transform.SetParent(this.transform);
                    Tiles.Add(tile);
                }
            }
        }
        PrintPiece();

    }

    public void RotatePiece(bool Direction)
    {
        if (Direction)
        {
            int[][] rotatedPiece = new int[PieceMatrix[0].Length][];
            for (int i = 0; i < rotatedPiece.Length; i++)
            {
                rotatedPiece[i] = new int[PieceMatrix.Length];
            }

            for (int i = 0; i < PieceMatrix.Length; i++)
            {
                for (int j = 0; j < PieceMatrix[i].Length; j++)
                {
                    rotatedPiece[j][PieceMatrix.Length - 1 - i] = PieceMatrix[i][j];
                }
            }

            PieceMatrix = rotatedPiece;
        }
        else
        {
            int[][] rotatedPiece = new int[PieceMatrix[0].Length][];
            for (int i = 0; i < rotatedPiece.Length; i++)
            {
                rotatedPiece[i] = new int[PieceMatrix.Length];
            }

            for (int i = 0; i < PieceMatrix.Length; i++)
            {
                for (int j = 0; j < PieceMatrix[i].Length; j++)
                {
                    rotatedPiece[PieceMatrix[0].Length - 1 - j][i] = PieceMatrix[i][j];
                }
            }

            PieceMatrix = rotatedPiece;
        }
        RecreatePiece();
    }

    private void Update()
    {
        transform.position = new Vector3(Position.x, Position.y, 0);
    }

    public void RecreatePiece()
    {
        foreach (GameObject tile in Tiles)
        {
            Destroy(tile);
        }
        Tiles.Clear();
        CreatePiece();
    }

    private void PrintPiece()
    {
        string pieceString = "";
        for (int i = 0; i < PieceMatrix.Length; i++)
        {
            for (int j = 0; j < PieceMatrix[i].Length; j++)
            {
                pieceString += PieceMatrix[i][j] + " ";
            }
            pieceString += "\n";
        }
        Debug.Log(pieceString);
    }



}
