using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class SubtractDeltaTime : Action
    {
        public SharedFloat variable;
        
        public override TaskStatus OnUpdate()
        {
            variable.Value -= Time.deltaTime;
            return TaskStatus.Success;
        }
    }
}