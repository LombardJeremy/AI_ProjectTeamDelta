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
        
        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            ScoreManager scoreManager = controller.GetComponent<ScoreManager>();
            if (!scoreManager) return TaskStatus.Failure;
            scoreManager.CalculateAllWeightAtPlayerPos();
            return TaskStatus.Success;
        }
    }
}