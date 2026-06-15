using UnityEngine;

namespace study_actionplatformer
{

    public class attackendstate : playeranimstatebase
    {
        public attackendstate(playercontroller owner) : base(owner)
        {

        }

        public override void Enter()
        {
            Owner.StopMovement();
        }

        public override void UpdateState(AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime > INPUT_RESET_TIME)
            {
                Animator.SetBool(playercontroller.IS_ATTACK, false);
            }
        }
    }
}
