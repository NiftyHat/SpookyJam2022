using System.Collections.Generic;
using Entity;
using NiftyFramework;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterEntitySet", menuName = "Game/NPCs/CharacterEntitySet", order = 2)]
public class CharacterEntitySet : RuntimeSet<CharacterEntity>
{
    public delegate void OnAssigned(IList<CharacterEntity> items);

    public event OnAssigned OnAssign;
    
    public void Assign(IList<CharacterEntity> items)
    {
        if (_itemList == null)
        {
            _itemList = new List<CharacterEntity>();
        }
        _itemList.Clear();
        _itemList.InsertRange(0, items);
        OnAssign?.Invoke(_itemList);
    }
}