using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject {
   
    public Material surfaceMaterial;
    public BiomeColourSettings biomeColourSettings;

    [System.Serializable]
    public class BiomeColourSettings
    {
        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0, 1)]
        public float blendAmount;
        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tintColour;
            [Range(0, 1)]
            public float biomeStartHeight;
            [Range(0, 1)]
            public float biomeTintPercent;
        }
    }
}
