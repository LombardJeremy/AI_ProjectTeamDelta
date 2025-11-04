using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class DropMine : Action
    {
        public SharedDeltaController Controller;

        public override TaskStatus OnUpdate()
        {
            if (Controller.Value != null)
            {
                Controller.Value.InputData.dropMine = true;
            }

            return TaskStatus.Success;
        }
    }
}
