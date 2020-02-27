/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;

namespace FinalYear.BoxMatch {

    public class CardHandler : MonoBehaviour {

        #region Variables
        private static CardHandler _instance;
        public static CardHandler Instance { get => _instance; set => _instance = value; }

        public GameObject cardStash = default;
        #endregion

        #region Unity Methods

        private void Awake() {
            if (_instance != null) {
                Destroy(this);
            } else {
                _instance = this;
            }
        }

        void Start() {
            SpawnCards();
        }

        public void SpawnCards() {
            foreach (Card card in GameHandler.Instance.cardList) {
                Instantiate(card, cardStash.transform);
            }
        }

        void Update() {        
        
        }
        #endregion
    }
}
