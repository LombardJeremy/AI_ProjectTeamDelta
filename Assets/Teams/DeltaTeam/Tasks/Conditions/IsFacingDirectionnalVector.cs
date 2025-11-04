using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class IsFacingDirectionnalVector : Conditional
    {
        public SharedDeltaController Controller;
        public SharedVector2 DirectionnalVector;
        public float ErrorMarge = 1f;

        public override string OnDrawNodeText()
        {
            return "Current Target : " + DirectionnalVector.Name;
        }

        public override TaskStatus OnUpdate()
        {
            float angle = CustomAimingHelpers.ComputeSteeringOrient(Controller.Value.OwnSpaceShip, DirectionnalVector.Value);
            Debug.Log("dir : " + DirectionnalVector.Value + " angle : " + angle);
            if (Mathf.Abs(Mathf.DeltaAngle(Controller.Value.OwnSpaceShip.Orientation,  angle)) < ErrorMarge)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
