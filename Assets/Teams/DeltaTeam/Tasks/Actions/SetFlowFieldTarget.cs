using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class SetFlowFieldTarget : Action
    {
        public SharedDeltaController SharedController;
        public SharedVector2 TargetPosition;
        
        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            Vector2 playerPos = controller.OwnSpaceShip.Position;
            GridController curFlowField = controller.GetComponent<GridController>();
            if (curFlowField == null) return TaskStatus.Failure;
            curFlowField.CreateNewFlowField(TargetPosition.Value);
            return TaskStatus.Success;
        }
    }
}