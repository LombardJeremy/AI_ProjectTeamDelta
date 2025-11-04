using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class InitValues : Action
    {
        public SharedDeltaController SharedController;
        public SharedVector2 EnemyPosition;
        public SharedVector2 OwnPosition;
        
        public override TaskStatus OnUpdate()
        {
            if (SharedController.Value == null) return TaskStatus.Failure;
            EnemyPosition.Value = SharedController.Value.OtherSpaceShip.Position;
            Debug.Log("Init values" + EnemyPosition);
            return TaskStatus.Success;
        }
    }
}