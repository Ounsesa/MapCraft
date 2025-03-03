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
    #region Variables
    [HideInInspector]
    public PieceType type;

    [SerializeField]
    private GameObject m_piecePrefab;    
    private List<GameObject> m_tiles = new List<GameObject>();
    #endregion

    public void InitPiece(PieceType Type, List<List<int>> Matrix)
    {
        this.type = Type;
        worldPosition = new Vector2Int(0, 0);
        this.matrix = Matrix;
    }
    public void CreatePiece()
    {
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                if (matrix[i][j] != GameManager.INVALID_TILE)
                {
                    Vector3 WorldPosition = new Vector3(this.worldPosition.x + j, this.worldPosition.y - i, 0);
                    GameObject tile = Instantiate(m_piecePrefab, WorldPosition, Quaternion.identity);
                    tile.transform.SetParent(transform);
                    if(type == PieceType.Resource)
                    {
                        tile.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else if(type == PieceType.Material)
                    {
                        tile.GetComponent<SpriteRenderer>().color = Color.black;
                    }
                    else if(type == PieceType.MapExtension)
                    {
                        switch(matrix[i][j])
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
                            case 4:
                                tile.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f); ;
                                break;
                        }
                    }
                    else if(type == PieceType.BiomeBuff)
                    {
                        switch (matrix[i][j])
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
                    else if(type == PieceType.MaterialBuff) 
                    { 
                        tile.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else if(type == PieceType.ResourceBuff)
                    {
                        tile.GetComponent<SpriteRenderer>().color = Color.magenta;
                    }
                    m_tiles.Add(tile);                    
                }
            }
        }
        PrintMatrix();

    }

    public void RotatePiece(bool Direction)
    {
        if (Direction)
        {
            List<List<int>> RotatedPiece = new List<List<int>>();
            for (int i = 0; i < matrix[0].Count; i++)
            {
                RotatedPiece.Add(new List<int>(new int[matrix.Count]));
            }

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    RotatedPiece[j][matrix.Count - 1 - i] = matrix[i][j];
                }
            }

            matrix = RotatedPiece;
        }
        else
        {
            List<List<int>> RotatedPiece = new List<List<int>>();
            for (int i = 0; i < matrix[0].Count; i++)
            {
                RotatedPiece.Add(new List<int>(new int[matrix.Count]));
            }

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    RotatedPiece[matrix[0].Count - 1 - j][i] = matrix[i][j];
                }
            }

            matrix = RotatedPiece;
        }
        RecreatePiece();
    }


    private void Update()
    {
        transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
    }

    public void RecreatePiece()
    {
        foreach (GameObject tile in m_tiles)
        {
            Destroy(tile);
        }
        m_tiles.Clear();
        CreatePiece();
    }




}
