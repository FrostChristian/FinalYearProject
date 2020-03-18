/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;

namespace FinalYear.BoxMatch {

    public class CardSpawner : MonoBehaviour {

        #region Variables
        private static CardSpawner _instance;
        public static CardSpawner Instance { get => _instance; set => _instance = value; }
        public  GameObject pointer = default;
        public Transform cardStash = default;
        public Transform cardPointerStash = default;// holds all pointer positions for the cards to be able to smoothly follow with lerping
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
            //SpawnCards();
        }

        public void SpawnCards() {
            //GameObject pointer = new GameObject("Pointer", typeof(RectTransform)); // create pointer GO for Instantiating

            foreach (Card card in GameHandler.Instance.cardList) {
                GameObject tempPointer = Instantiate(pointer, cardPointerStash.transform); // create pointer for each card
                Card tempCard = Instantiate(card,cardStash.transform); // create card at pointer transform
                tempCard._pointer = tempPointer.transform; // assign pointer to card for later destroy
            }
        }

        void Update() {

        }
        #endregion
    }
}
