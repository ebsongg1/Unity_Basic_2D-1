using UnityEngine;

namespace study_actionplatformer
{

    public class RangeController : EnemyController
    {
        [field:SerializeField] public Bullet BulletPrefab {  get; set; }
        [field:SerializeField] public Transform FirePoint {  get;  private set; }

        protected override void ProcessAttack()
        {
            // base.ProcessAttack();
            Bullet bullet = Instantiate(BulletPrefab, FirePoint.position, Quaternion.identity);

            float dir = Target.position.x
        }
    }
}
