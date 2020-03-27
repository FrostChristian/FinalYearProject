/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FinalYear.BoxMatch {

    public class GUIHandler : MonoBehaviour {

        #region Variables
        private static GUIHandler _instance;
        public static GUIHandler Instance { get => _instance; }

        public Button muteBtn;
        public Sprite muteOnSprite;
        public Sprite muteOffSprite;
        [Space]
        public GameObject dialogPanel;
        public Text headerText;
        public Text stereotypeText;
        public Text questionText;
        public Text explanationText;
        public Text scoreText;
        [Space]
        public GameObject winPanel;
        public Text endScoreText;

        private UnityAction introButtonFunc; // holds funtions for Intro panel button click
        #endregion

        #region Unity Methods
        private void Awake() {
            if (_instance != null) {
                Destroy(this);
            } else {
                _instance = this;
            }
            muteBtn.onClick.AddListener(() => OnMuteClicked()); // setup mutebtn
            dialogPanel.GetComponentInChildren<Button>().onClick.AddListener(() => ToggleDialogPanel()); // setup Dialog panel btn
            UpdateGUI(); // set score in GUI initially
        }
        #endregion

        public void ToggleDialogPanel() {
            StartCoroutine(DialogPanelAnimator());
        }

        public IEnumerator DialogPanelAnimator() {
            Animator animator = dialogPanel.GetComponent<Animator>();
            bool isOpen = animator.GetBool("Open");

            if (!dialogPanel.activeSelf) { // if panel is not active activate it and play ani
                dialogPanel.SetActive(true);
                animator.SetBool("Open", !isOpen);
            } else {
                animator.SetBool("Open", !isOpen);
                yield return new WaitForSeconds(.5f);
                dialogPanel.SetActive(false);
            }
        }

        public void ShowIntroPanel() {
            // setting up text
            headerText.text = " - Match the Cards! - ";
            stereotypeText.text = "";
            questionText.text = "Try and match the cards in to the box that you think they belong to!";
            explanationText.text = "Have Fun!";

            // setting up dialog panel button 
            introButtonFunc += CardHandler.Instance.SpawnCards; // add this to the delegate
            introButtonFunc += UpdateGUI; // add this to the delegate
            dialogPanel.GetComponentInChildren<Button>().onClick.AddListener(introButtonFunc); // add Action to onClick event

            ToggleDialogPanel();
        }

        public void ShowDialogPanel() {
            var card = GameHandler.ActiveCard.CardInformation.GetName;
            var box = GameHandler.ChoosenCardBox.GetStringBoxCategory;

            headerText.text = "Let's think about that!";
            stereotypeText.text = card;
            questionText.text = "Does >" + card + "< really fit in to box >" + box + "< ?";
            explanationText.text = "Info: " + GameHandler.ActiveCard.CardInformation.GetDescriptionShort;

            ToggleDialogPanel();
        }

        public void ShowWinPanel() {
            endScoreText.text = "You have a score of " + GameHandler.Score.ToString() + "! Amazing!";
            winPanel.SetActive(true);
        }

        public void UpdateGUI() {
            scoreText.text = "Score:\n" + GameHandler.Score.ToString();
            if (introButtonFunc != null) { // if Intro Action is still set
                GameHandler.Instance._intro = false; // set to true for GH update win check
                dialogPanel.GetComponentInChildren<Button>().onClick.RemoveListener(introButtonFunc); // remove intro button func
            }
        }

        public void OnMuteClicked() {
            SoundHandler.ToggleAudio();
            if (!SoundHandler._isMuted) {
                muteBtn.GetComponent<Image>().sprite = muteOffSprite;
            } else {
                muteBtn.GetComponent<Image>().sprite = muteOnSprite;
            }
        }

        public void OnRestartClicked() {
            GameHandler.ResetScore();
            CardHandler.Instance.SpawnCards();
            UpdateGUI();
            winPanel.SetActive(false);
        }
    }
}
