using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class HasLineOfSight : Action
    {
        public SharedDeltaController SharedController;
        public SharedVector2 TargetPosition;
        

        public override string OnDrawNodeText()
        {
            if (SharedController.Value == null) return "Missing Controller !";
            else return "Has Line Of Sight with " + TargetPosition.Name;
        }

        public override TaskStatus OnUpdate()
        {
            Vector2 spaceShipPosition = SharedController.Value.OwnSpaceShip.Position;
            Vector2 toTarget = TargetPosition.Value - spaceShipPosition;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(spaceShipPosition, TargetPosition.Value - spaceShipPosition,
                (TargetPosition.Value - spaceShipPosition).magnitude, LayerMask.GetMask("Asteroid"));
            if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.CompareTag("Asteroid"))
            {
                return TaskStatus.Failure;
            }
            return TaskStatus.Success;
        }
    }
}