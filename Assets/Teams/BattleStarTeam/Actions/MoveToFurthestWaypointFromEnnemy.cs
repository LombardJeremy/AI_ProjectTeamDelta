using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Mouvement")]
    public class MoveToFurthestWaypointFromEnnemy : Action
    {
        public SharedInputData InputDataRef;
        public SharedBattleStarData Data;

        [Tooltip("La distance minimale pour que le vaissau soit considéré comme \"à la position\"")]
        public SharedFloat DistanceThreshold;

        public SharedVector2 Target;

        public override TaskStatus OnUpdate()
        {
            //Disable target orientation
            InputData newInput = InputDataRef.Value;
            newInput.targetOrientation = -1;
            InputDataRef.SetValue(newInput);

            Target.SetValue(Data.Value.GetFarestWaypointFromEnnemy);

            return (Vector2.Distance(Data.Value.SelfPosition, Target.Value) <= DistanceThreshold.Value) ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}
