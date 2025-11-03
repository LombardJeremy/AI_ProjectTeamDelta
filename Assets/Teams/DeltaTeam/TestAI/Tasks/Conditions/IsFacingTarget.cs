using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam/TestAI")]
    public class IsFacingTarget : Conditional
    {
        public SharedController Controller;
        public SharedVector2 TargetPosition;

        public override string OnDrawNodeText()
        {
            return "Current Target : " + TargetPosition.Name;
        }

        public override TaskStatus OnUpdate()
        {
            Vector2 ownPosition = Controller.Value.OwnSpaceShip.Position;
            float angle = Mathf.Atan2(TargetPosition.Value.y - ownPosition.y, TargetPosition.Value.x - ownPosition.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(Controller.Value.OwnSpaceShip.Orientation) - Mathf.Abs(angle) < 1f) 
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
