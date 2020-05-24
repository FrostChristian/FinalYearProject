/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using System.Collections;
using UnityEngine;

namespace FinalYear.FlagPainter {

    //[System.Serializable]
    public class FlagMatch : MonoBehaviour { // PUT ON FLAG SPRITE; holds target color of sprite and checks if it is correctly filled

        #region Variables
        public Color targetColor;
        public bool isFilled;
        [SerializeField] private SpriteRenderer _rend;
        [SerializeField] private GameObject _mark;
        private Sprite _incorrectSprite;
        private Sprite _correctSprite;
        #endregion

        #region Unity Methods
        void Start() {
            _incorrectSprite = FlagHandler.Instance.incorrectSprite;
            _correctSprite = FlagHandler.Instance.correctSprite;
            _rend = GetComponent<SpriteRenderer>();
            _mark = gameObject.transform.GetChild(0).gameObject;
            //_mark.SetActive(false);
            StartCoroutine(TICK());
        }
        #endregion

        private void CheckIfColorFilled() { // checks attached sprite for fill color
            HandleMarkSprites();
            if (!Mathf.Approximately(_rend.color.r, targetColor.r)) {
                isFilled = false;
                return;
            }
            if (!Mathf.Approximately(_rend.color.g, targetColor.g)) {
                isFilled = false;
                return;
            }
            if (!Mathf.Approximately(_rend.color.b, targetColor.b)) {
                isFilled = false;
                return;
            }
            if (!Mathf.Approximately(_rend.color.a, targetColor.a)) {
                isFilled = false;
                return;
            }
            isFilled = true;
        }

        private void HandleMarkSprites() {

            if (GUIHandler.hintActive) {
                _mark.SetActive(true);
            } else {
                _mark.SetActive(false);
                return;
            }

            if (_mark.GetComponent<SpriteRenderer>() != null) {
                if (isFilled) {
                    _mark.GetComponent<SpriteRenderer>().sprite = _correctSprite;
                } else {
                    _mark.GetComponent<SpriteRenderer>().sprite = _incorrectSprite;
                }
            } else {
                _mark.AddComponent<SpriteRenderer>();
            }
        }

        private IEnumerator TICK() { // enum for performance
            while (true) { // always check loop!
                CheckIfColorFilled();
                yield return new WaitForSeconds(.1f);
            }
        }

    }
}
