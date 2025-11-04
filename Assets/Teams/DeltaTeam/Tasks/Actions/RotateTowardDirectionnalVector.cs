using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class RotateTowardDirectionnalVector : Action
    {

        public SharedDeltaController Controller;
        public SharedVector2 DirectionnalVector;
        public bool bUseSteering = false;
        
        public override string OnDrawNodeText()
        {
            return "Current Value listenned : " + DirectionnalVector.Name;
        }

        public override TaskStatus OnUpdate()
        {
            float angle;
            if (bUseSteering) angle = CustomAimingHelpers.ComputeSteeringOrient(Controller.Value.OwnSpaceShip, DirectionnalVector.Value);
            else angle = Mathf.Atan2(DirectionnalVector.Value.y, DirectionnalVector.Value.x) * Mathf.Rad2Deg;
            Controller.Value.InputData.targetOrientation = angle;
            return TaskStatus.Success;
        }
    }
}
