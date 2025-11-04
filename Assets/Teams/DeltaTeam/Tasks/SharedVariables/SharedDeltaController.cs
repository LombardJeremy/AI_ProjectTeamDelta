using BehaviorDesigner.Runtime;
using DeltaTeam;

namespace DeltaTeam.Tasks.Actions
{
    public class SharedDeltaController : SharedVariable<DeltaController>
    {
        public static implicit operator SharedDeltaController(DeltaController value)
        {
            return new SharedDeltaController { mValue = value };
        }
    }
}
