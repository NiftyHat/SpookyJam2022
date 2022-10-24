using System.Collections.Generic;
using Data;
using Data.Monsters;
using Data.Trait;
using Entity;
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
            public GuestItemPool(MaskGenerator.MaskPool maskPool, NameGenerator.NamePool namePool, List<TraitData> traits)
            {
                _maskPool = maskPool;
                _namePool = namePool;
                _traits = traits;
            }
            
            protected readonly MaskGenerator.MaskPool _maskPool;
            protected readonly NameGenerator.NamePool _namePool;
            protected readonly List<TraitData> _traits;

            public MaskGenerator.MaskPool Masks => _maskPool;
            public NameGenerator.NamePool Names => _namePool;

            public List<TraitData> Traits => _traits;
        }


        [SerializeField] protected NameGenerator _nameGenerator;
        [SerializeField] protected MaskGenerator _maskGenerator;
        [SerializeField] protected TraitDataSet _traitData;

        [SerializeField] protected MonsterGenerator _monsterGenerator;
        [SerializeField] protected CharacterEntitySet _characterEntitySet;
        
        protected List<MaskEntity> _maskList;
        protected List<CharacterEntity> _npcList;

        public GuestListGenerator()
        {
            
        }

        public void Generate(int guestCount, int monsterCount = 1, int killerCount = 1)
        {
            System.Random random = new  System.Random();
            int totalCount = guestCount + monsterCount + killerCount;
            MaskGenerator.MaskPool maskPool = _maskGenerator.GetPool();
            int maskCount = maskPool.Count;
            if (totalCount > maskCount)
            {
                totalCount = maskCount;
                guestCount = maskCount - monsterCount - killerCount;
            }
            NameGenerator.NamePool namePool = _nameGenerator.GetPool(random, totalCount * 2);
            GuestItemPool itemPool = new GuestItemPool(maskPool, namePool, _traitData.References);
            var monster = _monsterGenerator.Generate(random, itemPool);
            //_maskList = GenerateMasks(totalCount, _maskData.References, _maskColorData.References,random);
            //_npcList = new List<NPCEntity>(totalCount);
            //var monsters = GenerateMonster(random);
            _characterEntitySet.Add(monster);
        }

        public void GenerateMonster(MonsterEntityTypeData monsterTypeData, GuestItemPool itemPool)
        {
            
        }


    }
}