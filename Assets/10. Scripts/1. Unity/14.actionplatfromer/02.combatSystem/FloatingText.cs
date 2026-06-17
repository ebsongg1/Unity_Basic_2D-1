using UnityEngine;
using TMPro;
using System.Collections;
using System;

namespace study_utilities
{
    // # FloatingText?
    // - 미디어나 게임 개발, 모바일 앱 환경 등에서 화면 위에 떠다니거나
    // 둥둥 떠오르는 듯한 텍스트 효과나 위젯 등을 의미함

    // 목표
    // -FloatText의 특정 함수를 이용해서 위로 올라가는 연출과 사라지는 듯한 효과를 주고자 함.

    public class FloatingText : MonoBehaviour
    {
        [field: SerializeField] public float VerticalSpeed { get; private set; } = 1.0f;
        [field: SerializeField] public float LifeTime { get; private set; } = 2.0f;

        private TMP_Text text;
        private Renderer textRenderer;
        private Color originColor;
        private float progressTime = 0.0f;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            textRenderer = GetComponent<Renderer>();

            // 렌더링 순서 정렬인데 3차원 렌더러여서 orderInLayer가 아닌 sortingOrder를 사용.
            // 원리는 같은데 3차원에서 사용
            textRenderer.sortingOrder = 1000; 
        }

        public void Show()
        {

        }

        public void Show(string message, Color color, Vector3 worldPosition)
        {
            text.SetText(message);
            originColor = color;
            transform.position = worldPosition;
            StartCoroutine(AnimateCoroutine());
        }

        private IEnumerator AnimateCoroutine()
        {
            progressTime = 0.0f;
            // 원본 색상을 복사하여 알파만 0인 색상을 만든다
            Color targetColor = originColor;
            targetColor.a = 0.0f;

            while(true)
            {
                if (progressTime > LifeTime) break;

                // 컬러 변경 로직
                Color nowColor = Color.Lerp(originColor, targetColor, progressTime / LifeTime);
                text.color = nowColor;

                // 상승 로직
                transform.Translate(Vector3.up * VerticalSpeed * Time.deltaTime);

                yield return null;
                progressTime += Time.deltaTime;
            }

            gameObject.SetActive(false);
        }

     
    }
}
