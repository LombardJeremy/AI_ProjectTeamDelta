using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BattleStarTeam
{
    [TaskCategory("Battlestar/Conditions")]
    public class CompareTimer : Conditional
    {
        public enum TimerCompare
        {
            LessThan,
            LessThanOrEqual,
            MoreThan,
            MoreOrEqual
        }

        public SharedBattleStarData Data;
        public TimerCompare Compare;
        public SharedFloat To;

        public override TaskStatus OnUpdate()
        {
            float currentTime = Data.Value.TimeRemaining;
            bool isTrue = false;
            switch (Compare)
            {
                case TimerCompare.LessThan: isTrue = currentTime < To.Value; break;
                case TimerCompare.LessThanOrEqual: isTrue = currentTime <= To.Value; break;
                case TimerCompare.MoreThan: isTrue = currentTime > To.Value; break;
                case TimerCompare.MoreOrEqual: isTrue = currentTime >= To.Value; break;
            }
            return isTrue ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}

