using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGen 
{
    ColourSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColourSettings settings)
    {
        this.settings = settings;
        if(texture == null || texture.height != settings.biomeColourSettings.biomes.Length)
        {
            texture = new Texture2D(textureResolution, settings.biomeColourSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
        
    }

    public void UpdateHeight(PlanetHeightRange planetHeightRange)
    {
        settings.surfaceMaterial.SetVector("_planetHeightRange", new Vector4(planetHeightRange.MinHeight, planetHeightRange.MaxHeight));
            
    }
    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightProportionPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightProportionPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;

        float biomeIndex = 0;
        int numBiomes = settings.biomeColourSettings.biomes.Length;
        float blendRange = settings.biomeColourSettings.blendAmount / 2f;

        for (int i = 0; i < numBiomes; i++)
        {
            float distance = heightProportionPercent - settings.biomeColourSettings.biomes[i].biomeStartHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }
        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    public void UpdateColours()
    {
        Color[] colours = new Color[texture.width * texture.height];
        int colourIndex = 0;
        foreach(var biome in settings.biomeColourSettings.biomes)
        {
            for (int i = 0; i < textureResolution; i++)
            {
                Color gradientColour = biome.gradient.Evaluate(i / (textureResolution - 1f));
                Color tintColour = biome.tintColour;
                colours[colourIndex] = gradientColour * (1 - biome.biomeTintPercent) + tintColour * biome.biomeTintPercent;
                colourIndex++;
            }
        }
        
        texture.SetPixels(colours);
        texture.Apply();
        settings.surfaceMaterial.SetTexture("_texture", texture);
    }
}
