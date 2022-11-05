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
        [SerializeField][NonNull] protected Transform _ringTransform;

        protected float _defaultRingFillAlpha;
    
        protected void Start()
        {
            _defaultRingFillAlpha = _fillSprite.color.a;
        }

        protected void SetColour(Color color)
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
            _ringTransform.localScale = Vector3.one * distance * 0.1f * 2f;
        }

        public void ShowDistance(Vector3 sourcePosition, float range, Func<bool> validateRange)
        {
            sourcePosition = new Vector3(sourcePosition.x, 0.2f, sourcePosition.z);
            transform.position = sourcePosition;
            SetScale(range);
            if (validateRange != null)
            {
                if (validateRange())
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
