using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class RecalculateWaypointWeight : Action
    {
        public SharedDeltaController SharedController;
        public SharedFloat OwnWeight;
        public SharedFloat EnnemyWeight;
        
        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            ScoreManager scoreManager = controller.GetComponent<ScoreManager>();
            if (!scoreManager) return TaskStatus.Failure;
            scoreManager.CalculateAllWeightAtPlayerPos(OwnWeight.Value, EnnemyWeight.Value);
            return TaskStatus.Success;
        }
    }
}