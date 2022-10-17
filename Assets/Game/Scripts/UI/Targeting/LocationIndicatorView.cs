using System;
using NiftyFramework.Core.Utils;
using UnityEngine;

namespace UI.Targeting
{
    public class LocationIndicatorView : MonoBehaviour
    {
        [SerializeField][NonNull] private DistanceLineView _distanceLineView;

        public void ShowDistance(Vector3 start, Vector3 end, Func<float, bool> validateRange)
        {
            transform.position = end;
            _distanceLineView.transform.localPosition = Vector3.zero;
            if (_distanceLineView != null)
            {
                _distanceLineView.SetFrom(start);
            }

            if (validateRange != null)
            {
                float distance = Vector3.Distance(start,end);
                if (validateRange(distance))
                {
                    _distanceLineView.SetColor(Color.white);
                }
                else
                {
                    _distanceLineView.SetColor(Color.red);
                }
            }
        }
    }
}
