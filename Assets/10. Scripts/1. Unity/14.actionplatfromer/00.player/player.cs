using UnityEngine;

namespace study_actionplatformer
{
    // 스탯, 전투 관련 기능을 넣어놓을 것임. 그리고 어떤 개체에서든 player를 찾을 수 있는 기능을 만들것임.
    // CombatEntity를 상속받았기 때문에 '전투에 참여하는 개체'의 공통부(Pivot, TakerDamage 등 순수가상함수 계약)은 물려받고, 플레이어의 고유의 것만 여기 남음

    //public class

    [System.Serializable ]
    public struct AttackInfo
    {
        //public AttackKey Key;
        public int MinDamage;
        public int MaxDamage;
        public AnimationCurve damageCurve;

        // 구조체도 메서드를 가질 수 있음
        // 이 데이터를 어떻게 해석(데미지 계산 공식)하는가? 에 대한 내용은 
        // 데이터 곁에 두는 것이 응집도가 좋음
        // 기능이 많아지면 분리(확장함수)하는 게 좋지만, 몇개 없으면 struct 내부에다가 구현해놓고 사용해도 괜찮음

        public int RollDamage()
        {
            return 0;
        }
    }
    
    public class player : CombatEntity
    {
        public static player LocalPlayer { get; set; }
        //public override BaseStat BaseStat => Stat;
        private PlayerStat Stat {  get; set; }

        //private ov
        
        public override void TakeDamage(int damage)
        {
            
        }

        public override void TakeHeal(int heal)
        {
            
        }

       
    }
}
