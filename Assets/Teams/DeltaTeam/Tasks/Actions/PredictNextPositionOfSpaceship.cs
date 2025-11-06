using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPhysics2D;
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
            Vector2 finalPos = spaceShip.Position;
            if (!bPredictThrust)
            {
                Vector2 velocity = spaceShip.Velocity * AdvanceTime.Value;
                finalPos = spaceShip.Position + velocity;
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
                finalPos = predictedPos;
            }
            RaycastHit2D raycastHit2D = Physics2D.Raycast(spaceShip.Position, finalPos - spaceShip.Position, (finalPos - spaceShip.Position).magnitude, LayerMask.GetMask("Asteroid"));
            if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.CompareTag("Asteroid"))
            {
                finalPos = raycastHit2D.point;
            }
            StoredInformation.Value = CustomAimingHelpers.ClampPositionToGridSize(finalPos);
            return TaskStatus.Success;
        }
    }
}
