using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private bool useJobs;
    [SerializeField]
    private int count = 10;

    void Update()
    {
        float startTime = Time.realtimeSinceStartup;

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
