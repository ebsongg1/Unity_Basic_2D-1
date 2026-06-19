using UnityEngine;

namespace study_actionplatformer
{

    public class Bullet : MonoBehaviour
    {
        [field:SerializeField] public float Speed { get; private set;  } 
        private Vector3 foward = Vector3.right;

        private void Start()
        {

        }

        private void Update()
        {
            transform.Translate(foward * Speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject == player.LocalPlayer.gameObject)
            {
                Destroy(gameObject);
            }
        }

        public void Set(Vector3 direction)
        {
            foward = direction;
        }
    }
}
