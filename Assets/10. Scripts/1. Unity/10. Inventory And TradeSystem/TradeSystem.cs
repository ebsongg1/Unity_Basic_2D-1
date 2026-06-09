using UnityEngine;
using Jay;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace Study_Inventory
{
    public class TradeSystem : SingletonBase<TradeSystem>
    {
        public enum TradeType
        {
            Buy,
            Sell
        }

        public struct TradeInfo
        {
            public TradeType            TradeType;
            public InventorySlot        Start;
            public InventorySlot        End;
            public InventoryItem        Item;
        }

        // 원래는 묶어서(클래스화 해서) 쓰는것이 좋습니다
        // ex) TradePopupView
        [Header("PopUp Ref")]
        public GameObject Root;
        public TMP_Text ContentText;
        public Button YesButton;
        public Button NoButton;

        private bool isClick = false;
        private bool isPositiveClick = false;

        public void RequestTradeEvent(TradeInfo info)
        {

        }

        private IEnumerator TriggerTradeEvent(TradeInfo info)
        {
            yield return null;
        }

        private 
    }
}


