/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlagPainter {

    public class GUIHandler : MonoBehaviour {

        #region Variables
        public static GUIHandler _instance;
        [SerializeField] private Text _questionText = default;
        [SerializeField] private Text _flagNameText = default;
        [SerializeField] private Text _scoreText = default;
        [SerializeField] private List<GameObject> _colorSplashesInPanel = new List<GameObject>();

        private Flag _currentFlag = default;

        public GameObject colorSplashPrefab = default;
        [SerializeField] private List<GameObject> _colorSpawns = new List<GameObject>();

        #endregion

        #region Unity Methods
        private void Awake() {
            _instance = this;
        }

        void Start() {
            UpdateGUI();
        }
        #endregion

        private void UpdateGUI() {
            _currentFlag = FlagHandler.GetCurrentlyActiveFlagGameObject().GetComponent<Flag>();

            _flagNameText.text = _currentFlag._flagName.ToUpper(); // convert to uppercase string for FONT effect
            _questionText.text = "Can you fill in the correct colors for the " + _currentFlag._flagName + " Flag?";
            _scoreText.text = "score: " + GameHandler.Instance.Score.ToString();
            GameHandler.Instance.ResetActiveColor();
            SpawnFlagColorSplashButtons();
        }

        private void SpawnFlagColorSplashButtons() {
            ClearColorSplashButtons();
            for (int i = 0; i < _currentFlag.FlagMatches.Count; i++) {
                GameObject go = Instantiate(colorSplashPrefab, _colorSpawns[i].transform);
                go.GetComponentInChildren<ColorButton>().myColor = _currentFlag.FlagMatches[i].targetColor;
                _colorSplashesInPanel.Add(go.gameObject);
            }
        }

        private void ClearColorSplashButtons() {
            foreach (GameObject go in _colorSplashesInPanel) {
                Destroy(go);
            }
            _colorSplashesInPanel.Clear();
        }

        public static void UpdateGUI_Static() {
            _instance.UpdateGUI();
        }

        public void OnNextClicked() {
            FlagHandler.Instance.NextFlag();
            UpdateGUI();
        }

        public void OnPreviousClicked() {
            FlagHandler.Instance.PreviousFlag();
            UpdateGUI();
        }

        public void OnResetClicked() {
            GameHandler.Instance.ResetActiveColor();
            foreach (SpriteRenderer rend in FlagHandler.GetCurrentlyActiveFlagGameObject().GetComponentsInChildren<SpriteRenderer>()) {
                GameHandler.SetSpriteRendererColor(rend, GameHandler.DefaultColor);
            }
        }

        public void OnQuitClick() {
            Application.Quit();
        }

        public static void OnQuitClick_Static() {
            _instance.OnQuitClick();
        }
    }
}
