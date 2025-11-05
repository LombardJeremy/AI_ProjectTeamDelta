using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class RotateOppositeToTarget : Action
    {

        public SharedDeltaController Controller;
        public SharedVector2 TargetPosition;
        public float RunningForTime = 0;
        public bool bUseSteering = true;

        private float _timeStart;
        public override string OnDrawNodeText()
        {
            return "Current Target : " + TargetPosition.Name;
        }

        public override void OnStart()
        {
            _timeStart = Time.time;
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            Vector2 ownPosition = Controller.Value.OwnSpaceShip.Position;
            float angle;
            if (bUseSteering) angle = AimingHelpers.ComputeSteeringOrient(Controller.Value.OwnSpaceShip, TargetPosition.Value);
            else {angle = Mathf.Atan2(TargetPosition.Value.y - ownPosition.y, TargetPosition.Value.x - ownPosition.x) * Mathf.Rad2Deg;}
            Controller.Value.InputData.targetOrientation = Mathf.Repeat(angle + 180, 360f);
            if (_timeStart + RunningForTime >= Time.time)
            {
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }
    }
}
