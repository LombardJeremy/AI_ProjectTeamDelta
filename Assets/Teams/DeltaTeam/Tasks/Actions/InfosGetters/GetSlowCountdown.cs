using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetSlowCountdown : Action
    {
        public SharedDeltaController Controller;
        public SharedFloat StunCountdown;
        public bool bUseOwnShip;

        public override string OnDrawNodeText()
        {
            if (Controller.Value == null) return "Controller Missing !";
            return bUseOwnShip ? "Use Own Space Ship " : "Use Other Space Ship ";
        }

        public override TaskStatus OnUpdate()
        {
            DeltaController controller = Controller.Value;
            
            StunCountdown.Value = bUseOwnShip ? controller.OwnSpaceShip.HitPenaltyCountdown : controller.OtherSpaceShip.HitPenaltyCountdown;
            
            return TaskStatus.Success;
        }
    }
}