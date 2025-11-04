using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class ShockWave : Action
    {
        public SharedDeltaController Controller;

        public override TaskStatus OnUpdate()
        {
            if (Controller.Value != null)
            {
                Controller.Value.InputData.fireShockwave = true;
            }

            return TaskStatus.Success;
        }
    }
}
