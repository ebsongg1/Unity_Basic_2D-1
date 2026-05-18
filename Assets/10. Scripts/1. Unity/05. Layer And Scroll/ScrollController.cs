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

        public List<GameObject> enableLayerList = new List<GameObject>();

        private void Update()
        {
            // (speed * Time.deltaTime) = 초당 speed의 속도로 뭔가를 하겠다는 표현
            Vector3 moveVector = Vector3.left * (speed * Time.deltaTime);

            // 1. 활성화된 모든 레이어를 moveVector만큼 옮겨준다
            for(int i = 0; i < enableLayerList.Count; ++i)
            {
                enableLayerList[i].transform.Translate(moveVector);
            }

        }
    }
}
