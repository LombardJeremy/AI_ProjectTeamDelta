using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class FindClosestWaypoint : Action
    {
        public SharedDeltaController Controller;
        public SharedVector2 WaypointTarget;

        public override TaskStatus OnUpdate()
        {
            GameData gameData = Controller.Value.GameData;
            Vector2 ownPosition = Controller.Value.OwnSpaceShip.Position;
            int playerID = Controller.Value.OwnSpaceShip.Owner;
            WayPointView bestWP = null;
            foreach (WayPointView waypoint in gameData.WayPoints)
            {
                if (waypoint.Owner != playerID)
                {
                    if (bestWP == null) bestWP = waypoint;
                    if (Vector2.Distance(bestWP.Position, ownPosition) >
                        Vector2.Distance(waypoint.Position, ownPosition))
                    {
                        bestWP = waypoint;
                        //Debug.Log(waypoint.Position);
                    }
                }
            }

            if (bestWP == null) return TaskStatus.Failure;
            
            WaypointTarget.Value = bestWP.Position;
            
            return TaskStatus.Success;
        }
    }
}
