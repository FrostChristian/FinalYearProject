/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FlagPainter {

    public class GUIHandler : MonoBehaviour {

        #region Variables
        public static GUIHandler Instance;
        [SerializeField] private Text _questionText = default;
        [SerializeField] private Text _flagNameText = default;
        [SerializeField] private Text _scoreText = default;
        [SerializeField] private List<GameObject> _colorSplashesInPanel = new List<GameObject>();

        private Flag _currentFlag = default;

        public List<GameObject> colorSplashPrefab = new List<GameObject>();
        public Image paintBucketImage = default;
        public Image paintBucketHighlightImage = default; // in case of active black color, change color of highlight
        [SerializeField] private List<GameObject> _colorSpawns = new List<GameObject>();

        public static bool hintActive;

        #endregion

        #region Unity Methods
        private void Awake() {
            Instance = this;
        }

        void Start() {
            UpdateGUI();
        }
        #endregion

        private void UpdateGUI() {
            hintActive = false;
            _currentFlag = FlagHandler.GetCurrentlyActiveFlagGameObject().GetComponent<Flag>();
            _flagNameText.text = _currentFlag._flagName.ToUpper(); // convert to uppercase string for FONT effect
            _questionText.text = "Can you fill in the correct colors for the " + _currentFlag._flagName + " Flag?";
            _scoreText.text = "Score: " + GameHandler.Instance.Score.ToString();
            GameHandler.Instance.ResetActiveColor();
            paintBucketImage.color = GameHandler.DefaultColor;
            SpawnFlagColorSplashButtons();
        }

        public static void UpdateGUI_Static() {
            Instance.UpdateGUI();
        }

        private void SpawnFlagColorSplashButtons() {
            ClearColorSplashButtons();
            for (int i = 0; i < _currentFlag.FlagMatches.Count; i++) {
                GameObject go = Instantiate(colorSplashPrefab[Random.Range(0, colorSplashPrefab.Count)], _colorSpawns[i].transform);
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

        public void OnNextClicked() {
            FlagHandler.Instance.NextFlag();
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundOne);
        }

        public void OnPreviousClicked() {
            FlagHandler.Instance.PreviousFlag();
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundOne);
        }

        public void OnHintClicked() {
                        if (hintActive) {
                hintActive = false;
            } else {
                hintActive = true;
                GameHandler.Instance.AddScore(-10);
                _scoreText.text = "score: " + GameHandler.Instance.Score.ToString();
            }
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundOne);
        }

        public void OnResetClicked() {
            GameHandler.Instance.ResetActiveColor();
            foreach (SpriteRenderer rend in FlagHandler.GetCurrentlyActiveFlagGameObject().GetComponentsInChildren<SpriteRenderer>()) {
                GameHandler.SetSpriteRendererColor(rend, GameHandler.DefaultColor);
            }
            SoundHandler.PlaySound(SoundHandler.Sounds.EraserSoundOne);
        }

        public void OnEraserClicked() {
            GameHandler.Instance.ResetActiveColor();
            paintBucketImage.color = GameHandler.DefaultColor;
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundOne);
        }

        private void OnQuitClick() {
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundTwo);
            Application.Quit();
        }

        public static void OnQuitClick_Static() {
            Instance.OnQuitClick();
        }
    }
}
