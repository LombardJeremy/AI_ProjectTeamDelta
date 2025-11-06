using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetBestWaypoint : Action
    {
        public SharedDeltaController SharedController;
        public SharedVector2 BestWaypointPosition;
        
        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            ScoreManager scoreManager = controller.GetComponent<ScoreManager>();
            if (!scoreManager) return TaskStatus.Failure;
            WayPointView tmpPos = scoreManager.GetWaypointWithLessWeight();
            if (tmpPos != null)
            {
                BestWaypointPosition.Value = tmpPos.Position;
            }
            return TaskStatus.Success;
        }
    }
}