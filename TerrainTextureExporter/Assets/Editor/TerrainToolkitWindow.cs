using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainToolkitWindow : EditorWindow
{
    public string TextureOutputPath { get; set; }
    public string TextureInputPath { get; set; }

    private Dictionary<Vector3, IEnumerable<float>> _textureInfo;

    private readonly TerrainTextureToolkit _textureToolkit = new TerrainTextureToolkit
                                                             {
                                                                 Terrain = Terrain.activeTerrain
                                                             };

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
            TextureOutputPath = EditorGUILayout.TextField("Output Directory", TextureOutputPath);

            SimpleButtonHandler("Export Texture Info", () => TextureInputPath = _textureToolkit.SerializeTexturesToFile(TextureOutputPath));

            GUILayout.Label("Texture Importer", EditorStyles.boldLabel);

            TextureInputPath = EditorGUILayout.TextField("Input Directory", TextureInputPath);

            SimpleButtonHandler("Import Texture Info", () => _textureToolkit.LoadTexturesFromFile(TextureInputPath));
        } catch (Exception e)
        {
            Debug.Log("something happened: " + e);
        }
    }

    private static void SimpleButtonHandler(string exportTextureInfo, Action action)
    {
        if (GUILayout.Button(exportTextureInfo))
        {
            action();
        }
    }
}