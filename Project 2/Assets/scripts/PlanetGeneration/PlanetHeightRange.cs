using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetHeightRange
{
    public float MinHeight { get; private set; }
    public float MaxHeight { get; private set; }

    public PlanetHeightRange()
    {
        MinHeight = float.MaxValue;
        MaxHeight = float.MinValue;
    }

    public void AddValue(float input)
    {
        if (input > MaxHeight)
        {
            MaxHeight = input;
        }
        if (input < MinHeight)
        {
            MinHeight = input;
        }
    }
}
