using System.Collections.Generic;
using UnityEngine;

namespace Study.LayerAndScroll
{
    // ScrollController라는 녀석을 통해서
    // Layer의 속도를 조절해 볼겁니다
    public class ScrollController : MonoBehaviour
    {
        public enum ScrollDirection { Left, Right, Up, Down }

        [Header("Scroll Settings")]
        public float speed = 1.0f;
        public ScrollDirection direction = ScrollDirection.Left;

        [Header("Resources")]
        public GameObject[] layerPrefabs;

        [Header("Ref Objects")]
        public GameObject startLayer;
        public Transform endPivot;
        public Transform spawnPivot;

        private List<GameObject> enableLayerList = new List<GameObject>();

        private void Start()
        {
            enableLayerList.Add(startLayer);
        }

        private void Update()
        {
            MoveLayerList();
            CheckDestroyAbleLayer();
            CheckInstantiateLayer();
        }

        private Vector3 GetMoveDirection(ScrollDirection dir)
        {
            switch(dir)
            {
                case ScrollDirection.Left:
                    return Vector3.left;
                case ScrollDirection.Right:
                    return Vector3.right;
                case ScrollDirection.Up:
                    return Vector3.up;
                case ScrollDirection.Down:
                    return Vector3.down;
                default:
                    return Vector3.left;
            }
        }

        private void MoveLayerList()
        {
            // (speed * Time.deltaTime) = 초당 speed의 속도로 뭔가를 하겠다는 표현
            Vector3 dir = GetMoveDirection(direction);
            Vector3 moveVector = dir * (speed * Time.deltaTime);

            // 1. 활성화된 모든 레이어를 moveVector만큼 옮겨준다
            for (int i = 0; i < enableLayerList.Count; ++i)
            {
                enableLayerList[i].transform.Translate(moveVector);
            }
        }

        private void CheckDestroyAbleLayer()
        {
            // 2. 가장 첫번째 Layer(enableLayerList[0])가
            // EndPivot의 경계를 넘어간다면(x값보다 작아진다면)
            // 삭제한다.

            GameObject headLayer = enableLayerList[0];
            // 가장 앞에있는 Layer오브젝트를 가져옵니다

            bool check = false;
            
            switch(direction)
            {
                case ScrollDirection.Left:
                    // headLayer의 x가 endPivot보다 작다면
                    check = headLayer.transform.position.x <= endPivot.position.x;
                    break;
                case ScrollDirection.Right:
                    // headLayer의 x가 endPivot보다 크다면
                    check = headLayer.transform.position.x >= endPivot.position.x;
                    break;
                case ScrollDirection.Up:
                    // headLayer의 y가 endPivot보다 크다면
                    check = headLayer.transform.position.y >= endPivot.position.y;
                    break;
                case ScrollDirection.Down:
                    // headLayer의 y가 endPivot보다 작다면
                    check = headLayer.transform.position.y <= endPivot.position.y;
                    break;
            }

            // 왼쪽케이스
            if (check)
            {
                enableLayerList.RemoveAt(0);
                Destroy(headLayer);
            }
        }

        private void CheckInstantiateLayer()
        {
            // 3. Layer를 생성해줍니다. 현재 필요한 LayerObject 2 ~ 3개입니다

            //  GameObject.Instantiate(GameObject object) || .Instantiate(GameObject object)
            // 실행중(런타임)에 매개변수로 들어온 object의 사본을 생성합니다.
            // 생성한 객체는 생성할 객체의 타입으로 반환받을 수 있습니다                  
            while (enableLayerList.Count <= 2)
            {
                GameObject instance = Instantiate(layerPrefabs[0], // layerPrefabs[0]개체의 사본을 전달합니다
                    spawnPivot.transform.position, spawnPivot.rotation);
                // spawnPivot의 위치, spawnPivot의 회전값 이라는 말.    
                enableLayerList.Add(instance);
            }
        }
    }
}
