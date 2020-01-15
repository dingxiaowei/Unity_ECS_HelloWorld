using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Burst;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class SpawCubeSystem : JobComponentSystem
{
    private BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    struct SpawnCubeJob : IJobForEachWithEntity<SpawnCubeComponent, LocalToWorld>
    {
        public EntityCommandBuffer.Concurrent CommandBuffer;
        public void Execute(Entity entity, int index, [ReadOnly]ref SpawnCubeComponent spawnCubeComponent, [ReadOnly]ref LocalToWorld location)
        {
            for (var i = 0; i < spawnCubeComponent.row; i++)
            {
                for (int j = 0; j < spawnCubeComponent.colum; j++)
                {
                    var instance = CommandBuffer.Instantiate(index, spawnCubeComponent.prefab);
                    CommandBuffer.SetComponent(index, instance, new Translation()
                    {
                        Value = new float3(i, 0, j)
                    });
                }
            }
            CommandBuffer.DestroyEntity(index, entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new SpawnCubeJob()
        {
            CommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);

        m_EntityCommandBufferSystem.AddJobHandleForProducer(job);
        return job;
    }
}
