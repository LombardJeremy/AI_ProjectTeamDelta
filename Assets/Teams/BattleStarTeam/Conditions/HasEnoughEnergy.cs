using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("Battlestar/Conditions")]
    public class HasEnoughEnergy : Conditional
    {
        private enum EnergyCompare
        {
            HasMoreThan,
            HasLessThan
        }

        public SharedBattleStarData Data;

        [SerializeField] EnergyCompare _compare;

        [Tooltip("L'énergie minimum requis")]
        public SharedFloat RequiredEnergy;

        public override TaskStatus OnUpdate()
        {
            return (Data.Value.SelfEnnergy >= RequiredEnergy.Value && _compare == EnergyCompare.HasMoreThan) ||
                   (Data.Value.SelfEnnergy <= RequiredEnergy.Value && _compare == EnergyCompare.HasLessThan)
                    ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
