using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Study.Utilities;
using utilltly;
using System.Runtime.CompilerServices;

namespace study_actionplatformer
{
    // 조작, 애니메이션 재생을 담당하는 클래스

    // [책임 분리]
    // - PlayerController : '무엇을 할지' 결정하는 클래스. 입력을 읽고, 상태를 갈아끼우고, 공용 동작을 제공 
    // - CharacterController2D : '어떻게 움직일지'를 결정하는 클래스임
    // - 각종 states 들 : 애니메이션 실행 시 상태 별 행동들이 정의되어 있음

    // PlayerController는 상태패턴 기반의 FSM(유한상태기계)임
    // 아래 내용의 구조를 FollewerFSM 이라고 부름
    // 아래의 상태머신의 전이(Transition)의 주인은 애니메이터로 설정되어 있음
    // 매 프레임 현재 애니메이터의 애니메이션을 읽어서 그에 맞는 상태 클래스를 실행시키도록 되어있음. 
    // 그래서 전이를 결정하는 로직이 없음. (원래 FSM은 Transition이 중요함)

    public class playercontroller : MonoBehaviour
    {
        // 애니메이터 상태 Tag
        // - 애니메이터의 상태마다 지정할 수 있는 string Tag
        // - 컴포넌트에서는 해당 정보를 읽어서 현재 애니메이션 상태를 추측할 수 있음

        private const string ANIM_TAG_ATTACK = "Attack";
        private const string ANIM_TAG_ATTACK_END = "Attack_End";
        private const string ANIM_TAG_MOVEMENT = "Movement";
        private const string ANIM_TAG_FIRE = "Fire";
        private const string ANIM_TAG_JUMP = "Jump";

        // 애니메이터 파라미터 해시
        // - string을 이용해서 파라미터를 전달하면 가비지가 생기며, 실수(오타)가 많이 일어남
        // 특정 string값을 애니메이터 별로 Hash 값으로 치환하여 애니메이터 파라미터로 사용
        // - static readonly : 상수처럼 다루고 싶어서 사용하는 키워드

        public static readonly int MOVEMENT = Animator.StringToHash("Movement");
        public static readonly int IS_ATTACK = Animator.StringToHash("IsAttack");
        public static readonly int IS_GROUNDED = Animator.StringToHash("IsGrounded");
        public static readonly int JUMP = Animator.StringToHash("Jump");
        public static readonly int DESCENDING = Animator.StringToHash("Descending");
        public static readonly int IS_FIRE = Animator.StringToHash("IsFire");

        public Animator Animator { get; private set; }
        private SpriteRenderer SpriteRenderer { get; set; }
        private charactercontroller2d controller2D { get; set; }

        // 바라보는 방향. +1 = 오른쪽, -1은 = 왼쪽으로 처리함
        public int FacingDirection { get; private set; } = 1;

        // 각 상태를 제어하기 위한 멤버 변수들
        private Dictionary<int, playeranimstatebase> StateDic { get; set; } = new();
        private playeranimstatebase defaultState; // 예외처리를 위한 기본 상태
        private playeranimstatebase currentState; // 상태객체를 담아서 제어하기 위한 빈 객체 변수 

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            controller2D = GetComponent<charactercontroller2d>();

            defaultState = new movementstate(this);

            StateDic.Add(Animator.StringToHash(ANIM_TAG_MOVEMENT), defaultState);
            StateDic.Add(Animator.StringToHash(ANIM_TAG_ATTACK), new attackstate(this));
            StateDic.Add(Animator.StringToHash(ANIM_TAG_ATTACK_END), new attackendstate(this));
            StateDic.Add(Animator.StringToHash(ANIM_TAG_JUMP), new jumpstate(this));


        }

        private void Update()
        {
            UpdateAnimState();

            // 판정에 의한 애니메이션 재생용도라서 Update()에서 호출함
            UpdateJumpInput();
            UpdateGroundedAnimation();
        }

        private void UpdateAnimState()
        {
            AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            // - animator의 상태 정보를 담고 있는 구조체

            if(StateDic.TryGetValue(stateInfo.tagHash, out playeranimstatebase nextState) == false)
            {
                // 검색해서 없으면 nextState = defaultState 처리
                nextState = defaultState;
            }

            // 상태가 변경이 되었다면
            if(currentState != nextState)
            {
                currentState.Exit(); // ?. : null 체크하는 키워드. null이 아닐 때만 호출한다는 표현
                // => if(currentState != null) currentState.Exit()과 같은 표현
                currentState = nextState;
                nextState.Enter();
            }

            currentState.UpdateState(stateInfo);
        }

        // 아래부터는 상태 클래스들이 사용하는 공용함수들임

        public void HandleMovement()
        {
            Vector2 inputVector = SimpleInput.GetMoveAxisRaw();
            float absMovement = Mathf.Abs(inputVector.x);
            Animator.SetFloat(MOVEMENT,absMovement);

            if(absMovement > 0) // 이동량이 있을 때
            {
                // 삼항 연산자로 정면을 설정해줌
                FacingDirection = (inputVector.x < 0) ? -1 : 1;

                // x 스케일을 반전해서 좌우를 뒤집음
                // (히트박스를 뒤집기 위해서 스케일 반전을 사용)
                Animator.transform.localScale = new Vector3(FacingDirection, 1, 1);
            }

            controller2D.SetMoveInput(inputVector.x);

            if(SimpleInput.GetKeyDown(Key.Z))
            {
                Animator.SetBool(IS_ATTACK, true);
            }

            if(SimpleInput.GetKeyDown(Key.X))
            {
                Animator.SetBool(IS_FIRE, true);
            }
        }

        public void StopMovement()
        {
            controller2D.SetMoveInput(0.0f);
        }

        // 점프는 AnyState로 동작하기 때문에 상태머신에서 입력 처리를 함.
        // 상태 객체에게 호출을 위임해도 무방.
        private void UpdateJumpInput()
        {
            if(SimpleInput.GetKeyDown(Key.Space))
            {
                // 점프가 가능한 상태인지는 CharacterController가 판단.
                controller2D.RequestJump();
            }
        }

        private bool prevIsGrounded = false; 

        /// <summary>
        /// CharacterCotroller2D의 접지 상태를 애니메이터로 옮겨주는 함수
        /// </summary>
        private void UpdateGroundedAnimation()
        {
            bool isGrounded = controller2D.IsGrounded;
            Animator.SetBool(IS_GROUNDED, isGrounded);

            // 이전 프레임에서는 접지(바닥에 닿아있는) 상태였다가
            // 현재 프레임에서는 접지 하지 않는 상태가 되었을 때
            if(prevIsGrounded && (isGrounded == false))
            {
                // VerticalVelocity가 0보다 크면 => 상승
                if (controller2D.VerticalVelocity >  0.0f)
                {
                    Animator.SetTrigger(JUMP);
                }
                // 아니면 하강
                else
                {
                    Animator.SetTrigger(DESCENDING);
                }
            }

            prevIsGrounded = isGrounded;
        }
    }
}
