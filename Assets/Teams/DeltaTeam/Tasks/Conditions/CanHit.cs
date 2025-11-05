using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class CanHit : Conditional
    {
        public SharedDeltaController Controller;
        public bool bUseOwnShip = true;
        
        public override string OnDrawNodeText()
        {
            if (Controller.Value != null) return "Controller missing !";
            return base.OnDrawNodeText();
        }

        public override TaskStatus OnUpdate()
        {
            SpaceShipView spaceShipShooting = bUseOwnShip ? Controller.Value.OwnSpaceShip : Controller.Value.OtherSpaceShip;
            SpaceShipView otherSpaceShip = bUseOwnShip ? Controller.Value.OtherSpaceShip : Controller.Value.OwnSpaceShip;
            if (AimingHelpers.CanHit(spaceShipShooting, otherSpaceShip.Position, otherSpaceShip.Velocity, 1.5f))
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
