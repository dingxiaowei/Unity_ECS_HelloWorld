/***
 *
 * ECS技术HelloWorld项目演示(Unity2019版本)
 *
 * 实体管理体系(所属类型:"混合ECS"开发模式)
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities;

public class EntitiesManager : MonoBehaviour, IConvertGameObjectToEntity
{
    //Cube旋转速度
    public float FloCubeSpeed = 10f;
    /// <summary>
    /// 把GameObject转为实体(Entity)
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="dstManager"></param>
    /// <param name="conversionSystem"></param>
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        //创建一个"组件"
        var data = new RotationCubeComponent { Speed = FloCubeSpeed };
        //将组件加入EntityManager中，让Unity内置的环境实体管理器，进行管理
        dstManager.AddComponentData(entity,data);
    }
}
