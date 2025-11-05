using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class IsOnWaypoint : Conditional
    {
        public SharedDeltaController Controller;
        public SharedBool IsWaypointOwned;
        public float RadiusLooked = 1;

        public override string OnDrawNodeText()
        {
            if (Controller.Value == null) return "Controller Missing !";
            return base.OnDrawNodeText();
        }

        public override TaskStatus OnUpdate()
        { 
            List<WayPointView> gameDataWayPoints = Controller.Value.GameData.WayPoints;
            foreach (WayPointView wayPoint in gameDataWayPoints)
            {
                if (Vector2.Distance(wayPoint.Position, Controller.Value.OwnSpaceShip.Position) < RadiusLooked)
                {
                    IsWaypointOwned.Value = wayPoint.Owner == Controller.Value.OwnSpaceShip.Owner;
                    return TaskStatus.Success;
                } ;
            }
            return TaskStatus.Failure;
        }
    }
}
