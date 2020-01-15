using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[System.Serializable]
[RequireComponent(typeof(ConvertToEntity))]
[RequiresEntityConversion]
public class SpawnCubeDataSource : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    [Header("SpawnCubeSample param")]
    public int row;
    public int colum;
    public GameObject prefab;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnCubeData = new SpawnCubeComponent()
        {
            prefab = conversionSystem.GetPrimaryEntity(this.prefab),
            row = this.row,
            colum = this.colum
        };
        dstManager.AddComponentData(entity, spawnCubeData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefab);
    }
}
