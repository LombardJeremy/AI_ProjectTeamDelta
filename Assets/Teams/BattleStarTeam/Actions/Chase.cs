using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar")]
    public class Chase : Action
    {
        // Rotate vers l'ennemie, dès que la rot est bonne, avancer
        // Si objet ou mine, prends par rapport à la pos et radius le chemin à prendre, et rotate vers un point en dehors de cette radius
        // Si rotate pas bonne, rotate vers le spaceShipChase

        private SpaceShipView spaceShip;
        private SpaceShipView spaceShipChase;

        public float distanceToChase = 10.0f;

        public override TaskStatus OnUpdate()
        {
            SelectSpaceshipChase();

            // Target Position to chase
            //spaceShipChase.Position;

            if (Vector2.Distance(spaceShip.Position, spaceShipChase.Position) < distanceToChase) return TaskStatus.Success;

            return TaskStatus.Running;
        }

        private void SelectSpaceshipChase()
        {
            float distClosest = float.MaxValue;

            foreach (SpaceShipView ship in GameManager.Instance.GetGameData().SpaceShips)
            {
                if (ship.Owner != spaceShipChase.Owner)
                {
                    float dist = Vector2.Distance(spaceShipChase.Position, ship.Position);
                    if (dist < distClosest)
                    {
                        distClosest = dist;
                        spaceShipChase = ship;
                    }
                }
            }
        }
    }
}
