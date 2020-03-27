/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FinalYear.BoxMatch {

    public class CardHandler : MonoBehaviour {

        #region Variables
        private static CardHandler _instance;
        public static CardHandler Instance { get => _instance; set => _instance = value; }

        [HideInInspector] public List<Card> cardList = new List<Card>(); // holds cards so actual cardList wont get messed up

        public Transform cardStash = default;
        public Transform cardPointerStash = default;// holds all pointer positions for the cards to be able to smoothly follow with lerping

        public ColorPair[] cardColors; // asign in inspector

        [SerializeField] protected CardSettings _cardSettings = default; // insert CardSettings here!
        [SerializeField] private Card cardTemplate;

        public List<CardSetupInformation> unansweredCards = new List<CardSetupInformation>();
        public List<CardSetupInformation> answeredCards = new List<CardSetupInformation>();

        #endregion

        #region Unity Methods
        private void Awake() {
            if (_instance != null) {
                Destroy(this);
            } else {
                _instance = this;
            }
            if (cardColors.Length <= 0) {
                Debug.LogWarning("CardHandler.cs, Array of cardColors needs to be assigned in Inspector!");
            }
            _cardSettings.MarkAllCardsUnanswered();

        }
        #endregion

        public void SpawnCards() {
            GameObject pointer = new GameObject("Pointer", typeof(RectTransform)); // create pointer GO for Instantiating
            List<ColorPair> tempColorList = new List<ColorPair>(cardColors); // create a list from color array to work with

            StoreUnansweredCards();

            for (int i = 0; i < unansweredCards.Count; i++) {   // for every cards in settings
                GameObject tempPointer = Instantiate(pointer, cardPointerStash.transform); // create pointer for each card
                Card tempCard = Instantiate(cardTemplate, cardStash.transform); // create card at pointer transform
                tempCard._pointer = tempPointer.transform; // assign this pointer to card for later use (destroying the pointer)
                tempCard.CardInformation = unansweredCards[i];// assign Scriptable Object Info    

                /// Colors (Colors are choosen randomly for each card)
                Image tempCardBorder = tempCard.gameObject.transform.Find("Card_Border").GetComponent<Image>(); // ref to Card Border Img
                Image tempCardBgr = tempCard.gameObject.transform.Find("Card_Bgr").GetComponent<Image>(); // ref to Card bgr Img

                ColorPair randColorPair = tempColorList[Random.Range(0, tempColorList.Count)]; // get a random Color from list
                tempColorList.Remove(randColorPair); // remove this color from the list so that no color appears twice

                tempCardBorder.color = randColorPair.borderColor; // assign the color to the img
                tempCardBgr.color = randColorPair.bgrColor; // assign the color to the img

            }

            SoundHandler.PlaySound(SoundHandler.Sounds.ShuffleCards);
        }

        private void StoreUnansweredCards() {
            for (int i = 0; i < _cardSettings.TotalCards; i++) { // add to ansered card list
                if (!answeredCards.Contains(_cardSettings.GetCardByIndex(i)) && _cardSettings.GetCardByIndex(i).Answered) { // if card has been answeerd and is not jet in the list of answered cards
                    answeredCards.Add(_cardSettings.GetCardByIndex(i));
                }
            }

            if (answeredCards.Count == _cardSettings.TotalCards) { // if all cards are in answered list UNMARK CARDS
                _cardSettings.MarkAllCardsUnanswered();
                answeredCards.Clear(); // clear list for next lvl
            }

            /// list 10 unanswered randomCards from cardSettings ScriptableObj
            /// get rid of all answered cards in unclist
            for (int i = 0; i < _cardSettings.TotalCards; i++) {

                if (_cardSettings.GetCardByIndex(i).Answered) { // if card was answered
                    if (unansweredCards.Contains(_cardSettings.GetCardByIndex(i))) { // if its in the unansweerdCard list remove it
                        unansweredCards.Remove(_cardSettings.GetCardByIndex(i));
                    }
                }
            }

            for (int i = 0; i < _cardSettings.TotalCards; i++) {
                if (unansweredCards.Count == GameHandler.cardLimit) { // if we already have 10 cards return;
                    continue;
                }
                if (!_cardSettings.GetCardByIndex(i).Answered) {
                    // Card was not answered
                    unansweredCards.Add(_cardSettings.GetCardByIndex(i)); // add unaswered ones
                }
            }
        }

        public void CheckForCorrectBox(MatchCategory cardCategory) {
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { // get Pointer data
                position = Input.mousePosition // at mouse position
            };
            List<RaycastResult> results = new List<RaycastResult>(); // store hits in here
            EventSystem.current.RaycastAll(pointerData, results); // raycast to all elements at pointer
            if (results.Count > 0) { // if we hit something
                foreach (RaycastResult rcr in results) {
                    if (rcr.gameObject.GetComponentInParent<CardBox>() != null) { // look for CardBoxes
                        CardBox cb = rcr.gameObject.GetComponentInParent<CardBox>(); // reference to cardbox
                        GameHandler.ChoosenCardBox = cb;
                        if (cb.GetStringBoxCategory == cardCategory) { // check for category
                            // On right Box found!
                            GameHandler.Instance.OnRightCardBoxChoosen();
                            Debug.Log("Found the right Box! " + cb.name);
                        } else {
                            // On wrong Box found!
                            GameHandler.Instance.OnWrongCardBoxChoosen();
                            Debug.Log("Found the wrong Box! " + cb.name);
                        }
                    }
                }
            } else {
                Debug.LogWarning("Card.cs CheckForCorrectBox(): NO OBJECT FOR RAYCAST FOUND!");
            }
        }
    }

    [System.Serializable]
    public class ColorPair {
        public Color borderColor;
        public Color bgrColor;
    }
}
