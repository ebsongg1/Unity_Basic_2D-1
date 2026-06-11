using Study.Utilities;
using UnityEngine;

public class study_Worldmouseoint : MonoBehaviour
{
    private float ScreenWidth { get; set; }
    private float ScreenHeight { get; set; }

    private void Start()
    {
        // Screen 클래스에 접근해서 현재 화면의 너비와 높이 값을 가져옴
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
    }

    private void Update()
    {
        // 화면의 중점을 왼쪽 하단에서 화면 중앙 좌표로 전환
        Vector2 mousePosition = SimpleInput.GetMousePosition();

        // 화면 좌표계에서 World(Scene)의 좌표계로 전환
        // 전환할 때는 Camera의 기능을 이용

        Vector3 worldMousePoint = Camera.main.ScreenToWorldPoint(mousePosition);
        worldMousePoint.z = transform.position.

        // 전환된 위치를 대입
        transform.position = mousePosition;
    }
}
