using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardUI 
{
    public class MaskedGuestMenuWidget : MonoBehaviour
    {
        [SerializeField]
        private MaskedGuestCardWidget cardPrefab;
        [SerializeField] private Transform cardContainer;
        [SerializeField] private Transform selectedCardContainer;
        //Animation Hell
        public Transform[] lerpTargets;

        public float unselectedScale = 0.5f;
        public float selectedScalm = 1.0f;

        public void Initialize()
        {
        }


    }
}
