using UnityEngine;

namespace study_actionplatformer
{
    // # 이름을 CombatEventType으로 지은 이유?
    // 그냥 EventType이라고 하면 유니티 내장 타입인 UnityEngine.EventType(IMGUI용)과 이름이 겹쳐서(다른 걸로 해도 무방)

    public enum CombatEventType
    {
        DamageEvent,
        HealEvent
    }

    public struct CombatEvent
    {
        public CombatEventType EventType;
        public int Amount;
        public Vector3 Position;
    }

    
}
