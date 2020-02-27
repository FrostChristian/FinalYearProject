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
        [SerializeField] private bool _draggingCard;
        [SerializeField] private Vector3 _originalPosition = Vector3.zero;
        [SerializeField] private Vector3 _currentPosition;
        [SerializeField] private float lerpSpeed = 10f;
        [SerializeField] private float lerpStopDistance = 0.1f;
        [SerializeField] private int _cardCategory = 1;
        public int GetCardCategory { get => _cardCategory; }

        #endregion

        #region Unity Methods
        private void Awake() {
            GameHandler.Instance._tempCardList.Add(this);
        }
        private void Update() {
            if (_originalPosition == Vector3.zero) { // wait for grouplayout placement in scene and init position vars
                _originalPosition = transform.position;
            }

            if (_draggingCard) {
                transform.position = Input.mousePosition; // position card at mouse pointer
                _currentPosition = transform.position;
            } else {

            }
            //Debug.Log("Curr pos : " + _currentPosition + "\n Real pos: " + gameObject.transform.position + "\n Orig Pos: " + _originalPosition + "\n Name: " + gameObject.transform.name);
        }

        public void OnPointerDown(PointerEventData eventData) {
            GameHandler.ActiveCard = this; // set active card to this
            _originalPosition = transform.position;
            _draggingCard = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            _draggingCard = false;
            CheckForCardBox();
        }

        private IEnumerator MoveCardToOrigin() {// move cards back to origin
            while (Vector3.Distance(transform.position, _originalPosition) > lerpStopDistance) { // lerp back to original position
                transform.position = Vector3.Lerp(transform.position, _originalPosition, lerpSpeed * Time.deltaTime);
                Debug.Log(Vector3.Distance(transform.position, _originalPosition) + "Lerping");
                yield return null;
            }
        }

        private void CheckForCardBox() {
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { // get Pointer data
                position = Input.mousePosition // at mouse position
            };
            List<RaycastResult> results = new List<RaycastResult>(); // store hits in here
            EventSystem.current.RaycastAll(pointerData, results); // raycast all elements at pointer

            if (results.Count > 0) { // if we hit something
                foreach (RaycastResult rcr in results) {
                    if (rcr.gameObject.GetComponentInParent<CardBox>() != null) { // look for CardBoxes
                        CardBox cb = rcr.gameObject.GetComponentInParent<CardBox>();
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
            }
            StartCoroutine(MoveCardToOrigin()); 
        }
        #endregion
        public void Destroy() {
            GameHandler.Instance._tempCardList.Remove(this);
            Destroy(gameObject);
        }
    }
}