using Jay;
using System.Collections.Generic;
using Study.Utilities;
using study_utilities;
using UnityEngine;
using System;

namespace study_actionplatformer
{

    public class CombatSystem : SingletonBase<CombatSystem>
    {
        public FloatingText fText;
        private ComponentPool<FloatingText> pool;
        private List<ICombatObserver> observerList = new List<ICombatObserver>();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            pool = new ComponentPool<FloatingText>(fText, transform);
        }

        // 구독을 하는 함수
        public void Subscribe(ICombatObserver observer)
        {
            // 두 번 등록하는 거 방지하기 위한 예외처리(두번 등록 실수 많이함)
            if (observerList.Contains(observer)) return;
            observerList.Add(observer);
        }

        // 구독 해제를 하는 함수
        public void UnSubscribe(ICombatObserver observer)
        {
            if (observerList.Contains(observer) == false) return;
            observerList.Remove(observer);
        }

        // @: event라고 하는 키워드 회피용 접두사임
        public void To(CombatEntity sender, CombatEntity receiver, CombatEvent @event)
        {
            // 누가 누구에게 보내는지는 명확하게 검증을 해줘야 하기 때문에 이 부분은 null 처리
            // 그리고 바어적 검증을 수행함.
            // 호출이 많을수도 있는 자주 사용되는 함수이기 때문에 방어적으로 코딩을 해 줌
            if(sender == null || receiver == null)
            {
                Debug.LogWarning($"CombatSystem :::" + $"sender : {sender == null}, receiver : {receiver != null}");
                return;
            }

            Debug.Log($"{sender.name}이 {receiver.name}에게 {@event.EventType.ToString()}" + $", {@event.Amount}을 전달!");

            FloatingText floatingText = pool.Get();
            floatingText.Show($"{@event.Amount}", Color.orange, receiver.HeadUpPivot.position);

            switch (@event.EventType)
            {
                case CombatEventType.DamageEvent:
                    for(int i = 0; i < observerList.Count; ++i)
                    {
                        observerList[i].OnDamageTaken(sender, receiver, @event);
                    }
                    break;
                case CombatEventType.HealEvent:
                    for (int i = 0; i < observerList.Count; ++i)
                        observerList[i].OnHealTaken(sender, receiver, @event);
                    break;
            }

            


        }

        internal void UnSubscribe(DamagePopUpFeedback damagePopUpFeedback)
        {
            throw new NotImplementedException();
        }

        internal void Subscribe(DamagePopUpFeedback damagePopUpFeedback)
        {
            throw new NotImplementedException();
        }
    }
}
