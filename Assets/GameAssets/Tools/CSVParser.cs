using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

    public static Dictionary<CraftType, CraftCost> ParseCSVToDictionary(string pathFile)
    {
        var craftingCosts = new Dictionary<CraftType, CraftCost>();

        //This line
        using (StreamReader reader = new StreamReader(pathFile))
        {
            string line;
            bool isHeader = true;

            while ((line = reader.ReadLine()) != null)
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue; // Skip the header
                }

                string[] values = line.Split(';');

                CraftType craftType = (CraftType)System.Enum.Parse(typeof(CraftType), values[0]);
                CraftCost craftCost = new CraftCost
                {
                    CurrentCost = int.Parse(values[1]),
                    NextMultiplierCost = float.Parse(values[2]),
                    MaxMultiplierCost = float.Parse(values[3]),
                    MultiplierSubdivisions = int.Parse(values[4])
                };

                craftingCosts[craftType] = craftCost;
            }
        }

        return craftingCosts;
    }


    public static List<string> ParseCSVToTutorial(string PathFile)
    {
        List<string> result = new List<string>();

        using (StreamReader reader = new StreamReader(PathFile))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                result.Add(line);
            }
        }

        return result;
    }

}
