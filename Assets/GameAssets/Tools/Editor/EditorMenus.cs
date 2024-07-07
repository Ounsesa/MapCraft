using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorMenus
{
    [MenuItem("Tools/Map Preview")]
    public static void OpenMapPreview()
    {
        MapPreviewWindow.InitWindow();
    }
}
