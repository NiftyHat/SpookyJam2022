using System;
using NiftyFramework.Core.Utils;
using NiftyFramework.UI;
using UnityEngine;

namespace UI.Targeting
{
    public class LocationIndicatorView : MonoBehaviour, IView<Vector3, Vector3, Func<bool>>
    {
        [SerializeField][NonNull] private DistanceLineView _distanceLineView;

        public void Set(Vector3 start, Vector3 end, Func<bool> validateRange)
        {
            gameObject.SetActive(true);
            transform.position = end + Vector3.up;
            _distanceLineView.transform.localPosition = Vector3.zero;
            if (_distanceLineView != null)
            {
                _distanceLineView.SetFrom(start+ Vector3.up);
            }

            if (validateRange != null)
            {
                if (validateRange())
                {
                    _distanceLineView.SetColor(Color.white);
                }
                else
                {
                    _distanceLineView.SetColor(Color.red);
                }
            }
        }

        public void Clear()
        {
            gameObject.SetActive(false);
        }
    }
}
