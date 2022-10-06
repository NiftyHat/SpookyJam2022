using UnityEngine;

namespace NiftyFramework
{

    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        //---------------------------------------------------------------------
        // Fields
        //---------------------------------------------------------------------

        private static T _instance = default( T );

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------

        public static T instance
        {
            get
            {
                return _instance;
            }
        }

        //---------------------------------------------------------------------
        // MonoBehavior Methods
        //---------------------------------------------------------------------

        virtual public void Awake()
        {
            if ( _instance == null )
            {
                _instance = this as T;
            }
        }
    }
}