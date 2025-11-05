using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class IsStun : Conditional
    {
        public SharedDeltaController Controller;
        public bool bUseOwnShip = false;

        public override string OnDrawNodeText()
        {
            if (Controller.Value == null) return "Controller Missing !";
            return bUseOwnShip ? "Use Own Space Ship " : "Use Other Space Ship ";
        }

        public override TaskStatus OnUpdate()
        {
            SpaceShipView spaceShip = bUseOwnShip ? Controller.Value.OwnSpaceShip : Controller.Value.OtherSpaceShip;
            if (spaceShip.StunPenaltyCountdown > 0.1f)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
