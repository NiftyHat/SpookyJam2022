using System;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;

namespace UI.Targeting
{
    public class RangeIndicatorView : MonoBehaviour, IView<Vector3, RangeIndicatorView.Data>
    {
        public struct Data
        {
            public float Distance;
            public float Range;
        }

        [SerializeField] protected Color _defaultColour;
        [SerializeField] protected Color _inRangeColour;
        [SerializeField] protected Color _outOfRangeColor;
    
        [SerializeField][NonNull] protected SpriteRenderer _outlineSprite;
        [SerializeField][NonNull] protected SpriteRenderer _fillSprite;
        [SerializeField][NonNull] protected Transform _ringTranform;

        protected float _defaultRingFillAlpha;
    
        void Start()
        {
            _defaultRingFillAlpha = _fillSprite.color.a;
        }

        void SetColour(Color color)
        {
            _outlineSprite.color = color;
            _fillSprite.color = new Color(color.r, color.g, color.b, _defaultRingFillAlpha);
        }

        public void Set(Vector3 position, Data viewData)
        {
            
            if (viewData.Distance == 0)
            {
                SetColour(_defaultColour);
            }
            else
            {
                if (viewData.Distance < viewData.Range)
                {
                    SetColour(_inRangeColour);
                }
                else
                {
                    SetColour(_outOfRangeColor);
                }
            }
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }

        public void SetScale(float distance)
        {
           
            _ringTranform.localScale = Vector3.one * distance * 0.1f;
        }

        public void ShowDistance(Vector3 sourcePosition, Vector3 targetPosition, float range, Func<float, bool> validateRange)
        {
            transform.position = sourcePosition;
            SetScale(range);
            if (validateRange != null)
            {
                float distance = Vector3.Distance(sourcePosition,targetPosition);
                if (validateRange(distance))
                {
                    SetColour(_inRangeColour);
                }
                else
                {
                    SetColour(_outOfRangeColor);
                }
            }
            else
            {
                SetColour(_defaultColour);
            }
        }
    }
}
