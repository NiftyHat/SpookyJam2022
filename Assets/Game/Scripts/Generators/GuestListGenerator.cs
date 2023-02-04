using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using Data.Character;
using Data.Monsters;
using Data.Trait;
using Entity;
using NiftyFramework.Scripts;
using UnityEngine;

namespace Generators
{
    [CreateAssetMenu(fileName = "GuestListGenerator", menuName = "Game/GuestListGenerator", order = 6)]
    public class GuestListGenerator : ScriptableObject
    {
        /// <summary>
        /// Limited list off stuff the guest generation can pick from. The masks/names/traits they can pick from
        /// during the generation step.
        /// Whenever the generation uses one of these items it should be removed from the pool so no other guest
        /// can make use of it.
        /// </summary>
        public class GuestItemPool
        {
            public GuestItemPool(MaskGenerator.MaskPool maskPool, NameGenerator.NamePool namePool, TraitPoolGenerator.TraitPool traitPool, CharacterViewDataSet viewDataSet, ScheduleGenerator.SchedulePool schedulePool)
            {
                _maskPool = maskPool;
                _namePool = namePool;
                _traitPool = traitPool;
                _viewDataSet = viewDataSet;
                _schedulePool = schedulePool;
            }
            
            protected readonly MaskGenerator.MaskPool _maskPool;
            protected readonly NameGenerator.NamePool _namePool;
            protected readonly TraitPoolGenerator.TraitPool _traitPool;
            protected readonly CharacterViewDataSet _viewDataSet;
            protected readonly ScheduleGenerator.SchedulePool _schedulePool;

            public MaskGenerator.MaskPool Masks => _maskPool;
            public NameGenerator.NamePool Names => _namePool;
            public TraitPoolGenerator.TraitPool Traits => _traitPool;
            public CharacterViewDataSet ViewData => _viewDataSet;
            public ScheduleGenerator.SchedulePool Schedules => _schedulePool;
        }

        [SerializeField] protected NameGenerator _nameGenerator;
        [SerializeField] protected MaskGenerator _maskGenerator;
        [SerializeField] protected TraitPoolGenerator _traitPoolGenerator;

        [SerializeField] protected MonsterGenerator _monsterGenerator;
        [SerializeField] protected KillerGenerator _killerGenerator;
        [SerializeField] protected PersonGenerator _personGenerator;
        [SerializeField] protected CharacterEntitySet _characterEntitySet;

        [SerializeField] protected IntRange _peopleWithMonsterTraits;
        [SerializeField] protected CharacterViewDataSet _viewDataSet;
        [SerializeField] protected ScheduleGenerator _scheduleGenerator;

        public CharacterEntitySet EntitySet => _characterEntitySet;
        public MonsterEntityTypeDataSet MonsterTypeSet => _monsterGenerator?.MonsterTypeDate;
        
        public List<CharacterEntity> Generate(int personCount, int monsterCount = 1, int killerCount = 1, Int32 seed = 0)
        {
            System.Random random = new System.Random(seed);
            List<CharacterEntity> generatedCharacters = new List<CharacterEntity>();
            
            //calculate how many entities we need to make for the Guest list
            int totalCount = personCount + monsterCount + killerCount;
            
            //Fill out a pool of masks.
            MaskGenerator.MaskPool maskPool = _maskGenerator.GetPool();
            int maskCount = maskPool.Count;
            //if we have more people to make remove person entities until theres enough masks to go around.
            if (totalCount > maskCount)
            {
                Debug.LogWarning($"{totalCount} guest list was bigger than mask pool of {maskCount}. People will be removed to make space for Monsters/Killers");
                totalCount = maskCount;
                personCount = maskCount - monsterCount - killerCount;
                if (personCount <= 0)
                {
                    throw new ArgumentException(
                        $"{monsterCount} Monsters and {killerCount} Killers is more than the total number of mask {maskCount}");
                }
            }
            //name pool
            NameGenerator.NamePool namePool = _nameGenerator.GetPool(random, totalCount * 2);
            //trait pool
            TraitPoolGenerator.TraitPool traitPool = _traitPoolGenerator.GetPool(random, totalCount);

            ScheduleGenerator.SchedulePool schedulePool = _scheduleGenerator.GetPool(random, totalCount);
            //guest item pool shares everything that needs to be distributed across the entire guest list;
            GuestItemPool itemPool = new GuestItemPool(maskPool, namePool, traitPool, _viewDataSet, schedulePool);
            //Monster Generation
            HashSet<MonsterEntityTypeData> monsterTypeSet = new HashSet<MonsterEntityTypeData>();
            List<MonsterEntity> monsterEntities = new List<MonsterEntity>();
            MaskEntity monsterTargetMask = null;
            GuestSchedule monsterTargetSchedule = null;
            for (int i = 0; i < monsterCount; i++)
            {
                MonsterEntity monster = _monsterGenerator.Generate(random, itemPool, out var monsterType);
                monsterEntities.Add(monster);
                generatedCharacters.Add(monster);
                if (!monsterTypeSet.Contains(monsterType))
                {
                    monsterTypeSet.Add(monsterType);
                }

                if (monster.TypeData.HasCopyMaskAbility)
                {
                    itemPool.Masks.TryGet(out monsterTargetMask, random, monster.Mask.MaskData);
                }

                if (monster.TypeData.HasCopyScheduleAbility)
                {
                    monsterTargetSchedule = monster.Schedule;
                }
            }

            
            //Create a pool of possible monster traits. These get added to other guests to increase difficulty.
            HashSet<TraitData> monsterTraitPool = new HashSet<TraitData>();
            foreach (var item in monsterTypeSet)
            {
                monsterTraitPool.UnionWith(item.PreferredTraits);
            }
            List<TraitData> monsterTraitList = new List<TraitData>(monsterTraitPool);
            
            //pick a number of people who will get the monster trait added.
            int peopleWithMonsterTraitCount = _peopleWithMonsterTraits.GetRandom(random);
            traitPool.WithRandom(peopleWithMonsterTraitCount, AddMonsterTrait);
            void AddMonsterTrait(HashSet<TraitData> traitList)
            {
                var first = traitList.ElementAt(0);
                traitList.Remove(first);
                traitList.Add(monsterTraitList.RandomItem(random));
            }
            
            //People generation
            for (int i = 0; i < personCount; i++)
            {
                //TODO dsaunders - this is a ugly way of forcing the first guest to be marked if a Monster requires it.
                if (i == 0 && (monsterTargetMask != null || monsterTargetSchedule != null))
                {
                    var person = _personGenerator.Generate(random, monsterTargetMask,
                        monsterTargetSchedule, itemPool);
                    var firstMonster = monsterEntities[0];
                    if (firstMonster != null)
                    {
                        firstMonster.SetFollowTarget(person);
                    }
                    generatedCharacters.Add(person);
                }
                else
                {
                    var person = _personGenerator.Generate(random, itemPool);
                    generatedCharacters.Add(person);
                }
                
            }
            
            _characterEntitySet.Assign(generatedCharacters);

            if (monsterEntities.Count > 0)
            {
                for (int i = 0; i < killerCount; i++)
                {
                    MonsterEntity targetMonster = monsterEntities[i % monsterEntities.Count];
                    var killer = _killerGenerator.Generate(random, itemPool, targetMonster);
                    generatedCharacters.Add(killer);
                }
            }
            return generatedCharacters;
        }

        [ContextMenu("Test")]
        public void Test()
        {
            System.Random random = new System.Random();
            for (int i = 0; i < 100; i++)
            {
                int seed = random.Next(Int32.MaxValue);
               
                var generatedCharacters = Generate(personCount:8,monsterCount:1,killerCount:1, seed);
                Debug.Log(PrintDebug(generatedCharacters));
                random = new System.Random(seed);
            }
           
        }

        public static string PrintDebug(List<CharacterEntity> characterEntities)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in characterEntities)
            {
                sb.Append(item.PrintDebug());
                sb.AppendLine();
            }
            return sb.ToString();
        }

    }
}