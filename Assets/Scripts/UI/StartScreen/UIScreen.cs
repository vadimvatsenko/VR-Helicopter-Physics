using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : MonoBehaviour
    {
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int Hide = Animator.StringToHash("hide");
        
        private Animator _animator;
        
        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
            HandleAnimation(Show);
        }

        public virtual void ShowScreen()
        {
            HandleAnimation(Show);
        }

        public virtual void CloseScreen()
        {
            HandleAnimation(Hide);
        }
        
        private void HandleAnimation(int id) => _animator.SetTrigger(id);
    }
}
