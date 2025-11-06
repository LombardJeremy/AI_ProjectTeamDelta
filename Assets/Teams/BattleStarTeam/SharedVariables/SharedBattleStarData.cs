using BehaviorDesigner.Runtime;
using DoNotModify;
using UnityEngine;

namespace BattleStarTeam
{
    public class SharedBattleStarData : SharedVariable<BattleStarBlackboardData>
    {
        public static implicit operator SharedBattleStarData(BattleStarBlackboardData value)
        {
            return new SharedBattleStarData { Value = value };
        }
    }

}
