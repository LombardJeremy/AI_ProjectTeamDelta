using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Mouvement")]
    public class MoveToEnnemy : Action
    {
        public SharedInputData InputDataRef;
        public SharedBattleStarData Data;
        public SharedTargetData Target;


        [Tooltip("La distance minimale pour que le vaissau soit considere comme \"a la position\"")]
        public SharedFloat DistanceThreshold;


        public override TaskStatus OnUpdate()
        {
            //Disable target orientation
            InputData newInput = InputDataRef.Value;
            newInput.targetOrientation = -1;
            InputDataRef.SetValue(newInput);

            TargetData targetData = new TargetData(Data.Value.EnnemyPosition, .2f, Vector2.zero, false);
            Target.SetValue(targetData);

            return (Vector2.Distance(Target.Value.TargetPosition, Data.Value.SelfPosition) <= DistanceThreshold.Value) ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}
