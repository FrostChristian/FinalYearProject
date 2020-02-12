/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlagPainter {

    public class GameHandler : MonoBehaviour {

        #region Variables
        private static GameHandler _instance;
        public static GameHandler Instance { get => _instance; set => _instance = value; }

        [SerializeField] private static Color _defaultColor = new Color(1f, 1f, 1f, 1f); // set white as default
        public static Color DefaultColor { get => _defaultColor; }

        [SerializeField] private static Color _activeColor; // store active color here
        public static Color ActiveColor { get => _activeColor; set => _activeColor = value; }

        [SerializeField] private int _score = 0;
        public int Score { get => _score; }

        private bool _isTouchInputActive;

        /// <summary>
        /// Color spawn
        /// </summary>


        #endregion

        #region Unity Methods
        private void Awake() {
            if (_instance != null) { // singleton Gamehandler
                Destroy(this);
            } else {
                _instance = this;
            }
            ResetActiveColor(); // set active to default color
        }

        private void Update() {
            HandleUserInput(); // listen for input

        }
        #endregion Unity Methods

        public void UpdadeActiveColor(Color color) {
            _activeColor = color;
        }

        public void ResetActiveColor() {
            _activeColor = _defaultColor;
        }

        public static void SetSpriteRendererColor(SpriteRenderer rend, Color color) {
            rend.color = color;
        }

        public IEnumerator HandleOnFlagComplete() { // enum for performance
            yield return new WaitForSeconds(1f);
            while (!Flag.isFlagCompletelyFilled) { // always check loop!
                //Debug.Log("TICK GH");
                yield return new WaitForSeconds(.1f);
            }
            _score += 50;
            FlagHandler.Instance.NextFlag();
            GUIHandler.UpdateGUI_Static();
        }

        /// ---------------------------------- User Input ---------------------------------- ///
        private void HandleUserInput() {

            if (Input.GetKeyDown(KeyCode.Escape)) {
                GUIHandler.OnQuitClick_Static();
            }

            if (Input.GetAxisRaw("Fire1") != 0) { // check for User input on "Fire1" Input
                if (!_isTouchInputActive) { // make sure this will only be called once
                    _isTouchInputActive = true;
                    //call click event here
                    OnUserClick();
                }
            } else {
                _isTouchInputActive = false;
            }

            if (Input.GetAxisRaw("Fire1") > 0) {
                //call hold event here
                Debug.Log("IM HOLDING");
            }
        }

        private void OnUserClick() {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Drawing"));  // Input.GetTouch(0).position == for the app
            if (hit.collider != null) { // if we hit something
                SetSpriteRendererColor(hit.collider.gameObject.GetComponent<SpriteRenderer>(), _activeColor); // get the renderer and change its color
            }
        }
        /// ---------------------------------- +User Input ---------------------------------- ///
    }
}
