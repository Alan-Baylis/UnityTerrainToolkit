using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class TerrainTextureToolkit
{
    public static void UpdateControlTextureResolution(TerrainData terrainData, int newResolution)
    {
        var existingData = GetTerrainTextures(terrainData);

        terrainData.alphamapResolution = newResolution;

        ApplyTexturesToNewTerrain(terrainData, existingData);
    }

    private static TextureData GetTerrainTextures(TerrainData terrainData)
    {
        return new TextureData
               {
                   SplatMaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight),
                   ControlTextureResolution = terrainData.alphamapResolution
               };
    }


    private static void ApplyTexturesToNewTerrain(TerrainData terrainData, TextureData data)
    {
        if (data.ControlTextureResolution != terrainData.alphamapResolution)
        {
            data.AdjustSplatMapResolution(terrainData.alphamapResolution);
        }

        terrainData.SetAlphamaps(0, 0, data.SplatMaps);
    }
}