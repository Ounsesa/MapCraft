using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CSVParser
{
    public static void ParseCSVToMatrix(string PathFile, out List<List<int>> IntMatrix)
    {
        // Initialize the out parameter
        IntMatrix = new List<List<int>>();

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
            IntMatrix.Add(row);
        }
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


    public static void ParseCSVToMatrix(string PathFile, out List<List<string>> IntMatrix)
    {
        // Initialize the out parameter
        IntMatrix = new List<List<string>>();

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
            List<string> row = new List<string>();
            foreach (var value in line)
            {
                row.Add(value);
            }
            IntMatrix.Add(row);
        }
    }

    public static void ParseMatrixToCSV(string PathFile, List<List<string>> MapMatrix)
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
