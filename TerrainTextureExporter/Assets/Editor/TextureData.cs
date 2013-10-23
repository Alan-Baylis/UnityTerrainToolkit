using System;

[Serializable]
public class TextureData
{
    public int ControlTextureResolution { get; set; }
    public float[,,] SplatMaps { get; set; }

    public void AdjustSplatMapResolution(int newResolution)
    {
        var oldExponent = Math.Log(ControlTextureResolution, 2);

        TerrainToolkitWindow.Log("old exponent = " + oldExponent);

        var newExponent = Math.Log(newResolution, 2);

        TerrainToolkitWindow.Log("new exponent = " + newExponent);

        if (newExponent > oldExponent)
        {
            ExpandSplatMaps(newExponent, oldExponent);
        }

        ControlTextureResolution = newResolution;
    }

    private void ExpandSplatMaps(double newExponent, double oldExponent)
    {
        var delta = newExponent - oldExponent;
        var splatmapSizeModifier = (int) (Math.Pow(2, delta));

        var newFirstCount = (SplatMaps.GetUpperBound(0) + 1) * splatmapSizeModifier;
        var newSecondCount = (SplatMaps.GetUpperBound(1) + 1) * splatmapSizeModifier;
        var thirdCount = SplatMaps.GetUpperBound(2) + 1;

        var newSplatMaps = new float[newFirstCount, newSecondCount, thirdCount];

        for (var x = 0; x < newFirstCount; x++)
        {
            for (var z = 0; z < newSecondCount; z++)
            {
// ReSharper disable SuggestUseVarKeywordEverywhere
                int origX = x / splatmapSizeModifier;
                int origZ = z / splatmapSizeModifier;
// ReSharper restore SuggestUseVarKeywordEverywhere

                for (var i = 0; i < thirdCount; i++)
                {
                    newSplatMaps[x, z, i] = SplatMaps[origX, origZ, i];
                }
            }
        }

        SplatMaps = newSplatMaps;
    }
}