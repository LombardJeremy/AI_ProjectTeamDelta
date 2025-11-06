using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;


namespace BattleStarTeam
{
    [TaskCategory("Battlestar/Conditions")]
    public class IsGoingToHitMine : Conditional
    {
        public SharedBattleStarData Data;

        [Tooltip("60° = 30° de chaque coté")]
        public SharedFloat FieldOfView;

        [Tooltip("Distance pour considérer une mine \"Trop proche\"")]
        public SharedFloat MinimumDistance;

        [Tooltip("Distance qu'il faut pour pas tirer sur sa propre mine.")]
        public SharedFloat MaximumDistance;
        public override TaskStatus OnUpdate()
        {
            foreach(Vector2 minePosition in Data.Value.NearbyMines)
            {

                Vector2 dir = minePosition - Data.Value.SelfPosition;
                float angleToMine = Vector2.Angle(Data.Value.SelfLookAt, dir.normalized);
                float distanceToMine = Vector2.Distance(Data.Value.SelfPosition, minePosition);
                if (angleToMine <= FieldOfView.Value / 2f && distanceToMine <= MinimumDistance.Value && distanceToMine > MaximumDistance.Value)
                    return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
