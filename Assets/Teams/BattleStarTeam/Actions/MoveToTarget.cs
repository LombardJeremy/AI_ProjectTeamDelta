using BattleStarTeam;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;


namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Mouvement")]
    public class MoveToTarget : Action
    {
        public SharedInputData InputDataRef;
        public SharedBattleStarData Data;

        public SharedTargetData WaypointDestination;
        public SharedTargetData Target;

        [Tooltip("La distance minimale pour que le vaissau soit considere comme \"a la position\"")]
        public SharedFloat DistanceThreshold;

        public override TaskStatus OnUpdate()
        {
            //Disable target orientation
            InputData newInput = InputDataRef.Value;
            newInput.targetOrientation = -1;
            InputDataRef.SetValue(newInput);

            Target.SetValue(WaypointDestination.Value);
            bool hasReachedTarget = IsWaypointOwned(WaypointDestination.Value.TargetPosition) || (Vector2.Distance(WaypointDestination.Value.TargetPosition, Data.Value.SelfPosition) <= DistanceThreshold.Value);
            return hasReachedTarget ? TaskStatus.Success : TaskStatus.Running;
        }

        public bool IsWaypointOwned(Vector2 waypointPosition)
        {
            //Find waypoint by position
            foreach(WayPointView waypoint in Data.Value.Waypoints)
            {
                if(waypoint.Position == waypointPosition)
                    return waypoint.Owner == Data.Value.SelfOwner;
            }
            return true;
        }
    }
}
