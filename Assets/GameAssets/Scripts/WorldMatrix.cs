using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMatrix : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public List<List<int>> matrix;
    [HideInInspector]
    public Vector2Int worldPosition;
    #endregion

    public void PrintMatrix()
    {
        string MatrixString = "\n";
        for (int i = 0; i < matrix.Count; i++)
        {
            for (int j = 0; j < matrix[i].Count; j++)
            {
                MatrixString += matrix[i][j] + " ";
            }
            MatrixString += "\n";
        }
        Debug.Log(MatrixString);
    }
}
