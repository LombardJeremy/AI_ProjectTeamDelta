using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam/TestAI")]
    public class RotateTowardTarget : Action
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
            Debug.Log("angle : " + angle + ", TargetPos : " + TargetPosition.Value);
            Controller.Value.SetRotation(angle);
            return TaskStatus.Success;
        }
    }
}
