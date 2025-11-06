using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetEnergyAdvantage : Action
    {
        public SharedDeltaController SharedController;
        public SharedFloat EnergyAdvantageValue;

        public override string OnDrawNodeText()
        {
            return  "Stores Energy Advantage in " + EnergyAdvantageValue.Name;
        }

        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            
            EnergyAdvantageValue.Value = controller.OwnSpaceShip.Energy - controller.OtherSpaceShip.Energy;
            
            return TaskStatus.Success;
        }
    }
}