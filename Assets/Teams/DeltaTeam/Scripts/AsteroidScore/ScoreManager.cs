using System;
using System.Collections.Generic;
using DoNotModify;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private Dictionary<AsteroidView, float> asteroidScoreList;

    private void Start()
    {
        foreach (var asteroid in GameManager.Instance.GetGameData().Asteroids)
        {
            asteroidScoreList[asteroid] = 0.0f;
        }
        CalculateAllWeightAtPlayerPos();
    }

    public Dictionary<AsteroidView, float> CalculateAllWeightAtPlayerPos()
    {
        foreach (var asteroid in GameManager.Instance.GetGameData().Asteroids)
        {
            CalculateAsteroidWeight(asteroid);
        }
        return asteroidScoreList;
    }

    public AsteroidView GetAsteroidWithLessWeight()
    {
        AsteroidView key = null;
        float value = Int32.MaxValue;
        foreach (var asteroidWithHisValue in asteroidScoreList)
        {
            if (asteroidWithHisValue.Value < value)
            {
                value = asteroidWithHisValue.Value;
                key = asteroidWithHisValue.Key;
            }
        }
        return key;
    }

    public void CalculateAsteroidWeight(AsteroidView asteroidToCalculate)
    {
        float maxWeight = 0.0f;
        foreach (var asteroid in GameManager.Instance.GetGameData().Asteroids)
        {
            if(asteroid == asteroidToCalculate) continue;
            maxWeight += Vector2.Distance(asteroid.Position, asteroidToCalculate.Position);
        }
        maxWeight += Vector2.Distance(asteroidToCalculate.Position, transform.position);
        asteroidScoreList[asteroidToCalculate] = maxWeight;
    }
}
