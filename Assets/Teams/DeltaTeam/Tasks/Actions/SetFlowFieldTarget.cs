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
        private Vector2 OldPos = new Vector2(int.MaxValue,int.MaxValue);
        
        public override TaskStatus OnUpdate()
        {
            if (TargetPosition.Value == OldPos)
            {
                //Debug.Log("Same Pos For Flow Field");
                return TaskStatus.Success;
            }
            DeltaController controller = SharedController.Value;
            Vector2 playerPos = controller.OwnSpaceShip.Position;
            GridController curFlowField = controller.GetComponent<GridController>();
            if (curFlowField == null) return TaskStatus.Failure;
            curFlowField.CreateNewFlowField(TargetPosition.Value);
            OldPos = TargetPosition.Value;
            return TaskStatus.Success;
        }
    }
}