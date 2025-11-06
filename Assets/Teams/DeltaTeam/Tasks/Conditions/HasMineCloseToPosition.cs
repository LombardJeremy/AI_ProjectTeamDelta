using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class HasMineCloseToPosition : Conditional
    {
        public SharedDeltaController Controller;
        public SharedVector2 TargetPosition;
        public SharedVector2 ReturnedMinePosition;
        public float Radius = 0.75f;
        public bool bSkipUnactiveMine = true;
        
        public override string OnDrawNodeText()
        {
            if (Controller.Value == null) return "Controller missing !";
            return base.OnDrawNodeText();
        }

        public override TaskStatus OnUpdate()
        {
            List<MineView> mines = Controller.Value.GameData.Mines;
            foreach (MineView mine in mines)
            {
                if (Vector2.Distance(mine.Position, TargetPosition.Value) < Radius)
                {
                    if (bSkipUnactiveMine && !mine.IsActive) continue;
                    ReturnedMinePosition.Value = mine.Position;
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Failure;
        }
    }
}
