using DoNotModify;
using UnityEngine;

namespace BattleStarTeam { 

    public class Navigation : MonoBehaviour
    {

        public SpaceShipView spaceShip;

        [Header("Settings")]
        public float radiusDetectionAsteroid = 3f;
        public float distanceDetectionAsteroid = 5f;
        public float rotationDetectionAsteroid = 0.4f;

        public float radiusDetectionMine = 3f;
        public float distanceDetectionMine = 5f;
        public float rotationDetectionMine = 0.4f;

        public float distByRotationReduceMine = 0.1f;
        public float distByRotationReduceAsteroid = 0.1f;

        public float offsetAvoidance = 5.0f;

        public float maxDistThrust = 5f;

        public bool clockWiseOnly = false;

        public LayerMask asteroidLayer;
        public LayerMask mineLayer;

        public AnimationCurve thrustCurve;

        private TargetData targetData;

        private Vector2 target = Vector2.zero;

        public void SetNavigation(TargetData newTargetData)
        {
            targetData = newTargetData;

            target = targetData.TargetPosition;

            if (targetData.IsSecondTargetSet)
            {
                Vector2 dirToNext = (targetData.NextTarget - targetData.TargetPosition).normalized;
                Vector2 dirFromShip = (spaceShip.Position - targetData.TargetPosition).normalized;

                Vector2 perp = Vector2.Perpendicular(dirToNext);

                Vector2 pointA = targetData.TargetPosition + perp * targetData.TargetRadius;
                Vector2 pointB = targetData.TargetPosition - perp * targetData.TargetRadius;

                float dotA = Vector2.Dot(dirFromShip, (pointA - targetData.TargetPosition).normalized);
                float dotB = Vector2.Dot(dirFromShip, (pointB - targetData.TargetPosition).normalized);

                target = (dotA > dotB) ? pointA : pointB;

                Debug.DrawLine(spaceShip.Position, targetData.NextTarget, Color.gray);
            }
        }

        public float GetRotationValue()
        {
            MineView mineView = AnyMineInPath();
            if (mineView != null)
            {
                return GetNewRotationWithMine(mineView);
            }

            AsteroidView asteroidView = AnyAsteroidInPath();
            if (asteroidView != null)
                return GetNewRotationWithAsteroid(asteroidView);

            return GetRotationValueNormal();
        }

        private float GetRotationValueNormal()
        {
            if (Vector2.Distance(spaceShip.LookAt.normalized, (target - spaceShip.Position).normalized) < 0.25 
                && Vector2.Distance(spaceShip.Velocity.normalized, (target - spaceShip.Position).normalized) > 0.25)
                return AimingHelpers.ComputeSteeringOrient(spaceShip, target) - Vector2.SignedAngle(Vector2.right, spaceShip.Velocity);

            if (!clockWiseOnly)
                return Vector2.SignedAngle(spaceShip.LookAt, (target - spaceShip.Position).normalized);
            else
                return Vector2.Angle(spaceShip.LookAt, (target - spaceShip.Position).normalized);
        }

        public float GetThrustValue()
        {
            float dist = Vector2.Distance(spaceShip.Position, target);
            float thrust = Mathf.Clamp01(thrustCurve.Evaluate(dist / maxDistThrust));
            Debug.DrawRay(spaceShip.Position, (target - spaceShip.Position).normalized * maxDistThrust, Color.green);
            return thrust;
        }

        private AsteroidView AnyAsteroidInPath()
        {
            RaycastHit2D? hit = AnyObjectInPath(asteroidLayer, distanceDetectionAsteroid, radiusDetectionAsteroid, rotationDetectionAsteroid, distByRotationReduceAsteroid, Color.yellow);

            if (hit.HasValue)
                return hit.Value.rigidbody.GetComponent<Asteroid>().view;

            return null;
        }

        private MineView AnyMineInPath()
        {
            RaycastHit2D? hit = AnyObjectInPath(mineLayer, distanceDetectionMine, radiusDetectionMine, rotationDetectionMine, distByRotationReduceMine, Color.magenta);

            if (hit.HasValue)
                return hit.Value.rigidbody.GetComponent<Mine>().view;

            return null;
        }

        private RaycastHit2D? AnyObjectInPath(LayerMask layer, float distance, float radius, float rotation, float distByRotationReduce, Color colorRay)
        {
            float points = radius / rotation;

            float distClosest = float.MaxValue;
            RaycastHit2D? hit = null;

            for (int i = 0; i < points; i++)
            {
                float newDist = Mathf.Clamp(distance * Mathf.Clamp01(1 - distByRotationReduce * i), 0f, distance);

                Vector2 rotationLeft = Rotate(spaceShip.Velocity.normalized, i * rotation);
                Debug.DrawRay(spaceShip.Position, rotationLeft * newDist, colorRay);
                RaycastHit2D hitLeft;
                hitLeft = Physics2D.Raycast(spaceShip.Position, rotationLeft, newDist, layer);
                if (hitLeft)
                {
                    if (hitLeft.distance < distClosest)
                    {
                        distClosest = hitLeft.distance;
                        hit = hitLeft;
                    }
                }

                Vector2 rotationRight = Rotate(spaceShip.Velocity.normalized, -i * rotation);
                Debug.DrawRay(spaceShip.Position, rotationRight * newDist, colorRay);
                RaycastHit2D hitRight;
                hitRight = Physics2D.Raycast(spaceShip.Position, rotationRight, newDist, layer);
                if (hitRight)
                {
                    if (hitRight.distance <  distClosest)
                    {
                        distClosest = hitRight.distance;
                        hit = hitRight;
                    }
                }
            }

            return hit;
        }

        private int GetMinesAround()
        {
            int mines = 0;

            float points = radiusDetectionMine / rotationDetectionMine;
            for (int i = 0; i < points; i++)
            {
                float newDist = Mathf.Clamp(distanceDetectionMine * Mathf.Clamp01(1 - distByRotationReduceMine * i), 0f, distanceDetectionMine);

                Vector2 rotationLeft = Rotate(spaceShip.Velocity.normalized, i * rotationDetectionMine);
                RaycastHit2D hitLeft;
                hitLeft = Physics2D.Raycast(spaceShip.Position, rotationLeft, newDist, mineLayer);
                if (hitLeft)
                    mines++;

                Vector2 rotationRight = Rotate(spaceShip.Velocity.normalized, -i * rotationDetectionMine);
                RaycastHit2D hitRight;
                hitRight = Physics2D.Raycast(spaceShip.Position, rotationRight, newDist, mineLayer);
                if (hitRight)
                    mines++;
            }

            return mines;
        }

        private float GetNewRotationWithAsteroid(AsteroidView asteroid)
        {
            return GetNewRotationWithObject(asteroid.Position, asteroid.Radius, offsetAvoidance);
        }

        private float GetNewRotationWithMine(MineView mine)
        {
            return GetNewRotationWithObject(mine.Position, mine.ExplosionRadius, offsetAvoidance);
        }

        private float GetNewRotationWithObject(Vector2 objectPosition, float objectRadius, float offsetAvoidance)
        {


            Vector2 dirToObj = (objectPosition - spaceShip.Position).normalized;

            float alignment; 
            bool willCollide = WillCollide(spaceShip.Position, spaceShip.Velocity, objectPosition, objectRadius + offsetAvoidance, out alignment);

            if (!willCollide)
            {
                return GetRotationValueNormal();
            }

            float avoidAngle = Mathf.Lerp(35f, 85f, Mathf.Clamp01(alignment));

            Vector2 newDirRight = Rotate(Vector2.Perpendicular(dirToObj).normalized, avoidAngle);
            Vector2 addPosRight = newDirRight * (objectRadius + offsetAvoidance);

            Vector2 newDirLeft = Rotate(Vector2.Perpendicular(dirToObj).normalized, 180 - avoidAngle).normalized;
            Vector2 addPosLeft = newDirLeft * (objectRadius + offsetAvoidance);

            Vector2 addPos =
                (Vector2.Distance(spaceShip.Position, addPosRight) + Vector2.Distance(target, addPosRight)
                < Vector2.Distance(spaceShip.Position, addPosLeft) + Vector2.Distance(target, addPosLeft))
                ? addPosRight : addPosLeft;

            Vector2 posToFollow = objectPosition + addPos;

            Vector2 toTarget = (target - spaceShip.Position);
            Vector2 toPos = (posToFollow - spaceShip.Position);

            float targetDist = toTarget.magnitude;
            if (targetDist < Mathf.Epsilon) return GetRotationValueNormal();

            float proj = Vector2.Dot(toPos, toTarget.normalized);
            float pathViaPos = toPos.magnitude + Vector2.Distance(posToFollow, target);

            bool goesBehind = proj < -0.01f;
            bool pathTooLong = pathViaPos > targetDist * 1.0f;
            bool veryBadAngle = Vector2.Angle(toPos, toTarget) > 130f;

            if (!(goesBehind && pathTooLong) || !veryBadAngle)
            {
                print("No correction");
                return GetRotationValueNormal();
            }

            print("CORRECTION");
            Debug.DrawLine(spaceShip.Position, posToFollow, Color.white);

            return AimingHelpers.ComputeSteeringOrient(spaceShip, posToFollow);
        }

        private float GetLookAngle(Vector3 fromPosition, Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - fromPosition;
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            return angle;
        }

        public Vector2 Rotate(Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta * Mathf.Deg2Rad) - v.y * Mathf.Sin(delta * Mathf.Deg2Rad),
                v.x * Mathf.Sin(delta * Mathf.Deg2Rad) + v.y * Mathf.Cos(delta * Mathf.Deg2Rad)
            );
        }

        bool WillCollide(Vector2 moverPos, Vector2 moverVel, Vector2 targetPos, float targetRadius, out float alignment)
        {
            alignment = 0f;
            if (moverVel.sqrMagnitude < 0.001f) return false;

            Vector2 relPos = targetPos - moverPos;
            Vector2 velDir = moverVel.normalized;

            float projection = Vector2.Dot(relPos, velDir);
            if (projection < 0f) return false; // l’objet est derrière

            Vector2 closestPoint = moverPos + velDir * projection;
            float distToPath = Vector2.Distance(targetPos, closestPoint);

            alignment = Vector2.Dot(velDir, relPos.normalized); // 1 = pile vers l’objet
            return distToPath <= targetRadius;
        }
    }

}