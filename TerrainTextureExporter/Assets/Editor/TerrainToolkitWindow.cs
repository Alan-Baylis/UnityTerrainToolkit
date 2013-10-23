using System;
using System.Globalization;
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
            Debug.Log(e);
        }
    }

    private void ChangeResolution(string newControlTextureResolution)
    {
        if (Terrain.activeTerrain == null)
        {
            Debug.Log("No active terrain");
            return;
        }

        int newResolution;
        if (!int.TryParse(newControlTextureResolution, out newResolution))
        {
            Debug.Log("New Control Texture Resolution must be an int");
            return;
        }
        var power = Math.Log(newResolution, 2);
        newResolution = (int) Math.Pow(2, Math.Round(power));
        newResolution = Math.Min(newResolution, 2048);
        newResolution = Math.Max(newResolution, 16);

        NewControlTextureResolution = newResolution.ToString(CultureInfo.InvariantCulture);

        TerrainTextureToolkit.UpdateControlTextureResolution(Terrain.activeTerrain.terrainData, newResolution);
    }

    private static void SimpleButtonHandler(string exportTextureInfo, Action action)
    {
        if (GUILayout.Button(exportTextureInfo))
        {
            action();
        }
    }
}