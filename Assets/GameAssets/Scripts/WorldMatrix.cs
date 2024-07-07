using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMatrix : MonoBehaviour
{
    public List<List<int>> Matrix;
    public Vector2Int WorldPosition;

    public void PrintMatrix()
    {
        string matrixString = "\n";
        for (int i = 0; i < Matrix.Count; i++)
        {
            for (int j = 0; j < Matrix[i].Count; j++)
            {
                matrixString += Matrix[i][j] + " ";
            }
            matrixString += "\n";
        }
        Debug.Log(matrixString);
    }
}
