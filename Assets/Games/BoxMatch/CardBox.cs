/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;
using UnityEngine.EventSystems;

namespace FinalYear.BoxMatch {

    public class CardBox : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/ {

        #region Variables
       [SerializeField] private int _boxCategory = 1;
        public int GetBoxCategory { get => _boxCategory; }

        #endregion

        #region Unity Methods
        /*
        public void OnPointerUp(PointerEventData eventData) {
            if (_boxCategory == GameHandler.ActiveCard.CardCategory) {
            Debug.Log("Category " );
            }
        }

        public void OnPointerDown(PointerEventData eventData) {

        }
        */
        #endregion
    }
}
