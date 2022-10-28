using Entity;
using NiftyFramework.DataView;
using UnityEngine;

namespace UI.Targeting
{
    public class UISelectedTargetView : MonoBehaviour, IDataView<CharacterEntity>
    {
        [SerializeField] private UITargetPortraitPanel _targetPortrait;
        [SerializeField] private UIInteractionListPanel _interactionList;

        public void Clear()
        {
            _interactionList.Clear();
            _targetPortrait.Clear();
        }

        public void Set(CharacterEntity data)
        {
            _targetPortrait.Set(data);
        }
    }
}
