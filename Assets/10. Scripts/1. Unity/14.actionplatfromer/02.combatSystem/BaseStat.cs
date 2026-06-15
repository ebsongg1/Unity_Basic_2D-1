using UnityEngine;

namespace study_actionplatformer
{
    // 모든 캐릭터(플레이어,적,보스) 스탯의 공통 부모

    // 단순히 데이터만 담지 않고, 'HP'가 변하는 규칙(클램프, 사망 등)' 까지
    // 이 클래스가 책임짐 => 데이터가 변경의 규칙까지 책임진다는 말

    // 에디터에서 노출하려고 일부러 선언.
    // 원래는 숨기는 게 맞음
    [System.Serializable ]
    public class BaseStat
    {
        [field: SerializeField] public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public bool isDead => (Hp <= 0);

        /// <summary>
        /// 스탯 객체에게 데미지 적용하고, 죽었는지를 반환하는 함수
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public bool ApplyDamage(int damage)
        {
            Hp = Mathf.Clamp(Hp - damage, 0, MaxHp);
            return isDead;
        }

        public void ApplyHeal(int heal)
        {
            Hp = Mathf.Clamp(Hp + heal, 0, MaxHp);
        }

        /// <summary>
        /// 체력을 최대치로 되돌림.
        /// </summary>
        public void ResetToFull()
        {
            Hp = MaxHp;
        }
    }
}
