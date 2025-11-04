using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace DeltaTeam {

	public class DeltaController : BaseSpaceShipController
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
			/*SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			float targetOrient = otherSpaceship.Orientation;
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			return new InputData(thrust, targetOrient, needShoot, false, false);*/
			return InputData;
		}
	}

}
