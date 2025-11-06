using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Input")]
    public class SetFireInput : Action
    {
        [Tooltip("Mettre les inputdataref")]
        public SharedInputData InputDataRef;

        public SharedBool Fire;
        public SharedBool LandMine;
        public SharedBool Shockwave;

        public override TaskStatus OnUpdate()
        {
            InputData newInput = InputDataRef.Value;
            newInput.shoot = Fire.Value;
            newInput.dropMine = LandMine.Value;
            newInput.fireShockwave = Shockwave.Value;
            InputDataRef.SetValue(newInput);
            return TaskStatus.Success;
        }
    }
}
