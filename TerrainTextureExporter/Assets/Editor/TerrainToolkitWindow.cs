using System;
using UnityEditor;
using UnityEngine;

public class TerrainToolkitWindow : EditorWindow
{
    public string NewControlTextureResolution { get; set; }

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Terrain Toolkit")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof (TerrainToolkitWindow));
    }

    private void OnGUI()
    {
        try
        {
            GUILayout.Label("Texture Exporter", EditorStyles.boldLabel);

            NewControlTextureResolution = EditorGUILayout.TextField("New Control Texture Resolution", NewControlTextureResolution);

            SimpleButtonHandler("Change", () => ChangeResolution(NewControlTextureResolution));
        } catch (Exception e)
        {
            Debug.Log("something happened: " + e);
        }
    }

    private static void ChangeResolution(string newControlTextureResolution)
    {
        if (Terrain.activeTerrain == null)
        {
            Debug.Log("No active terrain");
            return;
        }

        int newTextureResolution;
        if (!int.TryParse(newControlTextureResolution, out newTextureResolution))
        {
            Debug.Log("New Control Texture Resolution must be an int");
            return;
        }

        TerrainTextureToolkit.UpdateControlTextureResolution(Terrain.activeTerrain.terrainData, newTextureResolution);
    }

    private static void SimpleButtonHandler(string exportTextureInfo, Action action)
    {
        if (GUILayout.Button(exportTextureInfo))
        {
            action();
        }
    }
}