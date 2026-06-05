using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Study.Utilities
{
    // PC 플랫폼에서 사용하는 키보드 마우스 인풋들 묶어놓은
    // 유틸클래스를 작성해 봅시다.
    // 목표는 쉽고 직관적으로 사용할 수 있도록 기능들을
    // 묶어 놓는 것입니다.

    // 아래처럼 작업하는 디자인 패턴을 Wrapper 패턴
    // 이라고 부릅니다. Wrapping 한다고 합니다

    // SimpleInput 클래스는 어디서든 호출할 수 있게
    // static(정적, 고정된, 랜드마크) 키워드를 사용하여
    // 선언 합니다

    public static class SimpleInput
    {
        #region 키보드 입력

        public static bool GetKeyDown(Key key)
        {
            return Keyboard.current[key].wasPressedThisFrame;
        }
        public static bool GetKeyUp(Key key)
        {
            return Keyboard.current[key].wasReleasedThisFrame;
        }
        public static bool GetKey(Key key)
        {
            return Keyboard.current[key].isPressed;
        }
        public static bool AnyKeyDown()
        {
            return Keyboard.current.anyKey.wasPressedThisFrame;
        }

        #endregion

        #region 마우스 입력

        /// <summary>
        /// 마우스 버튼 종류를 정의하는 열거형
        /// </summary>
        public enum MouseButton { Left = 0, Right = 1, Middle = 2}

        // 내부에서만 사용하는 함수 입니다.
        private static ButtonControl GetMouseControl(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return Mouse.current.leftButton;
                case MouseButton.Right:
                    return Mouse.current.rightButton;
                case MouseButton.Middle:
                    return Mouse.current.middleButton;
            }

            return null;
        }

        public static bool GetMouseButtonDown(MouseButton button)
        {
            return GetMouseControl(button).wasPressedThisFrame;
        }

        public static bool GetMouseButtonUp(MouseButton button)
        {
            return GetMouseControl(button).wasReleasedThisFrame;
        }

        public static bool GetMouseButton(MouseButton button)
        {
            return GetMouseControl(button).isPressed;
        }

        public static Vector2 GetMousePosition()
        {
            return Mouse.current.position.ReadValue();
        }

        /// <summary>
        /// 마우스가 한프레임에 이동한 위치 변화량을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetMouseDelta()
        {
            return Mouse.current.delta.ReadValue();
        }

        public static float GetMouseScrollDeltaRaw()
        {
            return Mouse.current.scroll.ReadValue().y;
        }

        // 마우스 휠 스크롤을 -1, 0, 1 단위로 반환합니다.
        // 그... 스크롤 오돌도돌한 한 단위 그거
        public static float GetMouseScrollDelta()
        {
            float raw = GetMouseScrollDeltaRaw();
            return Mathf.Approximately(raw, 0f) ? 0f : Mathf.Sign(raw);
            // Mathf.Approximately
            // 0이랑 비교를 하면 근사치로 일정한 범위내에 raw가 있는지
            // 비교를 합니다.
            // 0.99999999 = 1 => 이런 비교를 하는거임
        }

        #endregion
    }

}