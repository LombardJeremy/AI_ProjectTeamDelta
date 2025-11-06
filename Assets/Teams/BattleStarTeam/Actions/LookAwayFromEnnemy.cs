using BattleStarTeam;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("BattleStar/Angle")]
    public class LookAwayFromEnnemy : Action
    {
        public SharedInputData InputData;
        public SharedBattleStarData Data;

        [Tooltip("Degré a partir du quel l'angle est considéré comme bonne")]
        public SharedFloat AcceptanceThreshold;
        public override TaskStatus OnUpdate()
        {
            InputData newInputData = InputData.Value;
            Vector2 dir = Data.Value.EnnemyPosition - Data.Value.SelfPosition;

            if (dir.sqrMagnitude < Mathf.Epsilon)
                return TaskStatus.Success;

            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            targetAngle = NormalizeAngle(targetAngle + 180f);
            newInputData.targetOrientation = targetAngle;
            InputData.SetValue(newInputData);

            float delta = Mathf.DeltaAngle(Data.Value.SelfRotation, targetAngle);
            return (Mathf.Abs(delta) <= AcceptanceThreshold.Value) ? TaskStatus.Success : TaskStatus.Running;
        }

        private float NormalizeAngle(float a)
        {
            a = a % 360f;
            if (a < 0f) a += 360f; 
            return a;
        }
    }
}

