using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[System.Serializable]
[RequiresEntityConversion]
[RequireComponent(typeof(ConvertToEntity))]
public class NoiseHeightDataSource : MonoBehaviour, IConvertGameObjectToEntity
{
    [Header("NoiseHeightSample param")]
    public float waveFactor;
    public float sampleFactor;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var noiseHeightData = new NoiseHeightComponent()
        {
            waveFactor = this.waveFactor,
            sampleFactor = this.sampleFactor
        };
        dstManager.AddComponentData(entity, noiseHeightData);
    }
}
