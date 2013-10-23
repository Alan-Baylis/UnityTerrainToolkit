using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private static string CreateFilePath(string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        return Path.Combine(outputDirectory, Guid.NewGuid().ToString());
    }

    private static void Serialize(string filePath, IEnumerable<TextureData> data)
    {
        using (var fs = File.Create(filePath))
        {
            var serializer = new BinaryFormatter();
            serializer.Serialize(fs, data);
        }
    }

    private IEnumerable<TextureData> Deserialize(string filePath)
    {
        using (var fs = File.Open(filePath, FileMode.Open))
        {
            var deserializer = new BinaryFormatter();
            return (IEnumerable<TextureData>) deserializer.Deserialize(fs);
        }
    }

    private IEnumerable<TextureData> GetTextureMixes()
    {
        var textureInfo = new List<TextureData>();

        var terrainData = Terrain.terrainData;
        var terrainSize = terrainData.size;
        var controlTextureResoltuion = terrainData.alphamapResolution;

        for (var x = 0f; x < terrainSize.x; x += terrainSize.x/controlTextureResoltuion)
        {
            for (var z = 0f; z < terrainSize.z; z += terrainSize.z/controlTextureResoltuion)
            {
                var worldPosition = new Vector3(x, 0, z);
                var mix = GetTextureMix(worldPosition, Terrain.transform.position, terrainData);

                textureInfo.Add(new TextureData
                                {
                                    TextureMix = mix,
                                    Location = new Location<float, float>
                                               {
                                                   X = worldPosition.x,
                                                   Z = worldPosition.y
                                               },
                                });
            }
        }

        return textureInfo;
    }

    private void ApplyMixesToTerrain(IEnumerable<TextureData> data)
    {
        var terrainData = Terrain.terrainData;

        foreach (var textureData in data)
        {
            ApplyTextureMix(textureData, Terrain.transform.position, terrainData);
        }
    }

    private void ApplyTextureMix(TextureData textureData, Vector3 terrainPos, TerrainData terrainData)
    {
        var mapX = (int) (((textureData.Location.X - terrainPos.x)/terrainData.size.x)*terrainData.alphamapWidth);
        var mapZ = (int) (((textureData.Location.Z - terrainPos.z)/terrainData.size.z)*terrainData.alphamapHeight);

        var mixes = textureData.TextureMix.ToArray();
        var splatmapData = new float[1, 1, mixes.Count()];

        for (var n = 0; n < mixes.Count(); n++)
        {
            splatmapData[0, 0, n] = mixes[n];
        }

        terrainData.SetAlphamaps(mapX, mapZ, splatmapData);
    }

    //private void Start()
    //{
    //    terrain = Terrain.activeTerrain;
    //    terrainData = terrain.terrainData;
    //    terrainPos = terrain.transform.position;
    //}

    private static IEnumerable<float> GetTextureMix(Vector3 worldPos, Vector3 terrainPos, TerrainData terrainData)
    {
        // returns an array containing the relative mix of textures
        // on the main terrain at this world position.

        // The number of values in the array will equal the number
        // of textures added to the terrain.

        // calculate which splat map cell the worldPos falls within (ignoring TextureMix)
        var mapX = (int) (((worldPos.x - terrainPos.x)/terrainData.size.x)*terrainData.alphamapWidth);
        var mapZ = (int) (((worldPos.z - terrainPos.z)/terrainData.size.z)*terrainData.alphamapHeight);

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        var splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        // extract the 3D array data to a 1D array:
        var cellMix = new float[splatmapData.GetUpperBound(2) + 1];

        for (var n = 0; n < cellMix.Length; n++)
        {
            cellMix[n] = splatmapData[0, 0, n];
        }
        return cellMix;
    }
}