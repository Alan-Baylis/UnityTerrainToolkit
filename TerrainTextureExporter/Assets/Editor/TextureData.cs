using System;
using System.Collections.Generic;

[Serializable]
public class TextureData
{
    public Location<float, float> Location { get; set; }
    public IEnumerable<float> TextureMix { get; set; }
}