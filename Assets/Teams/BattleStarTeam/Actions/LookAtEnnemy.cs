using BattleStarTeam;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Angle")]
    public class LookAtEnnemy : Action
    {
        public SharedInputData InputData;
        public SharedBattleStarData Data;

        [Tooltip("Degré a partir du quel l'angle est considéré comme bonne")]
        public SharedFloat AcceptanceThreshold;

        [Tooltip("Degré a partir du quel l'angle est considéré comme bonne")]
        [SerializeField] private bool _useBounceAlgo;
        [SerializeField] private float _bounceSphereCastMultiplier;
        [SerializeField] private LayerMask _wallsLayer;

        private Vector2 bounceAlgoEstimatedPoint;
        private bool wasBounceAlgoPointSet;

        public override TaskStatus OnUpdate()
        {
            if (Data.Value.AreAsteroidBetweenShips)
                return TaskStatus.Failure;

            InputData newInputData = InputData.Value;
            Vector2 dir = Data.Value.EnnemyPosition - Data.Value.SelfPosition;

            if (dir.sqrMagnitude < Mathf.Epsilon)
                return TaskStatus.Success;

            //Calculate new angle
            float targetAngle = 0.0f;
            GetBouncingData(Data.Value.EnnemyPosition, Data.Value.EnnemyVelocity,out bool willBounceHappen, out Vector2 asteroidPosition);
            if (_useBounceAlgo && willBounceHappen)
            {
                //Calculate new angle (taking bonuce into account)
                if (!wasBounceAlgoPointSet)
                {
                    bounceAlgoEstimatedPoint = BounceAlgo(Data.Value.EnnemyPosition, asteroidPosition, Data.Value.EnnemyVelocity);
                    wasBounceAlgoPointSet = true;
                }
                
                Debug.Log(bounceAlgoEstimatedPoint);
                Vector2 dirToBounce = bounceAlgoEstimatedPoint - Data.Value.SelfPosition;
                if (dirToBounce.sqrMagnitude < Mathf.Epsilon)
                    targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // fallback
                else
                    targetAngle = Mathf.Atan2(dirToBounce.y, dirToBounce.x) * Mathf.Rad2Deg;
            }
            else
                //Calculate new angle (standard way)
                targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            targetAngle = NormalizeAngle(targetAngle);
            newInputData.targetOrientation = targetAngle;
            InputData.SetValue(newInputData);

            float delta = Mathf.DeltaAngle(Data.Value.SelfRotation, targetAngle);

            if(Mathf.Abs(delta) <= AcceptanceThreshold.Value)
            {
                wasBounceAlgoPointSet = false;
                return TaskStatus.Success;
            }
            else
                return TaskStatus.Running;
        }

        private float NormalizeAngle(float a)
        {
            a = a % 360f;
            if (a < 0f) a += 360f;
            return a;
        }

        private Vector2 BounceAlgo(Vector2 P0, Vector2 P2, Vector2 v)
        {
            //Calculate new angle (with bounce algo way)
            float t1 = Vector2.Distance(P2, P0) / v.magnitude;
            Vector2 n = (P2 - P0) / Abs(P2 - P0);
            return P2 + (v - 2 * (v * n) * n) * (3 - t1);
        }

        private void GetBouncingData(Vector2 position, Vector2 velocity, out bool isAsteroidHit, out Vector2 hitPoint)
        {
            RaycastHit2D hit = Physics2D.CircleCast(Data.Value.EnnemyPosition, .2f, Data.Value.EnnemyVelocity.normalized, Data.Value.EnnemyVelocity.magnitude * _bounceSphereCastMultiplier, _wallsLayer);
            isAsteroidHit = hit.collider != null;
            hitPoint = hit.collider != null ? hit.point : Vector2.zero;
        }

        Vector2 Abs(Vector2 a) => new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));

    }
}

