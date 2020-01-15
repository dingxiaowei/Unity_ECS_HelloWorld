using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct SpawnCubeComponent : IComponentData
{
    public int row;
    public int colum;
    public Entity prefab;
}
