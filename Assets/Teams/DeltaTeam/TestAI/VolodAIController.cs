using System;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam {

	public class VolodAITestController : BaseSpaceShipController
	{
		public InputData InputData;
		public SpaceShipView OtherSpaceShip;
		public SpaceShipView OwnSpaceShip;
		public GameData GameData;

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			OtherSpaceShip = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			OwnSpaceShip = spaceship;
			GameData = data;
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			GameData = data;
			/*
			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			float targetOrient = otherSpaceship.Orientation;
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			return new InputData(thrust, targetOrient, needShoot, false, false);*/
			//float angle = Mathf.Atan2(OtherSpaceShip.Position.y - transform.position.y, OtherSpaceShip.Position.x - transform.position.x) * Mathf.Rad2Deg;
			//_inputData.targetOrientation = angle;
			return InputData;
		}
	}

}
