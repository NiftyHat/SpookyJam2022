using NiftyFramework.UI;
using Spawn;
using UnityEngine;

namespace Entity
{
    public class FacingDirectionView : MonoBehaviour, IView<Vector3>, IView<CharacterSpawnPosition.FacingDirection>
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

        public void Set(CharacterSpawnPosition.FacingDirection facingDirection)
        {
            Vector3 vector3 = CharacterSpawnPosition.GetFacingVector(facingDirection);
            Set(vector3);
        }

        public void Clear()
        {
            transform.localScale = FaceLeft;
        }
    }
}