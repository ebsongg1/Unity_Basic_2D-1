using UnityEngine;

namespace Study.MergeGame
{
    public class AnimalBall : MonoBehaviour
    {
        public int level = 1;

        public bool hasFirstContact = false;
        public bool isMerged = false;

        private Rigidbody2D rBody;

        private void Awake()
        {
            rBody = GetComponent<Rigidbody2D>();
            //  gravityScale은 중력의 영향을 얼마나 받는지 결정하는 변수입니다
            rBody.gravityScale = 0.0f; 
        }

        public void Drop()
        {
            rBody.gravityScale = 1.0f;

            // transform.SetParent()
            // : 부모자식 관계를 형성해주는 함수 입니다. null 입력시
            //  부모 transform이 없는 월드 최상위 개체가 됩니다
            transform.SetParent(null);
        }

        // OnCollision 시리즈
        // : OnTrigger와 함께 사용되는 물리 감지 이벤트 함수 입니다
        // rigidbody를 통해서 감지 할 수 있습니다

        // OnCollisionEnter2D
        // : Trigger가 아닌 Collider와 물리적 충돌 했을때 호출되는 이벤트 함수입니다

        // OnCollisionStay2D
        // : 물리적 충돌이 유지되고 있을때 호출되는 이벤트 함수 입니다

        // OnCollisionExit2D
        // : 물리적 충돌이 종료 되었을때 호출되는 이벤트 함수 입니다


        private void OnCollisionEnter2D(Collision2D collision)
        {
            hasFirstContact = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
        }
    }
}

