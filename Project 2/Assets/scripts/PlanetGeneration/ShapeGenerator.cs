using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    public PlanetHeightRange planetHeightRange;
    private int counter = 0;
    public List<Vector3> validSpawnPoints = new List<Vector3>();


    public void UpdateSettings(ShapeSettings settings)
    {
        
        this.settings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        planetHeightRange = new PlanetHeightRange();

    }
    
    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;
        Vector3 output;
        Vector3 comparison = new Vector3(1, 1, 1);

        if (noiseFilters.Length > 0)
        {
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (settings.noiseLayers[0].enabled)
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        elevation = settings.planetRadius * (1 + elevation);
        planetHeightRange.AddValue(elevation);
        output = pointOnUnitSphere * elevation;
        
        if(elevation == settings.planetRadius && (Random.Range(0f, 1f) < 0.005))
        {
            counter++;
            validSpawnPoints.Add(output);
        }
        return output;
    }
}
