using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class SpawnCubeSystem : ComponentSystem
{
    private EntityManager manager;
    private bool isSpawnCompleted;
    protected override void OnCreate()
    {
        base.OnCreate();
        manager = World.Active.EntityManager;
        isSpawnCompleted = false;
    }

    protected override void OnUpdate()
    {
        if (!isSpawnCompleted)
        {
            Entities.ForEach((ref SpawnCubeComponent spawnCubeComponent) =>
            {
                var row = spawnCubeComponent.row;
                var colum = spawnCubeComponent.colum;
                using (NativeArray<Entity> entities =
                    new NativeArray<Entity>(row * colum, Allocator.Temp, NativeArrayOptions.UninitializedMemory))
                {
                    manager.Instantiate(spawnCubeComponent.prefab, entities);
                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < colum; j++)
                        {
                            int index = i + j * colum;
                            manager.SetComponentData(entities[index], new Translation()
                            {
                                Value = new float3(i, 0, j)
                            });
                        }
                    }
                }
                isSpawnCompleted = true;
            });
        }
    }
}
