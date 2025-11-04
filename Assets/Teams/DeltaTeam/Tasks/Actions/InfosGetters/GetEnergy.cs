using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetEnergy : Action
    {
        public SharedDeltaController SharedController;
        public SharedFloat EnergyValue;
        public bool bUseOwnShip = false;

        public override string OnDrawNodeText()
        {
            return  "Stores " + (bUseOwnShip ? "Own" : "Other") + " Energy in " + EnergyValue.Name;
        }

        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            
            EnergyValue.Value = bUseOwnShip ? controller.OwnSpaceShip.Energy : controller.OtherSpaceShip.Energy;
            
            return TaskStatus.Success;
        }
    }
}