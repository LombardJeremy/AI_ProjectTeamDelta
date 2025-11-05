using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class IsTargetBehindSpaceship : Conditional
    {
        public SharedDeltaController Controller;
        public SharedVector2 TargetPosition;
        public bool bUseOwnSpaceship = true;

        public override TaskStatus OnUpdate()
        {
            SpaceShipView spaceShip = bUseOwnSpaceship ? Controller.Value.OwnSpaceShip : Controller.Value.OtherSpaceShip;
            Vector2 right;
            float x = Vector2.right.x;
            float y = Vector2.right.y;
            float radOritentation = spaceShip.Orientation * Mathf.Deg2Rad;
            right.x = x * Mathf.Cos(radOritentation) - y * Mathf.Sin(radOritentation);
            right.y = y * Mathf.Cos(radOritentation) + x * Mathf.Sin(radOritentation);

            if (Vector2.Dot(right, spaceShip.Position - TargetPosition.Value) > 0)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
