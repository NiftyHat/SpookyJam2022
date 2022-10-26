using NiftyFramework.UI;
using UnityEngine;

namespace Entity
{
    public class FacingDirectionView : MonoBehaviour, IView<Vector3>
    {
        private static readonly Vector3 FaceLeft = new Vector3(1, 1, 1);
        private static readonly Vector3 FaceRight = new Vector3(-1, 1, 1);
        
        public void Set(Vector3 direction)
        {
            if (direction.x > 0)
            {
                transform.localScale = FaceLeft;
            }
            if (direction.x < 0)
            {
                transform.localScale = FaceRight;
            }
        }

        public void Clear()
        {
            transform.localScale = FaceLeft;
        }
    }
}