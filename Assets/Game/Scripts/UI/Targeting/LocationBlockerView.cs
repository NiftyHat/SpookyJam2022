using UnityEngine;

namespace UI.Targeting
{
    public class LocationBlockerView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        private Color? _defaultColor;
        private Color _blockingColor;
        
        public void SetBlocking(bool isBlocking)
        {
            if (_renderer != null)
            {
                if (_defaultColor == null)
                {
                    Color renderColor = _renderer.color;
                    _defaultColor = renderColor;
                    _blockingColor = new Color(renderColor.r, renderColor.g, renderColor.b, 0.4f);
                }
                if (isBlocking)
                {
                    _renderer.color = _blockingColor;
                }
                else
                {
                    _renderer.color = _defaultColor.Value;
                }
            }
           
        }
    }
}