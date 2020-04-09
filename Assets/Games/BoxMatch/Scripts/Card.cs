/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

namespace FinalYear.BoxMatch {

    public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        #region Variables
        [Header("--Card--")]
        public bool interactable = true;
        public bool draggingCard;
        [Space]
        [Header("Card Movement")]
        [SerializeField] private float lerpSpeed = 10f;
        [SerializeField] private float lerpStopDistance = 0.1f;
        [SerializeField] private float _lerpUpdateTime = .3f;
        public Transform _pointer = default; // lerp target!
        [Header("Card Information")]
        private CardSetupInformation _cardInformation = default; // holds all card info
        public CardSetupInformation CardInformation { get => _cardInformation; set => _cardInformation = value; }
        [Space]
        [SerializeField] private Image _backgroundColor;
        [SerializeField] private Image _borderColor;
        [SerializeField] private Text _cardText = default; // display text on card

        #endregion

        #region Unity Methods
        private void Awake() {
            if (interactable) {
                CardHandler.Instance.cardList.Add(this);
            }
        }

        private void Start() {
            if (interactable) {
                StartCoroutine(LerpCard()); // start checking this card for its position, if its not at its pointer position it will lerp to it
                UpdateCard();
            }
        }

        private void Update() {
            if (interactable && draggingCard) {
                transform.position = Input.mousePosition; // position card at mouse pointer
            }
        }
        #endregion

        public void UpdateCard() {
            _backgroundColor.color = CardInformation.AssignedColors.bgrColor;
            _borderColor.color = CardInformation.AssignedColors.borderColor;
            _cardText.text = CardInformation.Name;
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (interactable) {
                //GameHandler.ActiveCard = this; // set active card to this
                GameHandler.ActiveCard = this; // set active card to this
                draggingCard = true;
                SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundTwo);
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (interactable) {
                draggingCard = false;
                CardHandler.Instance.CheckForCorrectBox(GetEnumCardCategory());
                SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundOne);
            }
        }

        private IEnumerator LerpCard() {// lerp back to original position
            while (Vector3.Distance(transform.position, _pointer.position) > lerpStopDistance && !draggingCard) { // dist check
                transform.position = Vector3.Lerp(transform.position, _pointer.position, lerpSpeed * Time.deltaTime); //lerp
                //Debug.Log(Vector3.Distance(transform.position, _pointer.position) + "Lerping");
                yield return null;
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(_lerpUpdateTime, _lerpUpdateTime + .1f)); // wait before lerping again!
            StartCoroutine(LerpCard()); // loop
        }

        public MatchCategory GetEnumCardCategory() {
            return CardInformation.GetCardCategory;
        }

        public void Destroy() {
            CardInformation.Answered = true;
            Destroy(_pointer.gameObject); // destroy pointer obj
            Destroy(gameObject);
        }
    }
}