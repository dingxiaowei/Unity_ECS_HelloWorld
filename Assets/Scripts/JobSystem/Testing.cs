using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using UnityEngine.Jobs;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private bool useJobs;
    [SerializeField]
    private int count = 10;
    [SerializeField]
    private int zombieCount = 1000;
    [SerializeField]
    private Transform pfZombie;
    private List<Zombie> zombieList;

    public class Zombie
    {
        public Transform transform;
        public float moveY;
    }

    private void Start()
    {
        zombieList = new List<Zombie>();
        for (int i = 0; i < zombieCount; i++)
        {
            Transform zombieTransform = Instantiate(pfZombie, new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-5f, 5f)), Quaternion.identity);
            zombieList.Add(new Zombie
            {
                transform = zombieTransform,
                moveY = UnityEngine.Random.Range(1f, 2f)
            });
        }
    }

    void Update()
    {
        float startTime = Time.realtimeSinceStartup;
        if (useJobs)
        {
            //NativeArray<float3> positionArray = new NativeArray<float3>(zombieList.Count, Allocator.TempJob);
            NativeArray<float> moveYArray = new NativeArray<float>(zombieList.Count, Allocator.TempJob);
            TransformAccessArray transformAccessArray = new TransformAccessArray(zombieList.Count);
            for (int i = 0; i < zombieList.Count; i++)
            {
                //positionArray[i] = zombieList[i].transform.position;
                moveYArray[i] = zombieList[i].moveY;
                transformAccessArray.Add(zombieList[i].transform);
            }

            /*
            var reallyToughParallelJob = new ReallyTouchParallelJob
            {
                deltime = Time.deltaTime,
                positionArray = positionArray,
                moveYArray = moveYArray
            };
            */

            var reallyToughParallelJobTransforms = new ReallyToughParllelJobTransforms
            {
                deltime = Time.deltaTime,
                moveYArray = moveYArray
            };
            var jobHandle = reallyToughParallelJobTransforms.Schedule(transformAccessArray);
            jobHandle.Complete();

            //var jobHandle = reallyToughParallelJob.Schedule(zombieList.Count, 100);
            //jobHandle.Complete();

            for (int i = 0; i < zombieList.Count; i++)
            {
                //zombieList[i].transform.position = positionArray[i];
                zombieList[i].moveY = moveYArray[i];
            }

            //positionArray.Dispose();
            moveYArray.Dispose();
            transformAccessArray.Dispose();
        }
        else
        {
            foreach (var zombie in zombieList)
            {
                zombie.transform.position += new Vector3(0, zombie.moveY * Time.deltaTime);
                if (zombie.transform.position.y > 5f)
                {
                    zombie.moveY = -math.abs(zombie.moveY);
                }
                if (zombie.transform.position.y < -5f)
                {
                    zombie.moveY += math.abs(zombie.moveY);
                }
            }
        }
        /*
        if (useJobs)
        {
            NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            for (int i = 0; i < count; i++)
            {
                var jobHandle = ReallyToughTaskJob();
                jobHandleList.Add(jobHandle);
                //jobHandle.Complete();
            }
            JobHandle.CompleteAll(jobHandleList);
            jobHandleList.Dispose();
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                ReallyToughTask();
            }
        }
        */
        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }
    private void ReallyToughTask()
    {
        float value = 0f;
        for (int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }

    private JobHandle ReallyToughTaskJob()
    {
        var job = new ReallyToughJob();
        return job.Schedule();
    }
}

[BurstCompile]
public struct ReallyToughJob : IJob
{
    public void Execute()
    {
        float value = 0f;
        for (int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}

[BurstCompile]
public struct ReallyTouchParallelJob : IJobParallelFor
{
    public NativeArray<float3> positionArray;
    public NativeArray<float> moveYArray;
    [ReadOnly]
    public float deltime;

    public void Execute(int index)
    {
        positionArray[index] += new float3(0, moveYArray[index] * deltime, 0f);
        if (positionArray[index].y > 5f)
        {
            moveYArray[index] = -math.abs(moveYArray[index]);
        }
        if (positionArray[index].y < -5f)
        {
            moveYArray[index] += math.abs(moveYArray[index]);
        }
        float value = 0f;
        for (int i = 0; i < 1000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}


[BurstCompile]
public struct ReallyToughParllelJobTransforms : IJobParallelForTransform
{
    public NativeArray<float> moveYArray;
    [ReadOnly]
    public float deltime;
    public void Execute(int index, TransformAccess transform)
    {
        transform.position += new Vector3(0, moveYArray[index] * deltime, 0f);
        if (transform.position.y > 5f)
        {
            moveYArray[index] = -math.abs(moveYArray[index]);
        }
        if (transform.position.y < -5f)
        {
            moveYArray[index] += math.abs(moveYArray[index]);
        }
        float value = 0f;
        for (int i = 0; i < 1000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}
