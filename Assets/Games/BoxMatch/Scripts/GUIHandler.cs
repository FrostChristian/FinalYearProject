/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FinalYear.BoxMatch {

    public class GUIHandler : MonoBehaviour {

        #region Variables
        private static GUIHandler _instance;
        public static GUIHandler Instance { get => _instance; }

        [Header("Assign here!")]
        public Button muteBtn;
        public Sprite muteOnSprite;
        public Sprite muteOffSprite;
        [Space]
        public GameObject dialogPanel;
        public GameObject dialogCard;
        [HideInInspector] public CardSetupInformation _dialogCardInfo;
        [Space]
        public Text headerText;
        public Text stereotypeText;
        public Text questionText;
        public Text explanationText;
        public Text scoreText;
        [Space]
        public GameObject winPanel;
        public Text endText;

        public event EventHandler OnDialogPanelOpen;
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
                OnDialogPanelOpen?.Invoke(this, EventArgs.Empty); // fire event for card animation
                SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundOne);
                animator.SetBool("Open", !isOpen);
                yield return new WaitForSeconds(.5f);
                dialogPanel.SetActive(false);
            }
        }

        public void ShowIntroPanel() {
            // setting up text
            headerText.text = "     - Match the Cards! - ";
            stereotypeText.text = "";
            questionText.text = "";
            explanationText.text = "Try and match the cards in to the box that you think they belong to! \n\n" + "Have Fun!";

            // hide card for intro
            dialogCard.SetActive(false);

            // setting up dialog panel button 
            introButtonFunc += CardHandler.Instance.SpawnCards; // add this to the delegate
            introButtonFunc += UpdateGUI; // add this to the delegate
            dialogPanel.GetComponentInChildren<Button>().onClick.AddListener(introButtonFunc); // add Action to onClick event

            ToggleDialogPanel();
        }

        public void ShowDialogPanel() {
            CardSetupInformation card = GameHandler.ActiveCard.CardInformation;
            MatchCategory box = GameHandler.ChoosenCardBox.GetStringBoxCategory;
           
            // setting up text
            headerText.text = "Let's think about it!";
            stereotypeText.text = ""; //card.Name;
            questionText.text = ""; //"Does >" + card.Name + "< really fit in to box >" + box + "< ?";
            explanationText.text = card.GetDescriptionShort;

            ShowCardInfoInDialogPanel(card);
            ToggleDialogPanel();
        }

        private void ShowCardInfoInDialogPanel(CardSetupInformation card) {
            dialogCard.GetComponent<Card>().CardInformation = card; // set CardSetupInformation for dialogCard to the one from CardSetupInformation; 
            dialogCard.GetComponent<Card>().UpdateCard(); // update the card in the dialogPanel
            dialogCard.SetActive(true); // show the card
        }

        public void ShowWinPanel() {
            endText.text = "Do you want to play more cards?";
            winPanel.SetActive(true);
        }

        public void UpdateGUI() {
            if (introButtonFunc != null) { // if introButtonFunc Action is still active
                GameHandler.Instance._intro = false; // set to true for GH update win check
                dialogPanel.GetComponentInChildren<Button>().onClick.RemoveListener(introButtonFunc); // remove intro button func
            }
        }

        public void OnMuteClicked() {

            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundTwo);
            SoundHandler.ToggleAudio();

            if (!SoundHandler._isMuted) { // switch btn sprites
                muteBtn.GetComponent<Image>().sprite = muteOffSprite;
            } else {
                muteBtn.GetComponent<Image>().sprite = muteOnSprite;
            }
        }

        public void OnRestartClicked() {
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundTwo);
            CardHandler.Instance.SpawnCards();
            UpdateGUI();
            winPanel.SetActive(false);

        }
    }
}
