using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class DebugLog : Action
    {
        public string DebugMessage;

        public override TaskStatus OnUpdate()
        {
            Debug.Log(DebugMessage);
            return TaskStatus.Success;
        }
    }
}
