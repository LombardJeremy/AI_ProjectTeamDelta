using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class WillGetShot : Conditional
    {
        public SharedDeltaController Controller;
        public SharedFloat TimeTillShot;
        public SharedVector2 Intersection;
        public SharedVector2 DangerousBulletPosition;
        public float TimeTolerance;
        public float SameVelocityAngleTolerance = 60;

        public override void OnDrawGizmos()
        {
            if (Intersection.Value != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(Intersection.Value, 0.5f);
            }
            base.OnDrawGizmos();
        }

        public override TaskStatus OnUpdate()
        {
            List<BulletView> gameDataBullets = Controller.Value.GameData.Bullets;
            SpaceShipView spaceShip = Controller.Value.OtherSpaceShip;
            foreach (BulletView bullet in gameDataBullets)
            {
                Vector2 intersection;
                if (!AimingHelpers.ComputeIntersection(bullet.Position, bullet.Velocity, spaceShip.Position,
                        spaceShip.Velocity, out intersection)) continue;
                Vector2 spaceShipToIntersection = intersection - spaceShip.Position;
                Vector2 bulletToIntersection = intersection - bullet.Position;
                float bulletTimeToIntersection = bulletToIntersection.magnitude / Bullet.Speed;
                float targetTimeToIntersection = spaceShipToIntersection.magnitude / spaceShip.Velocity.magnitude;
                targetTimeToIntersection *= Vector2.Dot(spaceShipToIntersection, spaceShip.Velocity) > 0 ? 1 : -1;

                float timeDiff = bulletTimeToIntersection - targetTimeToIntersection;
                if (Mathf.Abs(timeDiff) < TimeTolerance)
                {
                    if (!CustomAimingHelpers.IsTargetBehindSpaceship(spaceShip, bullet.Position))
                    {
                        if (Mathf.Abs(Vector2.Angle(bullet.Velocity, spaceShip.Velocity)) < SameVelocityAngleTolerance)
                        {
                            Debug.Log("has skipped bullet");
                            continue;
                        }
                    }
                    TimeTillShot.Value = targetTimeToIntersection;
                    Intersection.Value = intersection;
                    DangerousBulletPosition.Value = bullet.Position;
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }
    }
}
