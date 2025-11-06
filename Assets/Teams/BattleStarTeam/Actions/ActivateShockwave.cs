using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Shockwave")]
    public class ActivateShockwave : Action
    {
        public SharedInputData InputDataRef;
        public override TaskStatus OnUpdate()
        {
            InputData newData = InputDataRef.Value;
            newData.fireShockwave = true;
            InputDataRef.SetValue(newData);
            return TaskStatus.Success;
        }
    }
}

