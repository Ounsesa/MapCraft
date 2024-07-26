using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class MapPreviewWindow : EditorWindow
{
    static MapPreviewWindow window;
    private List<List<int>> MapMatrix;

    private string MapFilePath = "Assets/GameAssets/GameData/";
    private string MapFileName = "InitialMap.csv";

    private bool MapSelected = false;
    private int ValueToAdd = 0;


    public static void InitWindow()
    {
        window = EditorWindow.GetWindow<MapPreviewWindow>("MapPreview");
        window.Show();
    }

    private void OnGUI()
    {
        MapSelectionGUI();
        if (MapSelected && MapMatrix != null)
        {
            TopGUI();
            ShowMapGUI();
            SaveMapGUI();
            DrawMapPreview();
        }
        if(window != null)
        {
            window.Repaint();
        }
    }

    private void MapSelectionGUI()
    {
        EditorGUILayout.BeginVertical();
        MapFilePath = EditorGUILayout.TextField("Map File Path: ", MapFilePath);
        MapFileName = EditorGUILayout.TextField("Map File Name: ", MapFileName);
        EditorGUILayout.Space();
        if (GUILayout.Button("Open Map"))
        {
            MapSelected = true;
            CSVParser.ParseCSVToMatrix(MapFilePath + MapFileName, out MapMatrix);
        }

        EditorGUILayout.EndVertical();
    }

    private void TopGUI()
    {
        int aux = ValueToAdd;
        string newValue = EditorGUILayout.TextField("ValueToAdd: ", ValueToAdd.ToString());
        if (!(int.TryParse(newValue, out ValueToAdd) || newValue == "-"))
        {
            EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Value To Add" , "Okey");
            ValueToAdd = aux;
        }


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("-"))
        {
            RemoveRow();
        }
        EditorGUILayout.LabelField("Rows: " + MapMatrix.Count);

        if (GUILayout.Button("+"))
        {
            AddRow();
        }
        if (GUILayout.Button("-"))
        {
            RemoveColumn();
        }
        EditorGUILayout.LabelField("Columns: " + MapMatrix[0].Count); 
        if (GUILayout.Button("+"))
        {
            AddColumn();
        }
        EditorGUILayout.EndHorizontal();        
    }


    private void ShowMapGUI()
    {
        for(int i = 0;  i < MapMatrix.Count; i++) 
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < MapMatrix[i].Count; j++)
            {
                int aux = MapMatrix[i][j];
                string newValue = EditorGUILayout.TextField(MapMatrix[i][j].ToString());
                int outValue = aux;
                if (!(int.TryParse(newValue, out outValue) || newValue == "-" || newValue == ""))
                {
                    EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Number Set in position (" + i + "," + j + ")", "Okey");
                    outValue = aux;
                }
                else if (outValue >= ResourcesManager.GetEnumLength<BiomeType>())
                {
                    EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Number Too High For Existing Biomes in position (" + i + "," + j + ")", "Okey");
                    outValue = aux;
                }
                else if (outValue < -1)
                {
                    EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Number Set in position (" + i + "," + j + "), Obstacle Value Is -1", "Okey");
                    outValue = aux;
                }

                MapMatrix[i][j] = outValue;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
    }

    private void SaveMapGUI()
    {
        if (GUILayout.Button("Save Map"))
        {
            CSVParser.ParseMatrixToCSV(MapFilePath + MapFileName, MapMatrix);
        }
    }

    private void AddRow()
    {
        List<int> NewRow = Enumerable.Repeat(ValueToAdd, MapMatrix[0].Count).ToList();
        MapMatrix.Add(NewRow);
    }

    private void RemoveRow()
    {
        MapMatrix.RemoveAt(MapMatrix.Count - 1);
    }

    private void AddColumn()
    {
        // Iterate through each row and add the new column value
        for (int i = 0; i < MapMatrix.Count; i++)
        {
            MapMatrix[i].Add(ValueToAdd);            
        }
    }

    private void RemoveColumn()
    {
        // Iterate through each row and remove one column
        for (int i = 0; i < MapMatrix.Count; i++)
        {
            MapMatrix[i].RemoveAt(MapMatrix[i].Count - 1);
        }

    }



    private void DrawMapPreview()
    {
        GUILayout.Label("Map Preview:");
        
        float StartX = 10f;    // Starting X position of the preview
        float TileSize = (position.width - StartX*2) / (MapMatrix[0].Count + MapMatrix.Count);  // Size of each tile in the preview
        float StartY = 150f + 30 * MapMatrix.Count;   // Starting Y position of the preview

        Handles.BeginGUI();
        for (int i = 0; i < MapMatrix.Count; i++)
        {
            for (int j = 0; j < MapMatrix[i].Count; j++)
            {
                Rect tileRect = new Rect(StartX + j * TileSize, StartY + i * TileSize, TileSize, TileSize);
                Color tileColor = GetTileColor(MapMatrix[i][j]);
                EditorGUI.DrawRect(tileRect, tileColor);
            }
        }
        Handles.EndGUI();
    }

    private Color GetTileColor(int value)
    {
        // Define your color scheme based on tile values
        switch (value)
        {
            case 0:
                return Color.green;  // Example: 0 represents grass
            case 1:
                return Color.yellow;   // Example: 1 represents a road
            case 2:
                return Color.gray;   // Example: 2 represents water
            case 3:
                return Color.cyan;
            default:
                return Color.black;  // Default color
        }
    }

}
