using Study.Utilities;
using study_actionplatformer;
using UnityEngine;
using UnityEngine.InputSystem;

namespace study_actionplatformer
{

    public class attackstate : playeranimstatebase
    {
        public attackstate(playercontroller owner) : base(owner)
        {
            
        }

        public override void Enter()
        {
            // 공격 중엔 이동 금지
            Owner.StopMovement();
        }

        public override void UpdateState(AnimatorStateInfo stateInfo)
        {
            // 상태의 초반부에는 직전 공격 입력을 제거해줌
            // AnimatorStateInfo.normalizedTime
            // - 현재 애니메이션 진행률 나타냄
            // - 시작되지 않았으면 0% = 0.0f
            // - 절반 진행했으면 50% = 0.5f
            // - 끝났으면 100% = 1.0f ( 보통 잘 안쓰임 )

            // 상태의 초반부에는 직전 공격 입력을 제거해줌
            if (stateInfo.normalizedTime > INPUT_RESET_TIME)
            {
                Animator.SetBool(playercontroller.IS_ATTACK, false);
            }
            // 콤보 입력 구간 (30% ~ 90%)
            else if(INPUT_RESET_TIME < stateInfo.normalizedTime && stateInfo.normalizedTime < COMBO_INPUT_END_TIME)
            {
                Animator.SetBool(playercontroller.IS_ATTACK, SimpleInput.GetKeyDown(Key.Z));
            }
        }
    }
}
