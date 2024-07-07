using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVParser
{
    public static List<List<int>> ParseCSVToMatrix(string PathFile)
    {
        List<List<int>> MapMatrix = new List<List<int>>();

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

        // Fill MapMatrix with the values from the CSV file
        foreach (var line in lines)
        {
            List<int> row = new List<int>();
            foreach (var value in line)
            {
                row.Add(int.Parse(value));
            }
            MapMatrix.Add(row);
        }

        return MapMatrix;
    }

    public static void ParseMatrixToCSV(string PathFile, List<List<int>> MapMatrix)
    {
        List<string> lines = new List<string>();

        // Convert MapMatrix to lines of CSV
        foreach (var row in MapMatrix)
        {
            string line = string.Join(";", row);
            lines.Add(line);
        }

        // Write lines to the file
        using (StreamWriter writer = new StreamWriter(PathFile))
        {
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }

}
