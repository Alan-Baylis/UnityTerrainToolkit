using System;

[Serializable]
public class TextureData
{
    public int ControlTextureResolution { get; set; }
    public float[,,] SplatMaps { get; set; }

    public void AdjustSplatMapResolution(int newResolution)
    {
        var oldExponent = (int) Math.Log(ControlTextureResolution, 2);

        var newExponent = (int) Math.Log(newResolution, 2);

        if (newExponent > oldExponent)
        {
            ExpandSplatMaps(newExponent - oldExponent);
        } else if (oldExponent > newExponent)
        {
            ShrinkSplatMaps(oldExponent - newExponent);
        }

        ControlTextureResolution = newResolution;
    }

    private void ShrinkSplatMaps(int delta)
    {
        var splatmapSizeModifier = (int) Math.Pow(2, delta);

        var newFirstCount = (SplatMaps.GetUpperBound(0) + 1) / splatmapSizeModifier;
        var newSecondCount = (SplatMaps.GetUpperBound(1) + 1) / splatmapSizeModifier;
        var thirdCount = SplatMaps.GetUpperBound(2) + 1;

        var newSplatMaps = new float[newFirstCount, newSecondCount, thirdCount];

        for (var x = 0; x < newFirstCount; x++)
        {
            for (var z = 0; z < newSecondCount; z++)
            {
// ReSharper disable SuggestUseVarKeywordEverywhere -- we want to be clear this is an integer division and we want a compile error if that changes. 
                int origX = x * splatmapSizeModifier;
                int origZ = z * splatmapSizeModifier;
// ReSharper restore SuggestUseVarKeywordEverywhere

                for (var i = 0; i < thirdCount; i++)
                {
                    newSplatMaps[x, z, i] = SplatMaps[origX, origZ, i];
                }
            }
        }

        SplatMaps = newSplatMaps;
    }

    private void ExpandSplatMaps(int delta)
    {
        var splatmapSizeModifier = (int) (Math.Pow(2, delta));

        var newFirstCount = (SplatMaps.GetUpperBound(0) + 1) * splatmapSizeModifier;
        var newSecondCount = (SplatMaps.GetUpperBound(1) + 1) * splatmapSizeModifier;
        var thirdCount = SplatMaps.GetUpperBound(2) + 1;

        var newSplatMaps = new float[newFirstCount, newSecondCount, thirdCount];

        for (var x = 0; x < newFirstCount; x++)
        {
            for (var z = 0; z < newSecondCount; z++)
            {
// ReSharper disable SuggestUseVarKeywordEverywhere -- we want to be clear this is an integer division and we want a compile error if that changes. 
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