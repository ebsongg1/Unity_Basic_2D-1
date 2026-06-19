using System.Collections;
using UnityEngine;
using study_utilities;
using Unity.VisualScripting;

namespace study_actionplatformer
{

    public class EnemyController : MonoBehaviour
    {
        // 코루틴을 이용한 FSM 만들기
        // 코루틴의 yield return StartCoroutine(코루틴); 함수를 이용해서 코루틴끼리 연결되어
        // 끊임없이 순환하는 구조의 소규모 인공지능 캐릭터를 만들기

        private static readonly int IS_MOVE = Animator.StringToHash("IsMove");
        private static readonly int ATTACK = Animator.StringToHash("Attack");

        private const float ATTACK_HIT_DELAY = 0.5f;
        private const float ATTACK_COOLDOWN = 2.0f;

        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float traceRange = 10.0f;
        [SerializeField] private float attackRange = 3f;
        [SerializeField] private float baseUpdateTerm = 0.1f;

        [SerializeField] private Transform patrolPoint;

        [SerializeField] private GameObject deadEffect;
        [SerializeField] private Vector3 deadEffectOffset;
        [SerializeField] private float deadEffectLifeTime = 0.5f;

        private Animator Animator { get; set; }
        private Enemy Enemy { get; set; }

        [field: SerializeField] public Transform Target;

        private Vector3 pointA, pointB, goalPoint;
        private Vector3 originalScale;

        // 빈 코루틴 필드 (빈 객체랑 동일함)
        protected IEnumerator nextStateCoroutine;

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            Enemy = GetComponentInChildren<Enemy>();
            originalScale = transform.localScale;

            pointA = transform.position;
            pointB = patrolPoint.position;
            goalPoint = pointB;
        }

        private void OnEnable()
        {
            StartCoroutine(FiniteStateMachineCoroutine());
        }

        // 메인 코루틴 루프
        private IEnumerator FiniteStateMachineCoroutine()
        {
            // 기본상태를 넣어주고 루프를 시작
            // IEnumrator : 유니티의 코루틴 한정으로 특정 코루틴의 진행상태를 저장하는 변수

            nextStateCoroutine = IdleStateCoroutine();

            // 게임 오프젝트가 켜져있다면 반복하는 루프를 구성
            while(gameObject.activeInHierarchy)
            {
                // yield return StartCoroutine(코루틴);
                // : 매개변수로 주어진 코루틴이 종료될 때까지 처리를 양보 => 대기
                yield return StartCoroutine(nextStateCoroutine);
            }

            StartCoroutine(nextStateCoroutine);

            yield return null;
        }

        private IEnumerator IdleStateCoroutine()
        {
            float waitTime = 0.0f;
            const float IDLE_WAIT_TIME = 3.0f; // 이런 변수는 밖으로 빼는 걸 추천

            WaitForSeconds term = new WaitForSeconds(baseUpdateTerm);

            while(true)
            {
                // Idle 상태의 탈출조건
                // 1. 3초가 지났을 때 PatrolState로 전환(Transition)
                // 2. 타깃이 TraceRange 안에 있을 때 AttackState로 전환

                if(IDLE_WAIT_TIME < waitTime)
                {
                    nextStateCoroutine = PatrolStateCoroutine();
                    yield break;
                }

                if(Target != null && transform.IsInRange(Target.position, traceRange))
                {
                    nextStateCoroutine = AttackStateCoroutine();
                    yield break; // 코루틴 자체를 탈출하는 키워드
                }

                yield return null;
                waitTime += baseUpdateTerm;
            }
            
        }

        private IEnumerator PatrolStateCoroutine()
        {
            // Patrol은 목표지점(goalPoint)을 향해 움직임
            // 목표지점에 도달하면 내 다음 목적지 포인트를 갱신하고 Idle 상태로 전환 됨

            const float STOPPING_DISTANCE = 0.1f;

            

            while (true)
            {
                Vector3 adjustGoalPoint = transform.position;
                adjustGoalPoint.x = goalPoint.x;

                if (Target != null && transform.IsInRange(Target.position, attackRange))
                {
                    nextStateCoroutine = AttackStateCoroutine();
                    yield break; // 코루틴 자체를 탈출하는 키워드 입니다
                }

                // 내가 현재 목표지점에 가까운지?
                // - 목표지점이 B인지 ?  A지점으로 갱신 : B지점으로 갱신

                // 목적지에 가까워졌으면
                if (transform.IsInRange(adjustGoalPoint, STOPPING_DISTANCE))
                {
                    // 삼항 연산자를 사용해서 pointB의 위치라면 pointA, 아니라면 pointB 바꿔 줌

                    // 내가 가까운 목표 지점에 따라서 해당 목표지점의 반대 지점으로 바꿔주기
                    goalPoint = transform.IsSamePosition(pointB) ? pointA : pointB;
                    goalPoint = transform.IsSamePosition(pointB, STOPPING_DISTANCE) ? pointA : pointB;
                    nextStateCoroutine = IdleStateCoroutine();
                    Animator.SetBool(IS_MOVE, false);
                    yield break;
                }

                Move(adjustGoalPoint);
                yield return null;
            }
        }

        private IEnumerator AttackStateCoroutine()
        {
            while (true)
            {
                // 1. Target이 사라질 경우
                if(Target == null)
                {
                    nextStateCoroutine = IdleStateCoroutine();
                    Animator.SetBool(IS_MOVE, false);
                    yield break;
                }

                // 2. Target이 범위(추적 범위) 밖으로 이동할 경우
                //  : 타겟이 매우 빠르게 추적 가능한 범위 바깥으로 이동하게 된 케이스
                if(Target.IsInRange(transform.position, traceRange) == false)
                {
                    nextStateCoroutine = IdleStateCoroutine();
                    Animator.SetBool(IS_MOVE, false);
                    yield break;

                }

                // ===========탈출 처리가 끝나면=============
                // 공격하거나 (타겟이 사거리 안에 있으면)
                if(Target.IsInRange(transform.position, attackRange))
                {
                    Animator.SetTrigger(ATTACK);

                    // 공격 모션이 타격 프레임에 도달할 즈음 데미지를 전달함
                    // - 정석은 애니메이션 이벤트 데이터 등을 넣어서 타격 프레임을 정확하게 알아내는게 맞음
                    // - 여기서는 간단하게 시간으로 처리함

                    // 공격 판정을 위한 대기
                    yield return new WaitForSeconds(ATTACK_HIT_DELAY);
                    ProcessAttack();

                    // 공격 전체 쿨타임 대기
                    yield return new WaitForSeconds(ATTACK_COOLDOWN - ATTACK_HIT_DELAY);
                    
                }

                // 이동한다(타겟이 사거리 안에 들어올 때까지)
                Move(Target.position);
                yield return null;
            }

        }

        private IEnumerator DeadStateCoroutine()
        {
            yield return null;
        }

        private void Move(Vector3 goalPosition)
        {
            Animator.SetBool(IS_MOVE, true);
            // 1차원 방향(사이드뷰, 플랫포머이니까)
            float direction = goalPosition.x - transform.position.x;

            // 0.0000001 이런 느낌의 미세한 차이인지 여부를 판단하기 위해 .Approximately 함수 사용
            // 차이가 미세하다면 움직이지 않아도 됨. 움직이면 떨릴 것 같아서 넣어놓음.
            if (Mathf.Approximately(direction, 0.0f)) return;

            // 부호만 받아냄.
            float moveDirection = Mathf.Sign(direction);
            transform.Translate(new Vector3(moveDirection, 0, 0) * (moveSpeed * Time.deltaTime));
            // 방향에 맞춰서 오른쪽/왼쪽 전환(스케일 x값을 이용)
            transform.localScale = (new Vector3(moveDirection * originalScale.x, originalScale.y, originalScale.z));
        }

        private void ProcessAttack()
        {
            Animator.SetBool(IS_MOVE, false);
        }

        


    }
}
