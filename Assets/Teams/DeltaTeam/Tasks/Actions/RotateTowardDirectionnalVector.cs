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
        
        public override string OnDrawNodeText()
        {
            return "Current Value listenned : " + DirectionnalVector.Name;
        }

        public override TaskStatus OnUpdate()
        {
            float angle = CustomAimingHelpers.ComputeSteeringOrient(Controller.Value.OwnSpaceShip, DirectionnalVector.Value);
            Controller.Value.InputData.targetOrientation = angle;
            return TaskStatus.Success;
        }
    }
}
