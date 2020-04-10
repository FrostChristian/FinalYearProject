/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;

namespace FinalYear.BoxMatch {

    public class CardBox : MonoBehaviour {

        #region Variables
        [SerializeField] private MatchCategory _boxCategory; // Store target for this card 
        public MatchCategory GetStringBoxCategory { get { return _boxCategory; } }
        private GameObject _highlight;
        #endregion

        #region Unity Methods
        private void Awake() {
            _highlight = gameObject.transform.Find("Highlight").gameObject;
        }

        private void Update() {
            HandleHighlight();
        }
        #endregion

        private void HandleHighlight() {
            if (GameHandler.ActiveCard != null &&
                GameHandler.ActiveCard.draggingCard &&
                RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, null)) { //check if pointer is over the box and if we have a dragging card
                _highlight.SetActive(true); // show highlight
            } else {
                _highlight.SetActive(false);
            }
        }
    }
}
