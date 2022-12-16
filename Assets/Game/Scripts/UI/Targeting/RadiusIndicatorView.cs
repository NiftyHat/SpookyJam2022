using System;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;

namespace UI.Targeting
{
    public class RadiusIndicatorView : MonoBehaviour, IView<Vector3, RadiusIndicatorView.Data>
    {
        public struct Data
        {
            public float Radius;
            public int TargetCount;
        }

        [SerializeField] protected Color _defaultColour;
        [SerializeField] protected Color _hitTargetColour;
        [SerializeField] protected Color _noTargetColour;
    
        [SerializeField][NonNull] protected SpriteRenderer _outlineSprite;
        [SerializeField][NonNull] protected SpriteRenderer _fillSprite;
        [SerializeField][NonNull] protected Transform _ringTransform;

        protected float _defaultRingFillAlpha = -1f;
        
        protected void SetColour(Color color)
        {
            if (_defaultRingFillAlpha == -1f)
            {
                _defaultRingFillAlpha = _fillSprite.color.a;
            }
            _outlineSprite.color = color;
            _fillSprite.color = new Color(color.r, color.g, color.b, _defaultRingFillAlpha);
        }

        public void Set(Vector3 position, Data viewData)
        {
            if (viewData.Radius == 0)
            {
                SetColour(_defaultColour);
            }
            else
            {
                if (viewData.TargetCount > 0)
                {
                    SetColour(_hitTargetColour);
                }
                else
                {
                    SetColour(_noTargetColour);
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

        public void ShowRadius(Vector3 sourcePosition, float radius, Func<int> validateRadius)
        {
            sourcePosition = new Vector3(sourcePosition.x, 0.2f, sourcePosition.z);
            transform.position = sourcePosition;
            SetScale(radius);
            if (validateRadius != null)
            {
                if (validateRadius() > 0)
                {
                    SetColour(_hitTargetColour);
                }
                else
                {
                    SetColour(_noTargetColour);
                }
            }
            else
            {
                SetColour(_defaultColour);
            }
        }
    }
}
