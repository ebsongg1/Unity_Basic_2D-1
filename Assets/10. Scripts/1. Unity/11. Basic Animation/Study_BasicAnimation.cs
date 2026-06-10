using Study.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Study_BasicAnimation
{
    public class Study_BasicAnimation : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            bool isWalk = SimpleInput.GetKey(Key.RightArrow) || SimpleInput.GetKey(Key.LeftArrow);
            bool isRun = SimpleInput.GetKey(Key.LeftShift);

            animator.SetBool("IsWalk", isWalk);
            animator.SetBool("IsRun", isRun);
        }
    }


}


