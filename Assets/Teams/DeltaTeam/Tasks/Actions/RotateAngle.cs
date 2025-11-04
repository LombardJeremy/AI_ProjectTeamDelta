using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class RotateAngle : Action
    {

        public SharedDeltaController Controller;
        public SharedFloat Angle;
        public bool bRotateRight = true;
        
        public override string OnDrawNodeText()
        {
            if (Controller.Value == null) return "Missing Controller !";
            return "Rotate " + (bRotateRight? "Right" : "Left") + " For " + Angle.Value + " Degrees";
        }

        public override TaskStatus OnUpdate()
        {
            Controller.Value.InputData.targetOrientation = Controller.Value.OwnSpaceShip.Orientation + (bRotateRight ? Angle.Value : -Angle.Value);
            return TaskStatus.Success;
        }
    }
}
