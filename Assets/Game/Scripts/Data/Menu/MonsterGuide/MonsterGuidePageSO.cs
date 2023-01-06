using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MonsterGuidePageSO", order = 1)]
public class MonsterGuidePageSO : ScriptableObject
{
    public string monsterName;
    public Sprite image;
    [TextArea(8,12)]
    public string description;
    [TextArea(2,5)]
    public string hint;
}
