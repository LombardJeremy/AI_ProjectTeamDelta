using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace DeltaTeam.Tasks.Actions
{
    [TaskCategory("DeltaTeam")]
    public class Shoot : Action
    {
        public SharedDeltaController Controller;
        public bool bShouldShoot = true;

        public override string OnDrawNodeText()
        {
            return bShouldShoot ? "Start Shooting" : "Stop Shooting";
        }

        public override TaskStatus OnUpdate()
        {
            if (Controller.Value != null)
            {
                Controller.Value.InputData.shoot = bShouldShoot;
            }

            return TaskStatus.Success;
        }
    }
}
