using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BattleStarTeam
{
    [TaskCategory("Battlestar/Conditions")]
    public class CompareWaypoints : Conditional
    {
        public enum WaypointsCompare
        {
            SelfHasMore,
            SelfHasMoreOrEqual,
            EnnemyHasMore,
            EnnemyHasMoreOrEqual
        }

        public SharedBattleStarData Data;
        public WaypointsCompare Compare;

        public override TaskStatus OnUpdate()
        {
            float currentWayPoints = Data.Value.NumberOfSelfWayPoints;
            float ennemyWayPoints = Data.Value.NumberOfEnnemyWayPoints;
            bool isTrue = false;
            switch (Compare)
            {
                case WaypointsCompare.SelfHasMore: isTrue = currentWayPoints > ennemyWayPoints; break;
                case WaypointsCompare.SelfHasMoreOrEqual: isTrue = currentWayPoints >= ennemyWayPoints; break;
                case WaypointsCompare.EnnemyHasMore: isTrue = ennemyWayPoints > currentWayPoints; break;
                case WaypointsCompare.EnnemyHasMoreOrEqual: isTrue = ennemyWayPoints >= currentWayPoints; break;
            }
            return isTrue ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}

