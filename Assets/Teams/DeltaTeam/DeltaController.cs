using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;

namespace DeltaTeam {

	public struct TimeSinceLastAction
	{
		
		public float Shoot;
		public float DropMine;
		public float Shockwave;

		public TimeSinceLastAction(bool b)
		{
			Shoot = -1000f;
			DropMine = -1000f;
			Shockwave = -1000f;
		}
	}
	public class DeltaController : BaseSpaceShipController
	{
		public InputData InputData;
		public SpaceShipView OtherSpaceShip;
		public SpaceShipView OwnSpaceShip;
		public GameData GameData;
		public GameObject PositionPreviewPrefab;
		public TimeSinceLastAction TimeSinceLastAction;
		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			OtherSpaceShip = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			OwnSpaceShip = spaceship;
			GameData = data;
			TimeSinceLastAction = new TimeSinceLastAction(true);
		}

		public void PreviewPosition(Vector2 position)
		{
			GameObject preview = Instantiate(PositionPreviewPrefab, position, Quaternion.identity);
			Destroy(preview, 1);
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			GameData = data;
			
			if (InputData.shoot) TimeSinceLastAction.Shoot = Time.time;
			if (InputData.dropMine) TimeSinceLastAction.DropMine = Time.time;
			if (InputData.fireShockwave) TimeSinceLastAction.Shockwave = Time.time;
			
			/*SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			float targetOrient = otherSpaceship.Orientation;
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			return new InputData(thrust, targetOrient, needShoot, false, false);*/

			return InputData;
		}
	}

}
