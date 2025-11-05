using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class GetCurrentScoreAdvantage : Action
    {
        public SharedDeltaController SharedController;
        public SharedFloat ScoreAdvantage;
        public SharedFloat CombatScoreAdvantage;
        public SharedFloat WaypointScoreAdvantage;

        public override string OnDrawNodeText()
        {
            if (SharedController.Value == null) return "Missing Controller";
            return base.OnDrawNodeText();
        }

        public override TaskStatus OnUpdate()
        {
            DeltaController controller = SharedController.Value;
            
            ScoreAdvantage.Value = controller.OwnSpaceShip.Score - controller.OtherSpaceShip.Score;
            CombatScoreAdvantage.Value = controller.OwnSpaceShip.HitScore - controller.OtherSpaceShip.HitScore;
            WaypointScoreAdvantage.Value = controller.OwnSpaceShip.WaypointScore - controller.OtherSpaceShip.WaypointScore;
            
            return TaskStatus.Success;
        }
    }
}