using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class AreAllWaypointsOwned : Conditional
    {
        public SharedDeltaController Controller;

        public override TaskStatus OnUpdate()
        {
            List<WayPointView> wayPoints = Controller.Value.GameData.WayPoints;
            foreach (WayPointView wayPointView in wayPoints)
            {
                if (wayPointView.Owner != Controller.Value.OwnSpaceShip.Owner)
                {
                    return TaskStatus.Failure;
                }
            }
            return TaskStatus.Success;
        }
    }
}
