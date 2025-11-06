using BehaviorDesigner.Runtime;
using DoNotModify;
using UnityEngine;

namespace BattleStarTeam
{
    public class SharedTargetData : SharedVariable<TargetData>
    {
        public static implicit operator SharedTargetData(TargetData value)
        {
            return new SharedTargetData { Value = value };
        }
    }
}
