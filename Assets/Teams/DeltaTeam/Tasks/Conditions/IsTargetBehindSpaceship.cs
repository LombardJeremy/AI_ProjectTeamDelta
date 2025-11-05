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
            Vector2 up = Vector2.up;
            float x = Vector2.up.x;
            float y = Vector2.up.y;
            float radOritentation = spaceShip.Orientation * Mathf.Deg2Rad;
            up.x = x * Mathf.Cos(radOritentation) - y * Mathf.Sin(radOritentation);
            up.y = y * Mathf.Cos(radOritentation) + x * Mathf.Sin(radOritentation);

            if (Vector2.Dot(up, spaceShip.Position - TargetPosition.Value) > 0)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
