﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalYear.BoxMatch {

    [CreateAssetMenu(fileName = "CardSet", menuName = "BoxMatchGame/Create new Card Set", order = 1)]

    public class CardSettings : ScriptableObject {

        [SerializeField] private List<CardSetupInformation> _cardsSOList = default;
        public List<CardSetupInformation> CardsSOList { get => _cardsSOList; set => _cardsSOList = value; }

        public int TotalCards { get { return CardsSOList.Count; } }


        public CardSetupInformation GetCardByIndex(int index) {
            return CardsSOList[index];
        }

        public void MarkAllCardsUnanswered() {
            for (int i = 0; i < CardsSOList.Count; i++) {
                CardsSOList[i].Answered = false;
            }
        }

    }
}
