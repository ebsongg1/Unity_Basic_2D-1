using Study.Utilities;
using UnityEngine;

public class syudy_transform2d : MonoBehaviour
{

    [field: SerializeField] private Transform Circle { get; set; }

  

    private void Update()
    {
        // 삼각형을 회전시키는 방법

        // 바라보게 할 위치
        Vector3 goalPosition = Circle.position;
        // 내 위치
        Vector3 myPosition = transform.position;

        // 방향 벡터 구하기 (목표 위치 - 내 위치)
        Vector3 dir = (goalPosition - myPosition).normalized;
        transform.up = dir;
    }
    
}
