using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("Battlestar/Conditions")]
    public class IsPlayerHit : Conditional
    {
        public SharedBattleStarData Data;

        public override TaskStatus OnUpdate()
        {
            return Data.Value.IsSelfSlowedDown ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}