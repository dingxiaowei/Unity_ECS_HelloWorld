using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct NoiseHeightComponent : IComponentData
{
    public float waveFactor;
    public float sampleFactor;
}
