using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

namespace Study_Inventory
{
    //  InventoryInput은 InventoryCavas(최상위 캔버스)
    // 에서 동작하는 기능입니다. 하위에 있는 모든 Slot, Inventory를
    // 개체에 대한 검사를 수행할 수 있습니다.

    // Held
    // : 특정 개체를 손에 쥐고 끌고가는 듯한 표현
    // Held 기능의 구현
    // 1. 드래그가 시작되면 시작된 위치의 Slot의 아이템 참조를 Cursor에게 Set한다
    // 2. 드래그가 진행되는 동안 Cursor 객체의 위치좌표를 Update한다
    // 3. 드래그가 종료되면 상황에 따라 알맞은 처리를 한다
    //  3.1 종료된 위치의 Slot이 Empty일 경우
    //  3.2 종료된 위치의 Slot에 아이템이 있을 경우
    //  3.3 종료된 위치에 Slot 자체가 없을 경우

    public class InventoryInput : MonoBehaviour
        , IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [field: SerializeField] 
        public InventorySlot CursorSlot { get; private set; }

        private GraphicRaycaster raycaster;
        
        private InventorySlot startSlot; // 드래그를 시작한 슬롯
        private InventorySlot endSlot;   // 드래그를 끝낸 슬롯


        private void Start()
        {
            raycaster = GetComponent<GraphicRaycaster>();
        }

        // 포인터(마우스, 터치) 입력 이벤트를 가지고 해당 위치에
        // Slot이 존재하는지를 검색 합니다.
        private InventorySlot GetSlotOrNull(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();

            // canvas에 붙어있는 raycaster 컴포넌트를 이용해서 
            // 검사를 진행하고 결과를 results 리스트에 담는다
            raycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                if(result.gameObject.TryGetComponent(out InventorySlot slot))
                {
                    // CursorSlot과 같으면 계속 검색합니다.
                    if (slot == CursorSlot) continue;
                    else return slot;
                }
            }

            return null;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            // 1. 드래그가 시작되면 시작된 위치의
            // Slot의 아이템 참조를 Cursor에게 Set한다

            // 시작슬롯을 검색한다
            startSlot = GetSlotOrNull(eventData);
            if (startSlot == null) return; // 검색된 슬롯이 없거나
            if (startSlot.IsEmpty) return; // 슬롯이 비어있다면 종료한다

            CursorSlot.SetItem(startSlot.Item);
        }

        public void OnDrag(PointerEventData eventData)
        {
            //2.드래그가 진행되는 동안 Cursor 객체의 위치좌표를 Update한다
            if (startSlot == null) return; // 드래그 시작되지 않았으면 종료한다
            CursorSlot.SetPosition(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (startSlot == null) return; // 드래그 시작되지 않았으면 종료한다

        }


    }

}

