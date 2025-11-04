using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetRemainingTime : Action
    {
        public SharedDeltaController SharedController;
        public SharedFloat RemainingTime;

        public override string OnDrawNodeText()
        {
            return  "Stores RemainingTime in " + RemainingTime.Name;
        }

        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            
            RemainingTime.Value = controller.GameData.timeLeft;
            
            return TaskStatus.Success;
        }
    }
}