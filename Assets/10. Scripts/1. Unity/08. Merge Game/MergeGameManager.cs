using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Study.MergeGame
{
    public class MergeGameManager : MonoBehaviour
    {
        // static 키워드는 랜드마크와 같습니다.
        // 프로그램에서 정적(고정되어있다~)으로 선언되어있어서
        // 전역적인 접근을 허용합니다.
        public static MergeGameManager Instance;

        public AnimalBall[] BallPrefabs;
        public int maxIndexInQueue = 4;

        private Queue<int> ballIndexQueue = new Queue<int>();


        #region Unity Methods
        private void Awake()
        {
            // 약식 싱글톤 선언
            // 특정 한 객체만 남기고 다 지워버린다~
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            //범위 에러 처리
            maxIndexInQueue = Mathf.Min(maxIndexInQueue, BallPrefabs.Length);
            maxIndexInQueue = Mathf.Max(maxIndexInQueue, 0);
            ballIndexQueue.Enqueue(Random.Range(0, maxIndexInQueue));
        }

        #endregion

        // 큐(ballIndexQueue)에서 꺼내온 녀석을 반환 합니다. MergeGameController에서 호출합니다
        public AnimalBall GetNextBall()
        {
            // 인덱스를 하나 뽑아주고
            int dequeueIndex = ballIndexQueue.Dequeue();
            // BallPrefabs의 길이만큼 랜덤한 숫자를 하나 뽑아서 큐에 담아줍니다
            // 한번에 여러개 담는 처리해도 상관없음
            ballIndexQueue.Enqueue(Random.Range(0, maxIndexInQueue));

            AnimalBall ball = GetBall(dequeueIndex);
            return ball;
        }

        private AnimalBall GetBall(int index)
        {
            // 예외처리
            if (index < 0 || BallPrefabs.Length >= index) return BallPrefabs[0];
            return BallPrefabs[index];
        }
    }
}


