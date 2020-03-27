/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using UnityEngine;

namespace FinalYear.BoxMatch {

    [Serializable]
    public class CardSetupInformation {

        [SerializeField] private string _stereotypeName = default; // used in menu
        [SerializeField] private MatchCategory _cardCategory;  // Store target for this card 
        [SerializeField] [Multiline] private string _descriptionShort = default; // general details       
        [SerializeField] private bool _answered = false; // general details       

        public string GetName { get { return _stereotypeName; } }
        public MatchCategory GetCardCategory { get { return _cardCategory; } }
        public string GetDescriptionShort { get { return _descriptionShort; } }
        public bool Answered { get => _answered; set => _answered = value; }

    }
}
