using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace utilltly
{
    // requirecomponent 
    // : 아래의 선언된 컴포넌트 클래스가 type of(컴포넌트)의 컴포넌트가 필요하다고 강제하는 키워드임
    // Gameobject에 Attach하게 되면 필요한 컴포넌트를 자동으로 생성
    [RequireComponent(typeof(Rigidbody2D))]

    public class charactercontroller2d : MonoBehaviour
    {

        [Header("Settings")]
        public float speed = 2.0f;
        public float gravity = -9.81f; 
        public float jumpPower = 8.0f; 
        public int   maxJumpCount = 2;
        public float groundCheckDistance = 0.5f;  
        public float skinWidth = 0.02f;
        public LayerMask collisionLayer;

        private Rigidbody2D rBody;
        private Collider2D col;

      
         
        private int jumpCount = 0;
        private bool jumpRequested = false;
        private Vector3 externalDelta;
        private float moveInput = 0.0f;

        // 아래의 두개는 외부에서 조회가 가능해야 함
        // 읽기 전용으로 바꿔줌
        public float VerticalVelocity { get; private set; } = 0.0f;
        
        public bool IsGrounded { get; private set; } = false;

        #region Unity methods

        private void Awake()
        {
            rBody = GetComponent<Rigidbody2D>();
            rBody.bodyType = RigidbodyType2D.Kinematic;

            col = GetComponent<Collider2D>();
       
        }

        private void FixedUpdate()
        {
            IsGrounded = CheckGrounded();

            ApplyGravaity();
            ApplyJump();

            Vector3 desireMove =
               new Vector3(moveInput * speed, VerticalVelocity) * Time.fixedDeltaTime
               + externalDelta;

            externalDelta = Vector3.zero;

            Vector3 moveVector = Vector3.zero;
            moveVector.x = ResolveAxisMovement(desireMove.x, Vector3.right);
            moveVector.y = ResolveAxisMovement(desireMove.y, Vector3.up);

            if (moveVector.y != desireMove.y) VerticalVelocity = 0.0f;

            rBody.MovePosition(transform.position + moveVector);
        }

        #endregion

        #region Public Methods (명령 통로)

        public void SetMoveInput(float horizontal)
        {
            moveInput = Mathf.Clamp(horizontal, -1.0f, 1.0f);
        }

        public void AddExternalMovement(Vector3 delta)
        {
            externalDelta += delta;
        }

        public bool RequestJump()
        {
            if(jumpCount >= maxJumpCount) return false;
            jumpRequested = true;
            return true;
        }

        #endregion

        #region Private Methods (운동 파이프라인)

        private void ApplyGravaity()
        {
            const float groundStickSpeed = -2.0f;

            if (IsGrounded && VerticalVelocity <= 0)
            {
                VerticalVelocity += groundStickSpeed;
                jumpCount = 0;
            }
            else // 공중에 있는 상태
            {
                // 물리엔진에 영향을 주는 코드라서 Time.fixedDeltaTime을 사용합니다.
                // 속도 = 속도 + 가속도 * 시간 (매 프레임마다 지속적으로 감소 합니다)
                VerticalVelocity += gravity * Time.fixedDeltaTime;
            }

        }

        private float ResolveAxisMovement(float move, Vector3 axis)
        {
            if (move == 0.0f) return 0.0f;

            Vector3 dir = move > 0.0f ? axis : -axis;
            float distance = Mathf.Abs(move);
        

            Bounds box = GetCastBox();
            RaycastHit2D hit = Physics2D.BoxCast(
                box.center, box.size, 0.0f, dir, distance, collisionLayer);

            if (hit.collider == null) return move;

  
            float allowed = Mathf.Max(0, hit.distance - skinWidth);

            return Mathf.Sign(move) * allowed;
    
        }

        private Bounds GetCastBox()
        {
            Bounds box = col.bounds; 

            Vector3 size = box.size;
            size.x -= skinWidth * 2;
            size.y -= skinWidth * 2;
            box.size = size;

            return box;
        }

        private void ApplyJump()
        {
            if (jumpRequested == false) return;
            jumpRequested = false; 

         
            if (jumpCount >= maxJumpCount) return;

          
            VerticalVelocity = jumpPower;
            jumpCount++;
            IsGrounded = false; 
        }

        private bool CheckGrounded()
        {
            Bounds box = GetCastBox();
            RaycastHit2D hit = Physics2D.BoxCast(
                box.center, box.size, 0.0f, Vector2.down, groundCheckDistance, collisionLayer);

            bool result = (hit.collider != null);
            return result;
        }


    }
}

    #endregion