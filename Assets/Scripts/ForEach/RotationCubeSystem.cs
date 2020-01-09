/***
 *
 * ECS技术HelloWorld项目演示(Unity2019版本)
 *
 * 系统
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class RotationCubeSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref RotationCubeComponent rotationSpeed, ref Rotation rotation) =>
        {
            rotation.Value = math.mul(math.normalize(rotation.Value),
                quaternion.AxisAngle(math.up(), rotationSpeed.Speed * Time.deltaTime));
        });
    }
}