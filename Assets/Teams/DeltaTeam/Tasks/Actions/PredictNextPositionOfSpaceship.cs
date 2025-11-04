using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class PredictNextPositionOfSpaceship : Action
    {
        public SharedDeltaController Controller;
        public SharedVector2 StoredInformation;
        public bool bUseOwnSpaceship = false;
        public SharedFloat AdvanceTime = 1; 

        public override string OnDrawNodeText()
        {
            return bUseOwnSpaceship ? "Use Own Spaceship" : "Use Other Spaceship";
        }

        public override TaskStatus OnUpdate()
        {
            SpaceShipView spaceShip = bUseOwnSpaceship ? Controller.Value.OwnSpaceShip : Controller.Value.OtherSpaceShip;
            Vector2 velocity = spaceShip.Velocity * AdvanceTime.Value;
            StoredInformation.Value = spaceShip.Position + velocity;
            return TaskStatus.Success;
        }
    }
}
