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
        public bool bPredictThrust = false;

        private float _previousOrientation;
        private float _lastExecuteTime = Time.time;

        public override string OnDrawNodeText()
        {
            return bUseOwnSpaceship ? "Use Own Spaceship" : "Use Other Spaceship";
        }

        public override TaskStatus OnUpdate()
        {
            SpaceShipView spaceShip = bUseOwnSpaceship ? Controller.Value.OwnSpaceShip : Controller.Value.OtherSpaceShip;
            if (!bPredictThrust)
            {
                Vector2 velocity = spaceShip.Velocity * AdvanceTime.Value;
                StoredInformation.Value = spaceShip.Position + velocity;
                return TaskStatus.Success;
            }
            else
            {
                Vector2 velocity = spaceShip.Velocity;
                Vector2 predictedPos = spaceShip.Position;
                Vector2 direction = new Vector2(Mathf.Cos(spaceShip.Orientation * Mathf.Deg2Rad), Mathf.Sin(spaceShip.Orientation  * Mathf.Deg2Rad));
                Vector2 addedDirection = direction * spaceShip.Thrust * 5.0f * Time.fixedDeltaTime;
                for (int i = 0; i < (int)(AdvanceTime.Value/Time.fixedDeltaTime); i++)
                {
                    velocity = Vector2.ClampMagnitude(velocity + addedDirection, spaceShip.SpeedMax);
                    predictedPos += velocity * Time.fixedDeltaTime;
                }
                StoredInformation.Value = predictedPos;
            }
            return TaskStatus.Success;
        }
    }
}
