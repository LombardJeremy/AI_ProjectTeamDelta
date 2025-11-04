using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class ResetInputData : Action
    {
        public SharedDeltaController Controller;

        public override string OnDrawNodeText()
        {
            return "Start frame without shooting, dropping mines or firing shockwaves";
        }

        public override TaskStatus OnUpdate()
        {
            if (Controller.Value != null)
            {
                Controller.Value.InputData.dropMine = false;
                Controller.Value.InputData.fireShockwave = false;
                Controller.Value.InputData.shoot = false;
            }

            return TaskStatus.Success;
        }
    }
}
