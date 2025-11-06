using DoNotModify;
using System.Collections.Generic;
using UnityEngine;

namespace BattleStarTeam
{
    public class BattleStarIdealWaypointAlgo : MonoBehaviour
    {
        [System.Serializable]
        private struct RangeScorer
        {
            public float Distance;
            public int Points;
        };

        [Header("Scorers")]
        [SerializeField] private List<RangeScorer> _scoreByRange;
        [SerializeField] private int _scoreByRangeDefault;
        [Space]
        [SerializeField] private int PointsIfInFront;
        [SerializeField] private int PointsIfInBack;
        [Space]
        [SerializeField] private int IsWayPointNearby;
        [SerializeField] private int IsNOWayPointNearby;
        [Space]
        [SerializeField] private int EnnemyWaypointPoints;
        [SerializeField] private int NeutralWaypointPoints;

        public WayPointView GetIdealWaypoint(BattleStarBlackboardData data)
        {
            WayPointView currentIdealWaypoint = null;
            int currentIdealWaypointScore = -1;
            foreach(WayPointView waypoint in data.Waypoints)
            {
                int currentWaypointScore = 0;

                if (waypoint.Owner == data.SelfOwner)
                    continue;

                //Calculate score
                bool foundScoreRange = false;
                foreach(RangeScorer range in _scoreByRange)
                {
                    if (Vector2.Distance(data.SelfPosition, waypoint.Position) <=  range.Distance)
                    {
                        foundScoreRange = true;
                        currentWaypointScore += range.Points;
                        break;
                    }
                }
                if (!foundScoreRange)
                    currentWaypointScore += _scoreByRangeDefault;

                currentWaypointScore += IsWaypointInFrontOfSelf(data.SelfLookAt.normalized, (waypoint.Position - data.SelfPosition).normalized) ? PointsIfInFront : PointsIfInBack;

                currentWaypointScore += data.WaypointsData[waypoint].IsClose ? IsWayPointNearby : IsNOWayPointNearby;
                currentWaypointScore += waypoint.Owner != -1 ? EnnemyWaypointPoints : NeutralWaypointPoints;

                if (currentWaypointScore > currentIdealWaypointScore)
                {
                    currentIdealWaypointScore = currentWaypointScore;
                    currentIdealWaypoint = waypoint;
                }
            }

            if (currentIdealWaypoint == null)
                return null;

            return currentIdealWaypoint;
        }

        public bool IsWaypointInFrontOfSelf(Vector2 selfDirection, Vector2 waypointToSelf)
        {
            float dot = Vector2.Dot(selfDirection, waypointToSelf);
            return (dot >= 0);
        }
    }
}

