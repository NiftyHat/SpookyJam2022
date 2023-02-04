using System;
using UnityEngine;

namespace Data.TextSequence
{
    public class NextPromptWidget : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int IsVisible = Animator.StringToHash("isVisible");

        public event Action OnInput;

        public void SetVisible(bool isVisible)
        {
            _animator.SetBool(IsVisible, isVisible);
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                OnInput?.Invoke();
            }
        }
    }
}