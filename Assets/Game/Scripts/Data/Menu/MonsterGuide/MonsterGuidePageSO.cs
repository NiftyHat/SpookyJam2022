using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MonsterGuidePageSO", order = 1)]
public class MonsterGuidePageSO : ScriptableObject
{
    public string monsterName;
    public Sprite image;
    [TextArea]
    public string description;
    [TextArea]
    public string hint;
}
