using Entity;
using NiftyFramework.DataView;
using NiftyFramework.Scripts.UnityUtils;
using UnityEditor;
using UnityEngine;

namespace Spawn
{
    public class CharacterSpawnPosition : MonoBehaviour, IDataView<CharacterEntity>
    {
        public enum FacingDirection
        {
            Left,
            Right
        }
        
        [SerializeField] private CharacterView _viewPrefab;
        [SerializeField] private CharacterSpawnSet _characterSpawnSet;
        [SerializeField] private CharacterView _viewCache;
        [SerializeField] private FacingDirection _facingDirection;

        private void OnValidate()
        {
            _characterSpawnSet = GetComponentInParent<CharacterSpawnSet>();
            if (_characterSpawnSet != null)
            {
                _characterSpawnSet.Register(this);
                if (_viewCache != null)
                {
                    Vector3 facingDirection = GetFacingVector(_facingDirection);
                    _viewCache.SetFacing(facingDirection);
                    _viewCache.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                Debug.LogWarning($"Orphan Spawn Location {name}");
            }
            
        }
        
        public static Vector3 GetFacingVector(FacingDirection facingDirection)
        {
            switch (facingDirection)
            {
                case FacingDirection.Left:
                    return Vector3.left;
                case FacingDirection.Right :
                    return Vector3.right;
            }
            return Vector3.left;
        }

        void OnDrawGizmosSelected()
        {
            Vector3 origin = transform.position;
            Vector3 facingOrigin = origin + Vector3.up * 10f;
            Vector3 facingDirection = GetFacingVector(_facingDirection);
            _viewCache.SetFacing(facingDirection);
            DrawArrow.ForGizmo(facingOrigin, facingDirection * 5f, Color.green);
        }

        private void OnDrawGizmos()
        {
            Vector3 origin = transform.position;
            if (_viewCache != null &&  _viewCache.Entity != null)
            {
                Handles.Label(origin, _viewCache.Entity.Name.Full);
            }
            else
            {
                Handles.Label(origin, "Guest Spawn");
            }
            Gizmos.DrawWireSphere(origin, 2f);
        }

        public bool IsValidSpawn<TEntityType>()
        {
            return true;
        }

        public void Clear()
        {
            if (_viewCache != null)
            {
                _viewCache.gameObject.SetActive(false);
            }
        }

        public void Set(CharacterEntity entity)
        {
            if (_viewCache == null)
            {
                _viewCache = Instantiate(_viewPrefab, this.transform);
            }
            _viewCache.gameObject.SetActive(true);
            _viewCache.Set(entity);
        }
    }
}
