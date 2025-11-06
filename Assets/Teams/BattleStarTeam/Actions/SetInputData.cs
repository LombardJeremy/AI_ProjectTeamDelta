using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Input")]
    public class SetInputData : Action
    {
        [Tooltip("Mettre InputData")]
        public SharedInputData InputDataRef;

        [Tooltip("la valeur du thrust (entre 0 et 1)")]
        public SharedFloat ThrottleValue;

        [Tooltip("L'angle de rotation (entre 0 et 360)")]
        public SharedFloat AngleValue;

        [Tooltip("Dois-je tirer sur cette frame ?")]
        public SharedBool FireValue;

        [Tooltip("Dois-je poser une mine sur cette frame ?")]
        public SharedBool MineValue;

        [Tooltip("Dois-je utiliser le shockwave ?")]
        public SharedBool ShockwaveValue;

        public override TaskStatus OnUpdate()
        {
            InputData newInput = InputDataRef.Value;
            newInput.thrust = ThrottleValue.Value;
            newInput.targetOrientation = AngleValue.Value;
            newInput.shoot = FireValue.Value;
            newInput.dropMine = MineValue.Value;
            newInput.fireShockwave = ShockwaveValue.Value;
            InputDataRef.SetValue(newInput);
            return TaskStatus.Success;
        }
    }
}
