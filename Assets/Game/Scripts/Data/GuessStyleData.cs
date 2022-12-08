using System;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GuessStyleData", menuName = "Game/Style/GuessStyleData", order = 1)]
    public class GuessStyleData : ScriptableObject
    {
        [Serializable]
        public class Style
        {
            public Color Color;
            public string FriendlyName;
        }

        [SerializeField] public Style Yes;
        [SerializeField] public Style No;

        public bool TryGet(Guess enumValue, out Style style)
        {
            switch (enumValue)
            {
                default:
                    style = null;
                    return false;
                case Guess.Yes:
                    style = Yes;
                    return true;
                case Guess.No:
                    style= No;
                    return true;
            }
        }
    }
}