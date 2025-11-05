using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetStunCountdown : Action
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
            
            StunCountdown.Value = bUseOwnShip ? controller.OwnSpaceShip.StunPenaltyCountdown : controller.OtherSpaceShip.StunPenaltyCountdown;
            
            return TaskStatus.Success;
        }
    }
}