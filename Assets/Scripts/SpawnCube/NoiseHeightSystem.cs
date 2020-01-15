﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;

public class NoiseHeightSystem : JobComponentSystem
{
    struct TranslationNoise : IJobForEach<NoiseHeightComponent, Translation>
    {
        public float time;

        public void Execute([ReadOnly]ref NoiseHeightComponent noiseHeightComponent, ref Translation translation)
        {
            var waveFactor = noiseHeightComponent.waveFactor;
            var sampleFactor = noiseHeightComponent.sampleFactor;
            translation.Value.y =
                waveFactor * noise.snoise(new float2(time + sampleFactor * translation.Value.x, time + sampleFactor * translation.Value.z));
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new TranslationNoise()
        {
            time = Time.realtimeSinceStartup
        };
        return job.Schedule(this, inputDeps);
    }
}