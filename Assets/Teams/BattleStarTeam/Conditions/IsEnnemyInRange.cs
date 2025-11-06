using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BattleStarTeam
{
    [TaskCategory("Battlestar/Conditions")]
    public class IsEnnemyInRange : Conditional
    {
        public SharedBattleStarData Data;

        public SharedFloat MaxDistance;

        public override TaskStatus OnUpdate()
        {
            return Vector2.Distance(Data.Value.SelfPosition, Data.Value.EnnemyPosition) <= MaxDistance.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }

}
