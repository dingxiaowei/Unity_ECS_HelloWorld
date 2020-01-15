using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class SpriteWithECSTesting : MonoBehaviour
{
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Material material;

    private EntityManager entityManager;
    private void Start()
    {
        entityManager = World.Active.EntityManager;
        NativeArray<Entity> entityArray = new NativeArray<Entity>(10, Allocator.Temp);
        //Allocator是申请内存的操作，不同于New，New是申请内存并且对象构造，Allocator只是申请分配内存,Allocator参数是个枚举，值分别是Allocator.Temp、Allocator.TempJob、Allocator.Persistent。三者的区别是生命周期不同和性能差别，Temp用于一帧内即回收数据，性能最好；TempJob生命周期是4帧，通常用于Job中，性能次于Temp；Persistent可以在程序运行过程中一直在，但性能最差。NativeArray与Array另一个不同是需要手动回收内存，即调用Dispose()。

        EntityArchetype entityArcheType = entityManager.CreateArchetype(
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(Rotation),
            //typeof(Scale),
            typeof(NonUniformScale)
            );
        entityManager.CreateEntity(entityArcheType, entityArray);
        foreach (var entity in entityArray)
        {
            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = mesh,
                material = material,
            });

            entityManager.SetComponentData(entity, new NonUniformScale
            {
                Value = new float3(1f, 3f, 1f)
            });
        }
        entityArray.Dispose(); //回收内存 对应上面开辟的内存 C++层调用的free
    }
}

[BurstCompile]
public class MoveSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation translation) =>
        {
            float moveSpeed = 1f;
            translation.Value.y += moveSpeed * Time.deltaTime;
        });
    }
}

[BurstCompile]
public class RotatorSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Rotation rotation) =>
        {
            rotation.Value = quaternion.Euler(0, 0, math.PI * Time.realtimeSinceStartup);
        });
    }
}

[BurstCompile]
public class ScalersSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Scale scale) =>
        {
            scale.Value += 1f * Time.deltaTime;
        });
    }
}