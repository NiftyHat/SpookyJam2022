using System;
using System.Linq;
using Entity;
using NiftyFramework.Core.Utils;
using NiftyFramework.DataView;
using UnityEngine;

namespace UI.Targeting
{
    public class UISelectedTargetView : MonoBehaviour, IDataView<CharacterEntity>
    {
        [SerializeField][NonNull] private UITargetPortraitPanel _targetPortrait;
        [SerializeField][NonNull] private UIInteractionListPanel _interactionList;
        [SerializeField][NonNull] private UIAssignedTraitsPanel _assignedTraitsPanel;

        public void Clear()
        {
            _interactionList.Clear();
            _targetPortrait.Clear();
        }

        public void Set(CharacterEntity data)
        {
            if (data == null)
            {
                Clear();
                return;
            }
            _targetPortrait.Set(data);
            if (data.Traits != null)
            {
                _assignedTraitsPanel.Set(data.Traits.ToList());
            }
            else
            {
                _assignedTraitsPanel.Clear();
            }
        }
    }
}
