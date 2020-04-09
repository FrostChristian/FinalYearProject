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
        [SerializeField] private MatchCategory _cardCategory = default;  // Store target for this card 
        [SerializeField] [Multiline] private string _descriptionShort = default; // general details       
        [SerializeField] private bool _answered = false; // general details      
        private ColorPair _assignedColors;

        public string Name { get => _stereotypeName; set => _stereotypeName = value;  }
        public MatchCategory GetCardCategory { get { return _cardCategory; } }
        public string GetDescriptionShort { get { return _descriptionShort; } }
        public bool Answered { get => _answered; set => _answered = value; }
        public ColorPair AssignedColors { get => _assignedColors; set => _assignedColors = value; }
    }
}
