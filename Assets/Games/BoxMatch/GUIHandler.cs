/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using UnityEngine;
using UnityEngine.UI;

namespace FinalYear.BoxMatch {

    public class GUIHandler : MonoBehaviour {

        #region Variables
        private static GUIHandler _instance;
        public static GUIHandler Instance { get => _instance; }

        public GameObject dialogPanel;
        public Text scoreText;
        public Text questionText;
        public Text explanationText;
        [Space]
        public GameObject winPanel;
        public Text endScoreText;


        #endregion

        #region Unity Methods

        private void Awake() {
            if (_instance != null) {
                Destroy(this);
            } else {
                _instance = this;
            }          
        }

        #endregion

        public void ShowDialogPanel() {
            var card = GameHandler.ActiveCard.GetCardCategory.ToString();
            var box = GameHandler.ChoosenCardBox.GetBoxCategory.ToString();

            questionText.text = "Does Card >" + card + "< really fit in to box >" + box + "< ?";
            explanationText.text = "No, Card >" + card + "< would not agree with you! Here is why it does not fit in box >" + box + "<";
            dialogPanel.SetActive(true);
        }

        public void ShowWinPanel() {
            endScoreText.text = "You have a score of " + GameHandler.Score.ToString() + "! Amazing!";
            winPanel.SetActive(true);
        }

        public void UpdateGUI() {
            scoreText.text = "Score: " + GameHandler.Score.ToString();
        }

        public void OnRestartClicked() {
            GameHandler.ResetScore();
            CardSpawner.Instance.SpawnCards();
            UpdateGUI();
            winPanel.SetActive(false);
        }
    }
}
