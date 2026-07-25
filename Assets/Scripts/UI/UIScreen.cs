using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreen : MonoBehaviour
    {
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int Hide = Animator.StringToHash("hide");
        
        public Selectable startSelectable;
        private Animator _animator;

        private Action _onScreenStart;
        private Action _onScreenClose;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            HandleAnimation(Show);
        }

        public virtual void StartScreen()
        {
            _onScreenStart?.Invoke();
            HandleAnimation(Show);
        }

        public virtual void CloseScreen()
        {
            _onScreenClose?.Invoke();
            HandleAnimation(Hide);
        }
        
        private void HandleAnimation(int id) => _animator.SetTrigger(id);
    }
}
