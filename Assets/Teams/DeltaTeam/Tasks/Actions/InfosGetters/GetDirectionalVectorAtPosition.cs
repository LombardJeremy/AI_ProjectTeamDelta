using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetDirectionalVectorAtPosition : Action
    {
        public SharedDeltaController SharedController;
        public SharedVector2 SpaceshipPosition;
        public SharedVector2 DirectionalVector;
        
        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            Vector2 playerPos = SpaceshipPosition.Value;
            if (controller.GetComponent<GridController>().curFlowField == null) return TaskStatus.Failure;
            DirectionalVector.Value = controller.GetComponent<GridController>().curFlowField
                .GetCellFromWorldPos(playerPos).bestDirection.Vector;
            return TaskStatus.Success;
        }
    }
}