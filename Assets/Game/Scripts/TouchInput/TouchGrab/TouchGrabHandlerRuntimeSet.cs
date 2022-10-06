using UnityEngine;

namespace NiftyFramework.TouchGrab
{
    [CreateAssetMenu(fileName = "TouchGrabHandlerRuntimeSet", menuName = "RuntimeSet/TouchGrabHandler", order = 1)]
    public class TouchGrabHandlerRuntimeSet : RuntimeSet<TouchGrabHandler>
    {


        public void OnEnable () 
        {
            _itemList.Clear();
        }

        public void HandleMove (Vector3 newPosition) 
        {
            int len = _itemList.Count - 1;
            if (len >= 0)
            {
                for (int i = len; i >= 0; i--)
                {
                    TouchGrabHandler handler = _itemList[i];
                    handler.HandleGrabMoved(newPosition);
                }
            }
        }

        public void HandleEnd (Ray touchRay) 
        {
            
        }
	}

}
  