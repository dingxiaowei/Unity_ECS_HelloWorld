/***
 *
 * ECS技术HelloWorld项目演示(Unity2019版本)
 *
 * 组件
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;

public struct RotationCubeComponent : IComponentData
{
    //实体旋转的速度
    public float Speed;
}
