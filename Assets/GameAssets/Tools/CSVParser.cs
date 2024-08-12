using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CSVParser
{
    public static void ParseCSVToMatrix(string resourceName, out List<List<int>> IntMatrix)
    {
        // Initialize the out parameter
        IntMatrix = new List<List<int>>();

        // Load the CSV file from the Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);

        // Check if the file was successfully loaded
        if (csvFile == null)
        {
            Debug.LogError($"Failed to load resource: {resourceName}");
            return;
        }

        // Read the CSV content and split it into lines
        List<string[]> lines = new List<string[]>();
        using (StringReader reader = new StringReader(csvFile.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(';');
                lines.Add(values);
            }
        }

        // Fill IntMatrix with the values from the CSV file
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

    public static void ParseMatrixToCSV(string fileName, List<List<int>> MapMatrix)
    {
        List<string> lines = new List<string>();

        // Convert MapMatrix to lines of CSV
        foreach (var row in MapMatrix)
        {
            string line = string.Join(";", row);
            lines.Add(line);
        }

        // Define a path to write to (outside of Resources)
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Write lines to the file
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }

        Debug.Log($"File saved to: {filePath}");
    }


    public static Dictionary<CraftType, CraftCost> ParseCSVToDictionary(string resourceName)
    {
        var craftingCosts = new Dictionary<CraftType, CraftCost>();

        // Load the CSV file from the Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);

        // Check if the file was successfully loaded
        if (csvFile == null)
        {
            Debug.LogError($"Failed to load resource: {resourceName}");
            return craftingCosts;
        }

        // Use a StringReader to read the CSV content line by line
        using (StringReader reader = new StringReader(csvFile.text))
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

    public static List<string> ParseCSVToTutorial(string resourceName)
    {
        List<string> result = new List<string>();

        // Load the CSV file from the Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>(resourceName);

        // Check if the file was successfully loaded
        if (csvFile == null)
        {
            Debug.LogError($"Failed to load resource: {resourceName}");
            return result;
        }

        // Use a StringReader to read the CSV content line by line
        using (StringReader reader = new StringReader(csvFile.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Debug.Log(line);
                result.Add(line);
            }
        }

        return result;
    }

}
