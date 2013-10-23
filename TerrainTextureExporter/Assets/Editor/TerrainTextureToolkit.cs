using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TerrainTextureToolkit
{
    public Terrain Terrain { get; set; }

    public void LoadTexturesFromFile(string textureInputPath)
    {
        if (!File.Exists(textureInputPath))
        {
            return;
        }

        var data = Deserialize(textureInputPath);

        ApplyMixesToTerrain(data);
    }

    public string SerializeTexturesToFile(string outputDirectory)
    {
        var data = GetTextureMixes();

        var filePath = CreateFilePath(outputDirectory);

        Serialize(filePath, data);

        return filePath;
    }

    private float[,,] GetTextureMixes()
    {
        var terrainData = Terrain.terrainData;

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        return terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
    }

    private static string CreateFilePath(string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        return Path.Combine(outputDirectory, Guid.NewGuid().ToString());
    }

    private static void Serialize(string filePath, float[,,] data)
    {
        using (var fs = File.Create(filePath))
        {
            var serializer = new BinaryFormatter();
            serializer.Serialize(fs, data);
        }
    }

    private float[,,] Deserialize(string filePath)
    {
        using (var fs = File.Open(filePath, FileMode.Open))
        {
            var deserializer = new BinaryFormatter();
            return (float[,,]) deserializer.Deserialize(fs);
        }
    }

    private void ApplyMixesToTerrain(float[,,] data)
    {
        var terrainData = Terrain.terrainData;

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        terrainData.SetAlphamaps(0, 0, data);
    }
}