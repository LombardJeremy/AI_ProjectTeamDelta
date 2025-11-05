using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetTimeForBulletToReachPosition : Action
    {
        public SharedDeltaController Controller;
        public SharedVector2 Position;
        public SharedFloat TimeToReach;
        public bool bUseOwnShip = true;

        public override string OnDrawNodeText()
        {
            if (Controller.Value == null) return "Controller Missing !";
            return bUseOwnShip ? "Use Own Ship" : "Use Own Position";
        }

        public override TaskStatus OnUpdate()
        {
            SpaceShipView spaceShip = bUseOwnShip ? Controller.Value.OwnSpaceShip : Controller.Value.OtherSpaceShip;
            TimeToReach.Value = Vector2.Distance(spaceShip.Position, Position.Value) / BulletView.Speed;
            return TaskStatus.Success;
        }
    }
}