using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    Material,
    Resource,
    MapExtension,
    MaterialBuff,
    ResourceBuff,
    BiomeBuff
}

public class Piece : WorldMatrix
{
    public PieceType Type;
    public GameObject PiecePrefab;
    List<GameObject> Tiles = new List<GameObject>();    

    public void InitPiece(PieceType Type, List<List<int>> Matrix)
    {
        this.Type = Type;
        WorldPosition = new Vector2Int(0, 0);
        this.Matrix = Matrix;
    }
    public void CreatePiece()
    {
        for (int i = 0; i < Matrix.Count; i++)
        {
            for (int j = 0; j < Matrix[i].Count; j++)
            {
                if (Matrix[i][j] != GameManager.Instance.INVALID_TILE)
                {
                    Vector3 WorldPosition = new Vector3(this.WorldPosition.x + j, this.WorldPosition.y - i, 0);
                    GameObject tile = Instantiate(PiecePrefab, WorldPosition, Quaternion.identity);
                    tile.transform.SetParent(transform);
                    if(Type == PieceType.Resource)
                    {
                        tile.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else if(Type == PieceType.Material)
                    {
                        tile.GetComponent<SpriteRenderer>().color = Color.black;
                    }
                    else if(Type == PieceType.MapExtension)
                    {
                        switch(Matrix[i][j])
                        {
                            case 0:
                                tile.GetComponent<SpriteRenderer>().color = Color.green;
                                break;
                            case 1:
                                tile.GetComponent<SpriteRenderer>().color = Color.yellow;
                                break;
                            case 2:
                                tile.GetComponent<SpriteRenderer>().color = Color.grey;
                                break;
                            case 3:
                                tile.GetComponent<SpriteRenderer>().color = Color.blue;
                                break;
                        }
                    }
                    else if(Type == PieceType.BiomeBuff)
                    {
                        switch (Matrix[i][j])
                        {
                            case 0:
                                tile.GetComponent<SpriteRenderer>().color = Color.green;
                                break;
                            case 1:
                                tile.GetComponent<SpriteRenderer>().color = Color.yellow;
                                break;
                            case 2:
                                tile.GetComponent<SpriteRenderer>().color = Color.grey;
                                break;
                            case 3:
                                tile.GetComponent<SpriteRenderer>().color = Color.blue;
                                break;
                        }
                    }
                    else if(Type == PieceType.MaterialBuff) 
                    { 
                        tile.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else if(Type == PieceType.ResourceBuff)
                    {
                        tile.GetComponent<SpriteRenderer>().color = Color.magenta;
                    }
                    Tiles.Add(tile);                    
                }
            }
        }
        PrintMatrix();

    }

    public void RotatePiece(bool Direction)
    {
        if (Direction)
        {
            List<List<int>> rotatedPiece = new List<List<int>>();
            for (int i = 0; i < Matrix[0].Count; i++)
            {
                rotatedPiece.Add(new List<int>(new int[Matrix.Count]));
            }

            for (int i = 0; i < Matrix.Count; i++)
            {
                for (int j = 0; j < Matrix[i].Count; j++)
                {
                    rotatedPiece[j][Matrix.Count - 1 - i] = Matrix[i][j];
                }
            }

            Matrix = rotatedPiece;
        }
        else
        {
            List<List<int>> rotatedPiece = new List<List<int>>();
            for (int i = 0; i < Matrix[0].Count; i++)
            {
                rotatedPiece.Add(new List<int>(new int[Matrix.Count]));
            }

            for (int i = 0; i < Matrix.Count; i++)
            {
                for (int j = 0; j < Matrix[i].Count; j++)
                {
                    rotatedPiece[Matrix[0].Count - 1 - j][i] = Matrix[i][j];
                }
            }

            Matrix = rotatedPiece;
        }
        RecreatePiece();
    }


    private void Update()
    {
        transform.position = new Vector3(WorldPosition.x, WorldPosition.y, 0);
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




}
