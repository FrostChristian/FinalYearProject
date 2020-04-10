/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace FinalYear.BoxMatch {

    public class CardHandler : MonoBehaviour {

        #region Variables
        private static CardHandler _instance;
        public static CardHandler Instance { get => _instance; set => _instance = value; }

        [HideInInspector] 
        public List<Card> cardList = new List<Card>(); // holds cards so actual cardList wont get messed up

        public Transform cardStash = default;
        public Transform cardPointerStash = default;// holds all pointer positions for the cards to be able to smoothly follow with lerping

        public ColorPair[] cardColors; // asign in inspector

        [SerializeField] protected CardSettings _cardSettings = default; // insert CardSettings here!
        [SerializeField] private Card cardTemplate;

        public List<CardSetupInformation> unansweredCards = new List<CardSetupInformation>();
        public List<CardSetupInformation> answeredCards = new List<CardSetupInformation>();

        private GameObject pointer;
        #endregion

        #region Unity Methods
        private void Awake() {
            if (_instance != null) {
                Destroy(this);
            } else {
                _instance = this;
            }

            Init();

        }
        #endregion
        
        private void Init() {
            if (cardColors.Length <= 0) {
                Debug.LogWarning("CardHandler.cs, Array of cardColors needs to be assigned in Inspector!");
            }

            _cardSettings.MarkAllCardsUnanswered();

            LoadCardsFromSO();

            if (pointer == null) {
                pointer = new GameObject("Pointer", typeof(RectTransform)); // create pointer GO for pointer Instantiating
                pointer.transform.SetParent(gameObject.transform);
            }
        }

        public void SpawnCards() {           

            List<ColorPair> tempColorList = new List<ColorPair>(cardColors); // create a list from color array to work with

            for (int i = 0; i < GetCardLimit(); i++) {   // for every cards in settings
                GameObject tempPointer = Instantiate(pointer, cardPointerStash.transform); // create pointer for each card
                Card tempCard = Instantiate(cardTemplate, cardStash.transform); // create card at pointer transform

                /// Colors (Colors are choosen randomly for each card)
                ColorPair randColorPair = tempColorList[UnityEngine.Random.Range(0, tempColorList.Count)]; // get a random Color from list
                tempColorList.Remove(randColorPair); // remove this color from the list so that no color appears twice
                unansweredCards[i].AssignedColors = randColorPair; // assign the colorpair to the CardSetupInformation

                tempCard._pointer = tempPointer.transform; // assign pointer to card for later use (destroying the pointer)
                tempCard.CardInformation = unansweredCards[i];// assign CardSetupInformation to card
            }

            SoundHandler.PlaySound(SoundHandler.Sounds.ShuffleCards);
        }

        private int GetCardLimit() {
            if (unansweredCards.Count < GameHandler.Instance.cardLimit) {
                return unansweredCards.Count;
            } else {
                return GameHandler.Instance.cardLimit;
            }
            //return Mathf.Min( GameHandler.Instance.cardLimit, unansweredCards.Count);
        }

        public void RemoveCardFromUnanswered(CardSetupInformation cardInfo) {
            unansweredCards.Remove(cardInfo);
            answeredCards.Add(cardInfo);
        }

        private void LoadCardsFromSO() {// sort all cards by answered and unanswered
            for (int i = 0; i < _cardSettings.TotalCards; i++) {
                if (!_cardSettings.GetCardByIndex(i).Answered && !unansweredCards.Contains(_cardSettings.GetCardByIndex(i))) {  // if card unanswerded add to list
                    unansweredCards.Add(_cardSettings.GetCardByIndex(i));
                } else if (_cardSettings.GetCardByIndex(i).Answered && !answeredCards.Contains(_cardSettings.GetCardByIndex(i))) {  // if answered add to ansered list
                    answeredCards.Add(_cardSettings.GetCardByIndex(i)); // add unaswered ones
                }
            }
            ExtentionMethods.Shuffle(unansweredCards); // shuffel cards for random display
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
