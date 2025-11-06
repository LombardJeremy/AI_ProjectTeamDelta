using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Mouvement")]
    public class ChooseNearestWaypoint : Action
    {
        public SharedBattleStarData Data;
        public SharedVector2 WaypointDestination;
        public override TaskStatus OnUpdate()
        {
            TargetData newTargetData = new TargetData(Data.Value.GetNearestWaypoint.Position, .5f, Data.Value.WaypointsData[Data.Value.GetNearestWaypoint].NextWaypoint.Position);
            WaypointDestination.SetValue(newTargetData);
            return TaskStatus.Success;
        }
    }
}