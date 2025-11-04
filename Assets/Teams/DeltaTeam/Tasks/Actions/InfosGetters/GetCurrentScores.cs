using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetCurrentScores : Action
    {
        public SharedDeltaController SharedController;
        public SharedFloat OwnScore;
        public SharedFloat OwnCombatScore;
        public SharedFloat OwnWaypointScore;
        public SharedFloat EnemyScore;
        public SharedFloat EnemyCombatScore;
        public SharedFloat EnemyWaypointScore;
        
        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            
            OwnScore.Value = controller.OwnSpaceShip.Score;
            OwnCombatScore.Value = controller.OwnSpaceShip.HitScore;
            OwnWaypointScore.Value = controller.OwnSpaceShip.WaypointScore;
            
            EnemyScore.Value = controller.OtherSpaceShip.Score;
            EnemyCombatScore.Value = controller.OtherSpaceShip.HitScore;
            EnemyWaypointScore.Value = controller.OtherSpaceShip.WaypointScore;
            
            return TaskStatus.Success;
        }
    }
}