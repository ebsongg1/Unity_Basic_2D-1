using UnityEngine;
using UnityEngine.InputSystem;

public class UFOController : MonoBehaviour
{
    // 위,아래 움직임 기능 넣어보기
    // 기본적으로 하강을 하지만 스페이스를 눌렀을때는 상승

    public float Speed = 1.0f;
    public int CoinCount = 0;

    // Update is called once per frame
    void Update()
    {
        //기본적으로 하강
        Vector3 moveVector = new Vector3(0, -1, 0);

        // Space를 누르면 상승한다
        if (Keyboard.current.spaceKey.isPressed)
        {
            moveVector = moveVector * (-1f); // -1 * -1 = +1;
        }

        //속도 * 프레임당 이동량을 구해준다
        moveVector = moveVector * Speed * Time.deltaTime;

        transform.position += moveVector;
    }

    // 이거 나중
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        CoinCount += 1;
    }
}