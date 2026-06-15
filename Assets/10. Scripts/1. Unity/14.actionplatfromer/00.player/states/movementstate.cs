using UnityEngine;

namespace study_actionplatformer
{
    public class movementstate : playeranimstatebase
    {
        public movementstate(playercontroller owner) : base(owner)
        {

        }

        public override void UpdateState(AnimatorStateInfo stateInfo)
        {
            Owner.HandleMovement();
        }
    }
}
