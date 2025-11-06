using BehaviorDesigner.Runtime;
using DoNotModify;
using System.Collections.Generic;
using UnityEngine;

namespace BattleStarTeam
{
    public struct BattleStarWayPointData
    {
        public WayPointView NextWaypoint;
        public bool IsClose;
    }

    [System.Serializable]
    public struct BattleStarBlackboardData
    {
        //Self
        private SpaceShipView _selfSpaceShip;
        public int SelfOwner;
        public Vector2 SelfPosition { get => _selfSpaceShip.Position; }
        public float SelfRotation { get => _selfSpaceShip.Orientation; }
        public Vector2 SelfLookAt { get => _selfSpaceShip.LookAt; }
        public bool IsSelfStun { get => _selfSpaceShip.StunPenaltyCountdown > 0; }
        public bool IsSelfSlowedDown { get => _selfSpaceShip.HitPenaltyCountdown > 0; }


        public float SelfEnnergy { get => _selfSpaceShip.Energy; }

        //Ennemy
        private SpaceShipView EnnemySpaceShip;
        public Vector2 EnnemyPosition { get => EnnemySpaceShip.Position; }
        public Vector2 EnnemyVelocity { get => EnnemySpaceShip.Velocity; }
        public bool IsEnnemyStun { get => EnnemySpaceShip.StunPenaltyCountdown > 0; }
        public bool IsEnnemySlowedDown { get => EnnemySpaceShip.HitPenaltyCountdown > 0; }

        //Timer
        public float TimeRemaining;


        //Shooting
        public float CanHitEnnemyAngleTolerance;
        public bool AreAsteroidBetweenShips;
        public bool CanHitEnnemy
        {
            get => (AimingHelpers.CanHit(_selfSpaceShip, EnnemyPosition, CanHitEnnemyAngleTolerance) ||
                                            AimingHelpers.CanHit(_selfSpaceShip, EnnemyPosition, EnnemyVelocity, CanHitEnnemyAngleTolerance)) &&
                                            !AreAsteroidBetweenShips;
        }


        //shockwave
        private float _shockwaveRange;
        public bool IsEnnemyInShockwaveRange { get => Vector2.Distance(SelfPosition, EnnemyPosition) <= _shockwaveRange; }

        //Bullets
        public List<BulletView> NearByBullets;
        public bool IsPlayerProbablyGetShot { get => NearByBullets.Count > 0 && !_selfSpaceShip.HasShot; }


        //Nearby mines
        public List<Vector2> NearbyMines;
        public bool IsMineNearby { get => NearbyMines.Count > 0; }


        //Nearby Waypoints
        private float _nearestWaypointsMinimumDifference;
        public List<WayPointView> Waypoints;
        public List<WayPointView> NearbyWaypoints;
        public bool IsWaypoitsNearby { get => NearbyWaypoints.Count > 0; }

        public Dictionary<WayPointView, BattleStarWayPointData> WaypointsData;
        public WayPointView IdealWayPoint;
        public int NumberOfSelfWayPoints;
        public int NumberOfEnnemyWayPoints;

        public Vector2 GetPositionBehindClosestAsteroid
        {
            get
            {
                if (_selfSpaceShip == null) return Vector2.zero;

                AsteroidView closestAsteroid = null;
                Vector2 closestAsteroidPos = Vector2.zero;
                float closestDistance = float.MaxValue;

                foreach (AsteroidView asteroid in GameManager.Instance.GetGameData().Asteroids)
                {
                    if (asteroid == null) continue;

                    float distanceToAsteroid = Vector2.Distance(asteroid.Position, SelfPosition);
                    if (distanceToAsteroid < closestDistance)
                    {
                        closestDistance = distanceToAsteroid;
                        closestAsteroidPos = asteroid.Position;
                        closestAsteroid = asteroid;
                    }
                }

                return closestAsteroidPos + (closestAsteroidPos - EnnemyPosition).normalized * closestAsteroid.Radius;
            }
        }

        public WayPointView GetNearestWaypoint
        {
            get
            {
                WayPointView nearestWaypoint = null;
                float smallestDistance = float.MaxValue;
                foreach (WayPointView waypoint in Waypoints)
                {
                    if (waypoint.Owner == SelfOwner)
                        continue;

                    float distanceToWaypoint = Vector2.Distance(waypoint.Position, SelfPosition);
                    if (distanceToWaypoint < smallestDistance && Mathf.Abs(distanceToWaypoint - smallestDistance) >= _nearestWaypointsMinimumDifference)
                    {
                        smallestDistance = distanceToWaypoint;
                        nearestWaypoint = waypoint;
                    }
                }
                return nearestWaypoint;
            }
        }
        public Vector2 GetFarestWaypointFromEnnemy
        {
            get
            {
                Vector2 farestWaypoints = Vector2.zero;
                float farestDistance = 0.0f;
                foreach (WayPointView waypoint in Waypoints)
                {
                    if (waypoint.Owner == SelfOwner)
                        continue;

                    if (Vector2.Distance(waypoint.Position, EnnemyPosition) >= farestDistance)
                    {
                        farestDistance = Vector2.Distance(waypoint.Position, EnnemyPosition);
                        farestWaypoints = waypoint.Position;
                    }
                }

                return farestWaypoints;
            }
        }


        public BattleStarBlackboardData(SpaceShipView self, SpaceShipView ennemy, float shockwaveRadius, float time, float hitEnnemyAngleTolerance, float nearestWaypointsMinimumDifference)
        {
            SelfOwner = self.Owner;
            _selfSpaceShip = self;
            EnnemySpaceShip = ennemy;
            TimeRemaining = time;

            CanHitEnnemyAngleTolerance = hitEnnemyAngleTolerance;
            _nearestWaypointsMinimumDifference = nearestWaypointsMinimumDifference;
            _shockwaveRange = shockwaveRadius;
            AreAsteroidBetweenShips = false;
            IdealWayPoint = null;

            NumberOfSelfWayPoints = 0;
            NumberOfEnnemyWayPoints = 0;

            NearByBullets = new List<BulletView>();
            NearbyMines = new List<Vector2>();
            NearbyWaypoints = new List<WayPointView>();
            Waypoints = new List<WayPointView>();
            WaypointsData = new Dictionary<WayPointView, BattleStarWayPointData>();
        }
    }
    public class BattleStarData : MonoBehaviour
    {
        private const string BEHAVIOUR_BLACKBOARD_DATA_PARAM_NAME = "Data";


        [Header("Reference")]
        [SerializeField] private BehaviorTree _behaviourTree;
        [SerializeField] private BattleStarIdealWaypointAlgo _idealWaypointAlgo;
        //Shooting
        [Header("Shooting")]
        [Tooltip("La tolerance a laquel l'ennmi tire")]
        [SerializeField, Range(0, 10)] private float _canHitEnnemyAngleTolerance;

        [Tooltip("La radius de la shockwave")]
        [SerializeField, Range(0, 10)] private float _shockwaveRadius;

        //Bullets
        [Header("Bullets")]
        [Tooltip("Radius des tirs a proximit�s")]
        [SerializeField, Range(0, 10)] private float _nearbyBulletRadius;

        //Nearby mines
        [Header("Mines")]
        [Tooltip("Radius des mines a proximit�s")]
        [SerializeField, Range(0, 10)] private float _nearbyMineRadius;

        //Nearby Waypoints
        [Header("Waypoints")]
        [Tooltip("Radius des mines a proximit�s")]
        [SerializeField, Range(0, 10)] private float _nearbyWaypointsRadius;
        [Tooltip("La distance a laquelle un waypoint est considerer comme \"proche\" d'un autre")]
        [SerializeField, Range(0, 10)] private float _scorerWaypointDistance;
        [Tooltip("La distance minimum pour que 2 points soient assez eloignes ")]
        [SerializeField, Range(0, 10)] private float _nearestWaypointsMinimumDifference;

        private BattleStarBlackboardData _blackboardData;
        public BattleStarBlackboardData BlackboardData { get => _blackboardData; }


        public void InitializeBlackboardData(SpaceShipView spaceship, GameData data)
        {
            _blackboardData = new BattleStarBlackboardData(spaceship, data.GetSpaceShipForOwner(1 - spaceship.Owner), _shockwaveRadius, data.timeLeft,
                                                           _canHitEnnemyAngleTolerance, _nearestWaypointsMinimumDifference);

            foreach (WayPointView wayPoint in data.WayPoints)
                _blackboardData.Waypoints.Add(wayPoint);
        }

        public void UpdateData(SpaceShipView spaceship, GameData data, LayerMask asteroidLayer)
        {
            _blackboardData.TimeRemaining = data.timeLeft;
            _blackboardData.CanHitEnnemyAngleTolerance = _canHitEnnemyAngleTolerance;

            //Find nearby bullets
            _blackboardData.NearByBullets.Clear();
            foreach (BulletView bullet in data.Bullets)
            {
                if (Vector2.Distance(_blackboardData.SelfPosition, bullet.Position) <= _nearbyBulletRadius)
                    _blackboardData.NearByBullets.Add(bullet);
            }

            //Find nearby mines
            _blackboardData.NearbyMines.Clear();
            foreach (MineView mines in data.Mines)
            {
                if (Vector2.Distance(_blackboardData.SelfPosition, mines.Position) <= _nearbyMineRadius)
                    _blackboardData.NearbyMines.Add(mines.Position);
            }

            _blackboardData.NearbyWaypoints.Clear();
            _blackboardData.NumberOfSelfWayPoints = 0;
            _blackboardData.NumberOfEnnemyWayPoints = 0;
            foreach (WayPointView wayPoint in data.WayPoints)
            {
                //Get owned waypoints
                if (wayPoint.Owner == _blackboardData.SelfOwner)
                    _blackboardData.NumberOfSelfWayPoints++;
                //Get ennemy waypoints
                else if (wayPoint.Owner == 1 - _blackboardData.SelfOwner)
                    _blackboardData.NumberOfEnnemyWayPoints++;

                //Find nearby Waypoints
                if (Vector2.Distance(_blackboardData.SelfPosition, wayPoint.Position) <= _nearbyWaypointsRadius)
                    _blackboardData.NearbyWaypoints.Add(wayPoint);
            }

            //Check if asteroids nearby
            Vector2 dir = _blackboardData.EnnemyPosition - _blackboardData.SelfPosition;
            RaycastHit2D hit = Physics2D.Raycast(_blackboardData.SelfPosition, dir.normalized, dir.magnitude, asteroidLayer);
            _blackboardData.AreAsteroidBetweenShips = hit.collider != null;

            //Get ideal Waypoint
            CalculateNearestWaypoints(data);
            _blackboardData.IdealWayPoint = _idealWaypointAlgo.GetIdealWaypoint(_blackboardData);

            _behaviourTree.SetVariableValue(BEHAVIOUR_BLACKBOARD_DATA_PARAM_NAME, _blackboardData);
        }

        private void CalculateNearestWaypoints(GameData data)
        {
            //Add all waypoint
            _blackboardData.WaypointsData.Clear();
            foreach (WayPointView wayPoint in data.WayPoints)
            {
                //Get nearest waypoint
                WayPointView nearestWayPoint = null;
                float smallestDist = float.MaxValue;
                foreach (WayPointView other in data.WayPoints)
                {
                    if (wayPoint == other)
                        continue;

                    if (other.Owner == _blackboardData.SelfOwner)
                        continue;

                    //Avoid double reference
                    //if(_blackboardData.WaypointsData.ContainsKey(other))
                    //{
                    //    if (_blackboardData.WaypointsData[other].NextWaypoint == wayPoint)
                    //        continue;
                    //}

                    float dist = Vector2.Distance(wayPoint.Position, other.Position);
                    if (dist < smallestDist)
                    {
                        smallestDist = dist;
                        nearestWayPoint = other;
                    }
                }

                //construct waypointData
                BattleStarWayPointData waypointData = new BattleStarWayPointData
                {
                    NextWaypoint = nearestWayPoint,
                    IsClose = (smallestDist <= _scorerWaypointDistance)
                };
                _blackboardData.WaypointsData[wayPoint] = waypointData;
            }
        }
    }
}

