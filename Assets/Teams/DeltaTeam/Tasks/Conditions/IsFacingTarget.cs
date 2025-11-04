using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class IsFacingTarget : Conditional
    {
        public SharedDeltaController Controller;
        public SharedVector2 TargetPosition;
        public float ErrorMarge = 1f;
        public bool bUseSteering = true;

        public override string OnDrawNodeText()
        {
            return "Current Target : " + TargetPosition.Name;
        }

        public override TaskStatus OnUpdate()
        {
            Vector2 ownPosition = Controller.Value.OwnSpaceShip.Position;
            float angle;
            if (bUseSteering) angle = AimingHelpers.ComputeSteeringOrient(Controller.Value.OwnSpaceShip, TargetPosition.Value);
            else {angle = Mathf.Atan2(TargetPosition.Value.y - ownPosition.y, TargetPosition.Value.x - ownPosition.x) * Mathf.Rad2Deg; }
            if (Mathf.Abs(Mathf.DeltaAngle(Controller.Value.OwnSpaceShip.Orientation,  angle)) < ErrorMarge)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
