using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetTimeSinceLastAction : Action
    {
        public SharedDeltaController SharedController;
        public SharedFloat TimeSinceLastShot;
        public SharedFloat TimeSinceLastMineDropped;
        public SharedFloat TimeSinceLastFiredShockwave;

        public override string OnDrawNodeText()
        {
            if (SharedController.Value == null) { return "Missing Controller"; }
            return base.OnDrawNodeText();
        }

        public override TaskStatus OnUpdate()
        {
            TimeSinceLastAction timeSinceLastAction = SharedController.Value.TimeSinceLastAction;
            
            TimeSinceLastShot.Value = Time.time - timeSinceLastAction.Shoot;
            TimeSinceLastFiredShockwave.Value = Time.time - timeSinceLastAction.Shockwave;
            TimeSinceLastMineDropped.Value = Time.time - timeSinceLastAction.DropMine;
            
            return TaskStatus.Success;
        }
    }
}