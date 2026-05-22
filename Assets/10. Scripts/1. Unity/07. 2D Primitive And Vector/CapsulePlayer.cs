using UnityEngine;
using UnityEngine.InputSystem;

namespace Study.PrimitiveAndVector
{
    public class CapsulePlayer : MonoBehaviour
    {
        // 캡슐 플레이어
        // 1. 화살표(좌,우)를 이용한 이동 및 표현
        // 2. Space버튼 이용한 점프
        // 3. Platform이라는 지형 위에서 움직여야 합니다.

        public enum State
        {
            Idle = 0,   // 대기 상태
            Left,       // 왼쪽으로 가는 상태
            Right       // 오른쪽으로 가는 상태
        }

        public GameObject[] SunGlasses;
        private State currentState = State.Idle;
        public float speed = 2.0f;

        private Rigidbody2D rBody;
        private Collider2D col;
        //public float jumpPower = 100;

        private void Awake()
        {
            rBody = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();

            // 혹시 모르니 Awake에서 Kinematic으로 초기화 해줍니다
            // RigidbodyType 중 Kinematic은 "물리 엔진이 마음대로 못 건드리는 몸"
            // 오직 우리가 시키는 대로만 움직입니다
            // => 운동을 100% 직접 제어하기 위함.
            rBody.bodyType = RigidbodyType2D.Kinematic;
        }
        
        private void FixedUpdate()
        {
            if(Keyboard.current.leftArrowKey.isPressed)
            {
                SetSunGlassState(State.Left);
                Move(Vector3.left);
            }
            else if (Keyboard.current.rightArrowKey.isPressed)
            {
                SetSunGlassState(State.Right);
                Move(Vector3.right);
            }
            else
            {
                SetSunGlassState(State.Idle);
            }

            if(Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Jump();
            }


            ApplyGravaity();

            Vector3 moveVector = Vector3.zero;
            moveVector.y = verticalVelocity;
            rBody.MovePosition(transform.position + moveVector);
        }

        private void SetSunGlassState(State state)
        {
            if (currentState == state) return;
            // 상태가 전환될때만 아래 로직이 실행되도록
            // 예외처리를 합니다.

            SunGlasses[(int)currentState].SetActive(false);
            SunGlasses[(int)state].SetActive(true);
            currentState = state;
        }

        private void Move(Vector3 dir)
        {
            //transform.Translate(dir * speed * Time.deltaTime);
            //transform.position += (dir * (speed * Time.deltaTime));
            // Vector3             Vector3 * (float: distance)

            // 이번 프레임에 움직일 벡터의 크기 : 이번 프레임 이동량
            Vector3 moveVector = dir * (speed * Time.fixedDeltaTime);
            
            // 내 위치와 이동량을 더해줍니다
            rBody.MovePosition(transform.position + moveVector);
        }

        private void Jump()
        {
            // AddForce를 사용하지 않고 점프를 구현해보세요
            // moveVector에 수평적 움직과 점프의 수직적 움직임을 통합
            // 하면 됩니다
            rBody.AddForceY(jumpPower, ForceMode2D.Impulse);
        }

        // 1. 중력을 구현
        // 2. 지형에 안착시키기
        // 3. 점프를 구현하기
        // 4. 끼임현상 없애기

        [Header("Settings")]
        public float gravity = -9.81f; // 중력 가속도(-9.81 = 현실세계)
        public float jumpPower = 8.0f; // 빨간줄 뜨면 안적어도됨
        public int maxJumpCount = 2;

        // 내부 상태 변수들
        private float verticalVelocity = 0.0f; // 실시간 수직 가속 운동 값
        private bool isGrounded = false; // 땅에 닿았는지 체크 하는 용도

        // # 중력 적용
        private void ApplyGravaity()
        {
            const float groundStickSpeed = -2.0f;

            // 바닥에 있는 상태이면서 위로 올라가는 힘(점프)이 없다면
            if(isGrounded && verticalVelocity <= 0)
            {
                // 캐릭터가 바닥에 잘 붙어 줄수 있게 Damping값을 넣어 줍니다.
                verticalVelocity += groundStickSpeed;
            }
            else // 공중에 있는 상태
            {
                // 물리엔진에 영향을 주는 코드라서 Time.fixedDeltaTime을 사용합니다.
                // 속도 = 속도 + 가속도 * 시간 (매 프레임마다 지속적으로 감소 합니다)
                verticalVelocity += gravity * Time.fixedDeltaTime;
            }
        }

    }
}