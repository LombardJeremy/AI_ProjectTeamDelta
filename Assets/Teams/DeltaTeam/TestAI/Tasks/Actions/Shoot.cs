using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam/TestAI")]
    public class Shoot : Action
    {
        public SharedController Controller;

        public override TaskStatus OnUpdate()
        {
            if (Controller.Value != null)
            {
                Controller.Value.SetShouldShoot(true);
            }

            return TaskStatus.Success;
        }
    }
}
