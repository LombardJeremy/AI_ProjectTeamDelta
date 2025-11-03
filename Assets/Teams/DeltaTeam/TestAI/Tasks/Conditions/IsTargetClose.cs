using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam/TestAI")]
    public class IsTargetClose : Conditional
    {
        public SharedController Controller;
        public SharedVector2 TargetPosition;
        public float Distance;

        public override string OnDrawNodeText()
        {
            return "Checking Target " + TargetPosition.Name + " Distance <= " + Distance;
        }

        public override TaskStatus OnUpdate()
        {
            if (Controller.Value == null) return TaskStatus.Failure;
            SpaceShipView otherSpaceShip = Controller.Value.OtherSpaceShip;
            SpaceShipView ownSpaceShip = Controller.Value.OwnSpaceShip;
            if (otherSpaceShip == null || ownSpaceShip == null) return TaskStatus.Failure;

            if (Vector2.Distance(otherSpaceShip.Position, ownSpaceShip.Position) <= Distance)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}
