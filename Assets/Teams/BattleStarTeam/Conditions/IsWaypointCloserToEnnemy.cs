using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;


namespace BattleStarTeam
{
    [TaskCategory("Battlestar/Conditions")]
    public class IsWaypointCloserToEnnemy : Conditional
    {
        public SharedBattleStarData Data;
        public SharedTargetData WaypointDestination;

        [Tooltip("La largeur du couloir.")]
        public SharedFloat PathWidth = 2f;

        public override TaskStatus OnUpdate()
        {
            Vector2 dirToEnemy = (Data.Value.EnnemyPosition - Data.Value.SelfPosition).normalized;
            float distanceToEnemy = Vector2.Distance(Data.Value.SelfPosition, Data.Value.EnnemyPosition);

            WayPointView nearestWaypoint = null;
            float nearestDist = float.MaxValue;
            foreach (WayPointView waypoint in Data.Value.Waypoints)
            {
                if (waypoint.Owner == Data.Value.SelfOwner)
                    continue;

                Vector2 dirWaypoint = waypoint.Position - Data.Value.SelfPosition;
                float projection = Vector2.Dot(dirWaypoint, dirToEnemy);
                if (projection < 0 || projection > distanceToEnemy)
                    continue;

                //Check if is inside corridor
                float perpendicular = Mathf.Abs(Vector2.Dot(dirWaypoint, new Vector2(-dirToEnemy.y, dirToEnemy.x)));
                if (perpendicular <= PathWidth.Value * 0.5f)
                {
                    float distToSelf = Vector2.Distance(Data.Value.SelfPosition, waypoint.Position);
                    if (distToSelf < nearestDist)
                    {
                        nearestDist = distToSelf;
                        nearestWaypoint = waypoint;
                    }
                }
            }

            if (nearestDist != float.MaxValue)
            {
                TargetData newTargetData = new TargetData(nearestWaypoint.Position, .5f, Data.Value.WaypointsData[nearestWaypoint].NextWaypoint.Position);
                WaypointDestination.SetValue(newTargetData);
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}

