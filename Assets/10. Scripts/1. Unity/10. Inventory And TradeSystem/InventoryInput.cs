using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Study_Inventory
{
    //  InventoryInput은 InventoryCavas(최상위 캔버스)
    // 에서 동작하는 기능입니다. 하위에 있는 모든 Slot, Inventory를
    // 개체에 대한 검사를 수행할 수 있습니다.

    public class InventoryInput : MonoBehaviour
        , IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }

}

