using UnityEngine;

namespace study_actionplatformer
{ 
    // jumpstate는 Animator의 AnyState(모든 상태) 기능 활용하여 구현함.

    // # AnyState(모든 상태)?
    // - 항상 표시(출력, 재생) 가능한 특수한 상태임.
    // 현재 존재하는 상태와 상관없이 특정 상태로 이동하려는 상황에 사용
    // - 상태머신(Animator)에 특수한 전환을 제어하는 방법.
    // - AnyState는 반복적인 애니메이션 상태제어에는 사용하지 않음
    // ex) Idle, Run 등의 지속적으로 Loop되는 애니메이션에는 사용 안 한다는 말
    public class jumpstate : playeranimstatebase
    {
        public jumpstate(playercontroller owner) : base(owner) { }

        public override void UpdateState(AnimatorStateInfo stateInfo)
        {
            // 점프 상태에서도 입력 허용.
            Owner.HandleMovement();
        }
    }
}