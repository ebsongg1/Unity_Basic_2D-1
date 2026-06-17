using study_actionplatformer;
using UnityEngine;

public class attackInfo : MonoBehaviour
{
    public struct AttackInfo
    {
        public AttackKey Key;
        public int MinDamage;
        public int MaxDamage;
        public AnimationCurve damageCurve;

        public int RollDamage()
        {
            // Curve에 t역할을 검색할 랜덤한 값을 추출
            // ex) 0~99까지의 랜덤한 숫자를 뽑음
            float randomRoll = Random.Range(0f, 1f);

            // 커브를 통해 가중치(t) 평가 = .Evaluate()를 이용해 Y축 값 추출
            float evaluation = damageCurve.Evaluate(randomRoll);

            float finalDamage = Mathf.Lerp(MinDamage, MaxDamage, evaluation);

            // Mathf.RoundToInt() : float를 반올림 하는 함수

            // Round : 반올림
            // Ceil : 올림
            // Floor : 버림(내림)
            // 접미사 ToInt() 붙는 함수의 경우 float로 반환하는 게 아니라 int로 반환함

            return Mathf.RoundToInt(finalDamage);
        }
    }
}
