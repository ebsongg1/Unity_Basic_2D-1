using Study.Utilities;
using study_actionplatformer;
using study_utilities;
using UnityEngine;

public class DebugFloationgTExt : MonoBehaviour
{
    public FloatingText fText;

    private ComponentPool<FloatingText> pool;

    private void Awake()
    {
        pool = new ComponentPool<FloatingText>(fText, transform);
    }

    public CombatEntity entity;

    void Update()
    {
        if(SimpleInput.GetKeyDown(UnityEngine.InputSystem.Key.Space))
        {
            float randX = Random.Range(-5f, 5f);
            float y = 1.5f;
            int damage = Random.Range(100, 300);
            CombatEvent @event;
            @event.EventType = CombatEventType.HealEvent;
            @event.Amount = 10;
            @event.Position = entity.transform.position;

            FloatingText instance = pool.Get();
            instance.Show($"{damage}", Color.orange, new Vector3(randX, y, 0.0f));
            CombatSystem.Instance.To(entity, entity, @event);
        }
    }
}
