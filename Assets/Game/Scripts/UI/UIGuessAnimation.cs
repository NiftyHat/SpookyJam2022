using NiftyFramework.UI;
using UnityEngine;

public class UIGuessAnimation : MonoBehaviour, IView<Guess>
{
    [SerializeField] private Animator _animator;

    protected Guess _state;
    private static readonly int EnumState = Animator.StringToHash("EnumState");

    public void Set(Guess enumGuess)
    {
        _state = enumGuess;
        if (_animator != null)
        {
            _animator.SetInteger(EnumState, (int)_state);
        }
    }

    public void Clear()
    {
        _animator.SetInteger(EnumState, 0);
    }
}
