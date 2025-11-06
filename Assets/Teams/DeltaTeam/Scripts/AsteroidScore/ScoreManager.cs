using System;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private Dictionary<WayPointView, float> waipointScoreList = new();

    private void Start()
    {
        foreach (var wayPoint in GameManager.Instance.GetGameData().WayPoints)
        {
            waipointScoreList[wayPoint] = 0.0f;
        }
        CalculateAllWeightAtPlayerPos();
    }

    public Dictionary<WayPointView, float> CalculateAllWeightAtPlayerPos()
    {
        foreach (var wayPoint in GameManager.Instance.GetGameData().WayPoints)
        {
            CalculateWaypointWeight(wayPoint);
        }
        return waipointScoreList;
    }

    public WayPointView GetWaypointWithLessWeight()
    {
        WayPointView key = null;
        float value = Int32.MaxValue;
        foreach (var asteroidWithHisValue in waipointScoreList)
        {
            if (asteroidWithHisValue.Value < value)
            {
                value = asteroidWithHisValue.Value;
                key = asteroidWithHisValue.Key;
            }
        }
        return key;
    }

    public void CalculateWaypointWeight(WayPointView waypointToCalculate)
    {
        float maxWeight = 0.0f;
        foreach (var waypoint in GameManager.Instance.GetGameData().WayPoints)
        {
            if(waypoint == waypointToCalculate) continue;
            maxWeight += Vector2.Distance(waypoint.Position, waypointToCalculate.Position);
        }
        maxWeight += Vector2.Distance(waypointToCalculate.Position, transform.position);
        waipointScoreList[waypointToCalculate] = maxWeight;
    }
}
