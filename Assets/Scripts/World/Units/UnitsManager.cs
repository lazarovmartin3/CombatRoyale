using Unity.Burst;
using Unity.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using System;


public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetClosestUnit(Player myself, Vector3 myPos)
    {
        List<Unit> allUnits = new List<Unit>();

        foreach (Player player in GameManager.Instance.GetAllPlayers())
        {
            if (player != myself)
            {
                allUnits = new List<Unit>(player.GetUnits());

                //for (int i = 0; i < player.GetUnits().Count; i++) 
                //{
                //    float distance = float.MaxValue;
                //    float testingDistance = Vector3.Distance(myPos, player.GetUnits()[i].GetPosition());
                //    if (testingDistance < distance)
                //    {
                //        distance = testingDistance;
                //        target = player.GetUnits()[i].gameObject;
                //    }
                //}
            }
        }

        if(allUnits.Count == 0) return null;

        // Create NativeArrays for job processing
        NativeArray<Vector3> unitPositions = new NativeArray<Vector3>(allUnits.Count, Allocator.TempJob);
        NativeArray<float> distances = new NativeArray<float>(allUnits.Count, Allocator.TempJob);
        NativeArray<int> closestIndex = new NativeArray<int>(1, Allocator.TempJob);
        closestIndex[0] = -1;

        // Populate the unitPositions array
        for (int i = 0; i < allUnits.Count; i++)
        {
            unitPositions[i] = allUnits[i].GetPosition();
        }

        // Schedule the job to find the closest unit
        var job = new FindClosestUnitJob
        {
            targetPosition = myPos,
            unitPositions = unitPositions,
            distances = distances,
            closestIndex = closestIndex
        };

        JobHandle handle = job.Schedule(allUnits.Count, 64);
        handle.Complete();

        // Retrieve the closest unit's index
        int index = closestIndex[0];
        GameObject closestUnit = (index != -1) ? allUnits[index].gameObject : null;

        // Dispose of NativeArrays
        unitPositions.Dispose();
        distances.Dispose();
        closestIndex.Dispose();

        return closestUnit;
    }

    [BurstCompile]
    private struct FindClosestUnitJob : IJobParallelFor
    {
        [ReadOnly] public Vector3 targetPosition;
        [ReadOnly] public NativeArray<Vector3> unitPositions;
        public NativeArray<float> distances;
        public NativeArray<int> closestIndex;

        public void Execute(int index)
        {
            distances[index] = Vector3.Distance(targetPosition, unitPositions[index]);

            if (closestIndex[0] == -1 || distances[index] < distances[closestIndex[0]])
            {
                closestIndex[0] = index;
            }
        }
    }
}
