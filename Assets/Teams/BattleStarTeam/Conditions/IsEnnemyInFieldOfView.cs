using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

namespace BattleStarTeam
{
    [TaskCategory("Battlestar/Conditions")]
    public class IsEnnemyInFieldOfView : Conditional
    {
        public SharedBattleStarData Data;

        [Tooltip("60° = 30° de chaque coté")]
        public SharedFloat FieldOfViewAngle = 60;


        [Tooltip("regarde derrière plutot que devant")]
        public SharedBool InvertedFOV;

        public override TaskStatus OnUpdate()
        {
            Vector2 selfDir = Data.Value.SelfLookAt;
            if (InvertedFOV.Value)
                selfDir = -selfDir;

            Vector2 dir = Data.Value.EnnemyPosition - Data.Value.SelfPosition;  
            float angleToEnemy = Vector2.Angle(selfDir, dir.normalized);
            return (angleToEnemy <= FieldOfViewAngle.Value / 2f) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}