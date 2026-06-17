using UnityEngine;

namespace study_actionplatformer
{

    public class HurtBox : MonoBehaviour
    {
        [field:SerializeField] public CombatEntity Owner {  get; private set; }

        private void Awake()
        {
            Owner = GetComponent<CombatEntity>();
            if(Owner == null)
            {
                Owner = GetComponentInParent<CombatEntity>();
            }
            if (Owner == null)
            {
                Debug.LogError($"{name} : 부모 계층에서 CombatEntity 를 찾지 못했습니다.");
            }
        }
    }
}
