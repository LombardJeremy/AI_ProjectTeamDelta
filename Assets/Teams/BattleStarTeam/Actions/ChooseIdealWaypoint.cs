using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using System.Collections.Generic;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Mouvement")]
    public class ChooseIdealWaypoint : Action
    {
        public SharedInputData InputDataRef;

        public SharedBattleStarData Data;
        public SharedTargetData WaypointDestination;
        public override TaskStatus OnUpdate()
        {
            //Disable target orientation
            InputData newInput = InputDataRef.Value;
            newInput.targetOrientation = -1;
            InputDataRef.SetValue(newInput);

            WayPointView idealWaypoint = Data.Value.IdealWayPoint;
            if (idealWaypoint == null)
                return TaskStatus.Failure;

            TargetData newTargetData = new TargetData(idealWaypoint.Position, .5f, Data.Value.WaypointsData[idealWaypoint].NextWaypoint.Position);
            WaypointDestination.SetValue(newTargetData);
            return TaskStatus.Success;
        }
    }
}