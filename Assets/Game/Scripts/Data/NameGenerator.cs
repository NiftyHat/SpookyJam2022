using System.Collections.Generic;
using System.Text;
using Entity;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NameGenerator", menuName = "Game/Names/NameGenerator", order = 1)]
    public class NameGenerator : ScriptableObject
    {
        public class NamePool
        {
            private Dictionary<CharacterName.ImpliedGender, List<CharacterName>> _map;
            
            public NamePool(Dictionary<CharacterName.ImpliedGender, List<CharacterName>> map)
            {
                _map = map;
            }

            public string PrintDebug()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Name Pool ");
                sb.AppendLine();
                foreach (var kvp in _map)
                {
                    sb.Append(kvp.Key.ToString());
                    sb.Append(":");
                    foreach (var item in kvp.Value)
                    {
                        sb.Append($"'{item.Full}'");
                        sb.Append(" ");
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }

            public bool TryGet(out CharacterName characterName, CharacterName.ImpliedGender impliedGender)
            {
                if (_map.TryGetValue(impliedGender, out var list))
                {
                    if (list.Count > 0)
                    {
                        int lastIndex = list.Count - 1;
                        characterName = list[list.Count - 1];
                        list.RemoveAt(lastIndex);
                        return true;
                    }
                }
                characterName = null;
                return false;
            }
            
            public bool TryGet(out CharacterName characterName, System.Random random)
            {
                CharacterName.ImpliedGender impliedGender = GetRandomGender(random);
                if (_map.TryGetValue(impliedGender, out var list))
                {
                    if (list.Count > 0)
                    {
                        int lastIndex = list.Count - 1;
                        characterName = list[list.Count - 1];
                        list.RemoveAt(lastIndex);
                        return true;
                    }
                }
                characterName = null;
                return false;
            }
        }
        
        [Header("First Name")]
        [SerializeField] protected NameTableData _firstNameMasc;
        [SerializeField] protected NameTableData _firstNameFemme;
        [SerializeField] protected NameTableData _firstNameNeutral;

        [Header("Last Name")]
        [SerializeField] protected NameTableData _lastNameTable;

       //[Header("Middle Name")]
       // public NameTableData _middleNameTable;
        //public float _middleNameChance = 0.3f;

        public NamePool GetPool(System.Random random, int count)
        {
            Dictionary<CharacterName.ImpliedGender, List<CharacterName>> map =
                new Dictionary<CharacterName.ImpliedGender, List<CharacterName>>();
            map[CharacterName.ImpliedGender.Masc] = GetNames(random,CharacterName.ImpliedGender.Masc, count);
            map[CharacterName.ImpliedGender.Femme] = GetNames(random,CharacterName.ImpliedGender.Femme, count);
            map[CharacterName.ImpliedGender.Neutral] = GetNames(random,CharacterName.ImpliedGender.Neutral, count);
            return new NamePool(map);
        }

        public List<CharacterName> GetNames(System.Random random, CharacterName.ImpliedGender gender, int count)
        {
            List<CharacterName> characterNames = new List<CharacterName>();
            NameTableData firstNameTable = null;
            switch (gender)
            {
                case CharacterName.ImpliedGender.Femme:
                    firstNameTable = _firstNameFemme;
                    break;
                case CharacterName.ImpliedGender.Masc:
                    firstNameTable = _firstNameMasc;
                    break;
                case CharacterName.ImpliedGender.Neutral:
                case CharacterName.ImpliedGender.None:
                    firstNameTable = _firstNameNeutral;
                    break;
            }

            List<string> firstNameList = null;
            List<string> lastNameList = null;
            if (firstNameTable != null)
            {
                firstNameList = firstNameTable.GetRandom(random, count);
            }

            if (_lastNameTable != null)
            {
                lastNameList  = _lastNameTable.GetRandom(random, count);
            }

            for (int i = 0; i < count; i++)
            {
                if (firstNameList != null && lastNameList != null)
                {
                    CharacterName characterName = new CharacterName(firstNameList[i], lastNameList[i], gender);
                    characterNames.Add(characterName);
                }
            }
            return characterNames;
        }

        [ContextMenu("Test")]
        public void Test()
        {
            System.Random random = new System.Random();
            var testPool = GetPool(random, 10);
            Debug.Log(testPool.PrintDebug());
        }

        public static CharacterName.ImpliedGender GetRandomGender(System.Random random)
        {
            int genderInt = random.Next(10);
            if (genderInt == 0)
            {
                return  CharacterName.ImpliedGender.Neutral;
            }
            else if (genderInt <= 6)
            {
                return  CharacterName.ImpliedGender.Femme;
            }
            else
            {
                return  CharacterName.ImpliedGender.Masc;
            }
        }
    }
}