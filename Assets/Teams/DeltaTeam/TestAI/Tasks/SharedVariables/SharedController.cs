using BehaviorDesigner.Runtime;
using DeltaTeam;

namespace DeltaTeam.Tasks.Actions
{
    public class SharedController : SharedVariable<VolodAITestController>
    {
        public static implicit operator SharedController(VolodAITestController value)
        {
            return new SharedController { mValue = value };
        }
    }
}
