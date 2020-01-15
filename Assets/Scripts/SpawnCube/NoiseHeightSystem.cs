using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class NoiseHeightSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var time = Time.realtimeSinceStartup;
        Entities.ForEach((ref Translation translation, ref NoiseHeightComponent noiseHeightComponent) =>
        {
            var waveFactor = noiseHeightComponent.waveFactor;
            var sampleFactor = noiseHeightComponent.sampleFactor;

            translation.Value.y = waveFactor * noise.snoise(new float2(time + sampleFactor * translation.Value.x,
                                      time + sampleFactor * translation.Value.z));
        });

    }
}