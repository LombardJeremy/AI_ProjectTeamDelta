using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Input")]
    public class FireAssist : Action
    {
        [Tooltip("Mettre InputData")]
        public SharedInputData InputDataRef;
        [Tooltip("Mettre Data")]
        public SharedBattleStarData BattleStarDataRef;

        public override TaskStatus OnUpdate()
        {
            InputData newData = InputDataRef.Value;
            newData.shoot = BattleStarDataRef.Value.CanHitEnnemy;
            InputDataRef.SetValue(newData);
            return newData.shoot ? TaskStatus.Success : TaskStatus.Failure;
        }
    }

}