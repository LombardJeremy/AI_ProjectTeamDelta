using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam/TestAI")]
    public class SetThrust : Action
    {
        public SharedController Controller;
        public float thrust = 1f;

        public override string OnDrawNodeText()
        {
            return "Set thrust to " + thrust;
        }

        public override TaskStatus OnUpdate()
        {
            if (Controller.Value != null)
            {
                Controller.Value.InputData.thrust = thrust;
            }

            return TaskStatus.Success;
        }
    }
}
