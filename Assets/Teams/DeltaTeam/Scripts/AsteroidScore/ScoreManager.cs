using System;
using System.Collections.Generic;
using DeltaTeam;
using DoNotModify;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private Dictionary<WayPointView, float> waipointScoreList = new();

    private DeltaController spaceShipController;

    [SerializeField] private float weightTweak = .7f;
    [SerializeField] private float weightTweakEnnemy = .5f;
    

    private void Start()
    {
        spaceShipController = GetComponent<DeltaController>();
        if (!spaceShipController)
        {
            return;
        }
        foreach (var wayPoint in GameManager.Instance.GetGameData().WayPoints)
        {
            waipointScoreList[wayPoint] = 0.0f;
        }
        CalculateAllWeightAtPlayerPos(weightTweak, weightTweakEnnemy);
    }

    public Dictionary<WayPointView, float> CalculateAllWeightAtPlayerPos(float ownWeight, float ennemyWeight)
    {
        weightTweak = ownWeight;
        weightTweakEnnemy = ennemyWeight;
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
        foreach (var waypointWithHisValue in waipointScoreList)
        {
            if(waypointWithHisValue.Key.Owner == spaceShipController.OwnSpaceShip.Owner) continue;
            if (waypointWithHisValue.Value < value)
            {
                value = waypointWithHisValue.Value;
                key = waypointWithHisValue.Key;
            }
        }
        return key;
    }

    public void CalculateWaypointWeight(WayPointView waypointToCalculate)
    {
        if (waypointToCalculate.Owner == spaceShipController.OwnSpaceShip.Owner)
        {
            waipointScoreList[waypointToCalculate] = 0;
            return;
        }
        float maxWeight = 0.0f;
        foreach (var waypoint in GameManager.Instance.GetGameData().WayPoints)
        {
            if(waypoint == waypointToCalculate) continue;
            maxWeight += Vector2.Distance(waypoint.Position, waypointToCalculate.Position);
        }
        maxWeight += Mathf.Exp(Vector2.Distance(waypointToCalculate.Position,  spaceShipController.OwnSpaceShip.Position) * weightTweak);
        maxWeight -= Mathf.Exp(Vector2.Distance(waypointToCalculate.Position,  spaceShipController.OtherSpaceShip.Position) * weightTweakEnnemy);
        if(maxWeight <= 0) maxWeight = 0.0001f;
        waipointScoreList[waypointToCalculate] = maxWeight;
        //Debug.Log(maxWeight);
    }
}
