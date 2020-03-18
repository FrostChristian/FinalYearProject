/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.Collections.Generic;

namespace FinalYear.BoxMatch {

    public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        #region Variables
        [Header("Card")]
        [SerializeField] private int _cardCategory = 1;
        public int GetCardCategory { get => _cardCategory; }
        [Space]
        [SerializeField] private bool _draggingCard;
        [Space]
        [Header("Card Lerp")]
        [SerializeField] private float lerpSpeed = 10f;
        [SerializeField] private float lerpStopDistance = 0.1f;
        [SerializeField] private float _lerpUpdateTime = .3f;
        public Transform _pointer = default; // lerp target!
        #endregion

        #region Unity Methods
        private void Awake() {
            GameHandler.Instance._tempCardList.Add(this);
        }

        private void Start() {
            StartCoroutine(LerpCard()); // start checking this card for its position, if its not at its pointer position it will lerp to it
        }

        private void Update() {
            if (_draggingCard) {
                transform.position = Input.mousePosition; // position card at mouse pointer
            }
        }
        #endregion

        public void OnPointerDown(PointerEventData eventData) {
            GameHandler.ActiveCard = this; // set active card to this
            _draggingCard = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            _draggingCard = false;
            CheckForCorrectBox();
        }

        private IEnumerator LerpCard() {// lerp back to original position
            while (Vector3.Distance(transform.position, _pointer.position) > lerpStopDistance && !_draggingCard) { // dist check
                transform.position = Vector3.Lerp(transform.position, _pointer.position, lerpSpeed * Time.deltaTime); //lerp
                //Debug.Log(Vector3.Distance(transform.position, _pointer.position) + "Lerping");
                yield return null;
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(_lerpUpdateTime, _lerpUpdateTime +.1f)); // wait before lerping again!
            StartCoroutine(LerpCard()); // loop
        }

        private void CheckForCorrectBox() {
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { // get Pointer data
                position = Input.mousePosition // at mouse position
            };
            List<RaycastResult> results = new List<RaycastResult>(); // store hits in here
            EventSystem.current.RaycastAll(pointerData, results); // raycast to all elements at pointer
            if (results.Count > 0) { // if we hit something
                foreach (RaycastResult rcr in results) {
                    if (rcr.gameObject.GetComponentInParent<CardBox>() != null) { // look for CardBoxes
                        CardBox cb = rcr.gameObject.GetComponentInParent<CardBox>(); // referenc to cardbox
                        GameHandler.ChoosenCardBox = cb;
                        if (cb.GetBoxCategory == GetCardCategory) { // check for category
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

        public void Destroy() {
            Destroy(_pointer.gameObject); // destroy pointer obj
            Destroy(gameObject);
        }
    }
}