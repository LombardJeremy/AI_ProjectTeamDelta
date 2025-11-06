using BehaviorDesigner.Runtime;
using DoNotModify;
using UnityEngine;

namespace BattleStarTeam
{
    public class SharedInputData : SharedVariable<InputData>
    {
        public static implicit operator SharedInputData(InputData value)
        {
            return new SharedInputData { Value = value };
        }
    }
}
