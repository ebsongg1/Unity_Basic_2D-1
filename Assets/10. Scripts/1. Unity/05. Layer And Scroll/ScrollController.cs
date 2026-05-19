using System.Collections.Generic;
using UnityEngine;

namespace Study.LayerAndScroll
{
    // ScrollController라는 녀석을 통해서
    // Layer의 속도를 조절해 볼겁니다
    public class ScrollController : MonoBehaviour
    {
        [Header("Scroll Settings")]
        public float speed = 1.0f;

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
            // (speed * Time.deltaTime) = 초당 speed의 속도로 뭔가를 하겠다는 표현
            Vector3 moveVector = Vector3.left * (speed * Time.deltaTime);

            // 1. 활성화된 모든 레이어를 moveVector만큼 옮겨준다
            for(int i = 0; i < enableLayerList.Count; ++i)
            {
                enableLayerList[i].transform.Translate(moveVector);
            }

            // 2. 가장 첫번째 Layer(enableLayerList[0])가
            // EndPivot의 경계를 넘어간다면(x값보다 작아진다면)
            // 삭제한다.

            GameObject headLayer = enableLayerList[0]; 
            // 가장 앞에있는 Layer오브젝트를 가져옵니다

            if(headLayer.transform.position.x <= endPivot.position.x)
            {
                enableLayerList.RemoveAt(0);
                Destroy(headLayer);
            }

            // 3. Layer를 생성해줍니다. 현재 필요한 LayerObject 2 ~ 3개입니다
            // GameObject.Instantiate(GameObject object) || .Instantiate(GameObject object)
            // 런타임 중에 매개변수로 들어온 object의 사본을 생성합니다.

            // .Instantiate() : 실행중(런타임)에 게임오브젝트를 생성하는 함수 입니다.
            //                  생성한 객체는 생성할 객체의 타입으로 반환받을 수 있습니다 
            
            while(enableLayerList.Count <= 2)
            {
                GameObject instance = Instantiate(layerPrefabs[0], // layerPrefabs[0]개체의 사본을 전달합니다
                    spawnPivot.transform.position, spawnPivot.rotation);
                    // spawnPivot의 위치, spawnPivot의 회전값 이라는 말.    
                enableLayerList.Add(instance);
            }
            

            

        }
    }
}
