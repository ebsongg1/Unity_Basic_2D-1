using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Study.UGUI
{

    public class ui : MonoBehaviour
    {
        // 스크립트를 이용해 여러 캔버스를 제어하는 방법
        // 1. 키입력 제어
        // 2. 버튼을 사용해서 UI 입력을 통해 제어

        public enum CanvasType
        {
            CanvasA, 
            CanvasB, 
            CanvasC, 
            CanvasD
        }

        private Canvas[] canvases;
        private Canvas menuCanvas;

        private void Awake()
        {
            canvases = GetComponentsInChildren<Canvas>();

            // 여러 캔버스 중 메뉴컨버스 게임오브젝트를 골라서 메뉴컨버스에 할당해줌. 해당 캔버스는 언제나 활성화

            for(int i = 0; i < canvases.Length; ++i)
            {
                if (canvases[i].gameObject.name.Equals("MenuCanvas"))
                {
                    menuCanvas = canvases[i];
                    break;
                }
            }

            SetActiveCanvas(CanvasType.CanvasA);

        }

        private void Update()
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                SetActiveCanvas(CanvasType.CanvasA);
            }
        }

        private void SetActiveCanvas(CanvasType canvasType)
        {
            for (int i = 0;i < canvases.Length; ++i)
            {
                canvases[i].enabled = false;
            }

            // 켜줘야할 녀석들만 켜줌
            menuCanvas.enabled = true;
        }

        private void SetActiveCanvas(int canvasType)
        {

        }

        public void EmptyFunction()
        {

        }

        private Button[] buttons;

        private void SetButtons()
        {
            buttons = menuCanvas.transform.GetComponentsInChildren<Button>();

            for (int i = 0; i < buttons.Length; ++i)
            {
                int index = i;

                buttons[i].onClick.AddListener(() => SetActiveCanvas(index));

                // 람다 표현식 : 프로그래밍에서 함수를 하나의 식으로 간결하게 표현하는 방법
                // 익명함수, 무명함수(이름이 없는 함수)라고도 부르고, 코드의 가독성을 높이지만, 비용(메모리를 사용해서 가비지)이 소모됨

                // 캡쳐 : 람다가 선언된 범위 밖의 외부 변수를 람다 내부로 가져와서 사용하는 동작을 의미
                //        값 캡쳐와 참조 캡쳐가 있어서 캡쳐가 일어날 경우 의도치 않은 버그가 발생할 수 있음
            }

            for (int i = 0; i <= buttons.Length; ++i)
            {
                if(i % 2 == 0)
                {
                    buttons[i].interactable = true;
                }
            }
        }

    }
}
