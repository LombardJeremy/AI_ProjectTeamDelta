using BehaviorDesigner.Runtime;
using DoNotModify;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleStarTeam {

	public class BattleStarController : BaseSpaceShipController
	{

		private Navigation nav;


		private const string BEHAVIOUR_INPUT_DATA_PARAM_NAME = "InputData";

		[SerializeField] private InputData _inputData;
		[SerializeField] private Vector2 _target;

		public InputData InputData { get => _inputData; set => _inputData = value; }
		public Vector2 Target { get => _target; }

		[Header("Reference")]
		[SerializeField] BehaviorTree _behaviourTree;
		[SerializeField] BattleStarData _dataContainer;

        [Header("Settings")]
        [SerializeField] LayerMask asteroidLayer;
        [SerializeField] LayerMask mineLayer;

        [SerializeField] float avoidDistance = 1f;

        [SerializeField] float radiusDetectionAsteroid = 3f;
        [SerializeField] float distanceDetectionAsteroid = 3f;
        [SerializeField] float rotationDetectionAsteroid = 3f;

        [SerializeField] float rotationDetectionMine = 3f;
        [SerializeField] float radiusDetectionMine = 3f;
        [SerializeField] float distanceDetectionMine = 3f;
        [SerializeField] float distByRotationReduceMine = 0.1f;
        [SerializeField] float distByRotationReduceAsteroid = 0.1f;

        [SerializeField] bool clockWiseOnly = false;
        [SerializeField] float maxDistanceThrust = 3f;
        [SerializeField] AnimationCurve easing;

        public override void Initialize(SpaceShipView spaceship, GameData data)
		{
            _dataContainer.InitializeBlackboardData(spaceship, data);
            SetupNav(spaceship);
        }

        private void SetupNav(SpaceShipView spaceship)
        {
            nav = gameObject.AddComponent<Navigation>();
            nav.spaceShip = spaceship;
            nav.asteroidLayer = asteroidLayer;
            nav.mineLayer = mineLayer;

            nav.radiusDetectionAsteroid = radiusDetectionAsteroid;
            nav.radiusDetectionMine = radiusDetectionMine;

            nav.distanceDetectionAsteroid = distanceDetectionAsteroid;
            nav.distanceDetectionMine = distanceDetectionMine;

            nav.offsetAvoidance = avoidDistance;

            nav.clockWiseOnly = clockWiseOnly;

            nav.maxDistThrust = maxDistanceThrust;
            nav.thrustCurve = easing;

            nav.rotationDetectionAsteroid = rotationDetectionAsteroid;
            nav.rotationDetectionMine = rotationDetectionMine;

            nav.distByRotationReduceMine = distByRotationReduceMine;
            nav.distByRotationReduceAsteroid = distByRotationReduceAsteroid;
        }

        public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
            nav.SetNavigation(((SharedTargetData)_behaviourTree.GetVariable("Target")).Value);
            //nav.target = GameManager.Instance.GetGameData().SpaceShips.Find(x => x.Owner != spaceship.Owner).Position;

            //Reset le shoot / landmine / shockwave pour pas les lancer a chaque frame
            if (spaceship.HasShot)
				_inputData.shoot = false;
            if (spaceship.HasDroppedMine)
                _inputData.dropMine = false;
            if (spaceship.HasFiredShockwave)
                _inputData.fireShockwave = false;

            //Mettre a jour les données dans la data
            _dataContainer.UpdateData(spaceship, data, asteroidLayer);

            InputData newInputData = ((SharedInputData)_behaviourTree.GetVariable(BEHAVIOUR_INPUT_DATA_PARAM_NAME)).Value;
            _inputData.shoot = newInputData.shoot; 
            _inputData.dropMine = newInputData.dropMine; 
            _inputData.fireShockwave = newInputData.fireShockwave;
			_inputData.thrust = nav.GetThrustValue();
			
            if(newInputData.targetOrientation >= 0) //On se sert de inputData pour réécrire l'angle de rotation
                _inputData.targetOrientation = newInputData.targetOrientation;
            else
                _inputData.targetOrientation = spaceship.Orientation + nav.GetRotationValue();

            return _inputData;
		}
    }
}
