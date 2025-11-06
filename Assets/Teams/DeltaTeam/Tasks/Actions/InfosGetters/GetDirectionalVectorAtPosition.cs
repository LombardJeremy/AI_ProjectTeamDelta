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
            Vector2 anticipatedPos = SpaceshipPosition.Value;
            if (controller.GetComponent<GridController>().curFlowField == null) return TaskStatus.Failure;

            FlowField tmpFlowField = controller.GetComponent<GridController>().curFlowField;
            Cell cellAtPos = tmpFlowField.GetCellFromWorldPos(anticipatedPos);
            if (cellAtPos.bestDirection == GridDirection.None)
            {
                Vector2 tmpPos = cellAtPos.worldPos;
                Cell tmpCell = null;
                if (tmpFlowField.GetCellFromWorldPos(new Vector2(tmpPos.x + 1, tmpPos.y)).bestDirection !=
                    GridDirection.None)
                {
                    tmpCell = tmpFlowField.GetCellFromWorldPos(new Vector2(tmpPos.x + 1, tmpPos.y));
                }
                else if (tmpFlowField.GetCellFromWorldPos(new Vector2(tmpPos.x - 1, tmpPos.y)).bestDirection !=
                    GridDirection.None)
                {
                    tmpCell = tmpFlowField.GetCellFromWorldPos(new Vector2(tmpPos.x - 1, tmpPos.y));
                }
                else if (tmpFlowField.GetCellFromWorldPos(new Vector2(tmpPos.x, tmpPos.y + 1)).bestDirection !=
                    GridDirection.None)
                {
                    tmpCell = tmpFlowField.GetCellFromWorldPos(new Vector2(tmpPos.x, tmpPos.y + 1));
                }
                else if (tmpFlowField.GetCellFromWorldPos(new Vector2(tmpPos.x, tmpPos.y - 1)).bestDirection !=
                    GridDirection.None)
                {
                    tmpCell = tmpFlowField.GetCellFromWorldPos(new Vector2(tmpPos.x, tmpPos.y - 1));
                }
                if (tmpCell != null)
                {
                    DirectionalVector.Value = tmpFlowField.GetCellFromWorldPos(tmpCell.worldPos).bestDirection.Vector;
                    return TaskStatus.Success;
                }
            }
            DirectionalVector.Value = cellAtPos.bestDirection.Vector;
            return TaskStatus.Success;
        }
    }
}