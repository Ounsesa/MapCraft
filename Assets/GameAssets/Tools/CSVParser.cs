using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVParser
{
    public static int[][] ParseCSVToMatrix(string PathFile)
    {
        int[][] MapMatrix;

        // Read the entire file to determine the number of rows and columns
        List<string[]> lines = new List<string[]>();
        using (StreamReader reader = new StreamReader(PathFile))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(';');
                lines.Add(values);
            }
        }

        // Initialize the MapMatrix based on the number of rows and columns
        int numRows = lines.Count;
        int numCols = lines[0].Length;
        MapMatrix = new int[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            MapMatrix[i] = new int[numCols];
        }

        // Fill MapMatrix with the values from the CSV file
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                MapMatrix[i][j] = int.Parse(lines[i][j]);
            }
        }

        return MapMatrix;
    }

    public static void ParseMatrixToCSV(string PathFile, int[][] MapMatrix)
    {
        string[] lines = new string[MapMatrix.Length];
        // Fill MapMatrix with the values from the CSV file
        for (int i = 0; i < MapMatrix.Length; i++)
        {
            for (int j = 0; j < MapMatrix[0].Length; j++)
            {
                lines[i] += MapMatrix[i][j] + ";";
            }
            lines[i] = lines[i].TrimEnd(';');
        }

        using (StreamWriter writer = new StreamWriter(PathFile)) 
        { 
            for(int i = 0; i < lines.Length; i++)
            {
                writer.WriteLine(lines[i]);
            }
        }
    }
}
