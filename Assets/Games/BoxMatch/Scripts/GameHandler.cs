/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalYear.BoxMatch {

    public enum MatchCategory { Non, Male, Female, Any } // setup Match Categorys

    public class GameHandler : MonoBehaviour {

        #region Variables
        private static GameHandler _instance;
        public static GameHandler Instance { get => _instance; set => _instance = value; }

        [SerializeField] private static Card _activeCard = default;
        public static Card ActiveCard { get => _activeCard; set => _activeCard = value; }

        private static CardBox _choosenCardBox = default;
        public static CardBox ChoosenCardBox { get => _choosenCardBox; set => _choosenCardBox = value; }

        public int cardLimit = 2;

        public bool _intro = true;

        [Header("Sound")]
        public GameAudioClip[] gameAudioClips; // stores all audio clips; assigned in inspector!
        #endregion

        #region Unity Methods
        private void Awake() {
            if (_instance != null) {
                Destroy(this);
            } else {
                _instance = this;
            }

            if (SceneManager.GetActiveScene().name == "BoxMatch") {
                BoxMatchMenu.Open();
                Debug.Log("GameManager Awake(): Entered Game through BoxMatch");
            }
        }

        private void Start() {
            GUIHandler.Instance.ShowIntroPanel();
            SoundHandler.PlayBackgroundSound(SoundHandler.Sounds.Background);
        }
        #endregion

        public void StartCheckGame() {
            StartCoroutine(CheckEndGame());
        }

        private IEnumerator CheckEndGame() {  // assigned to DialogPanel Button
            yield return new WaitForSeconds(0.5f);
            if (CardHandler.Instance.cardList.Count == 0 && !_intro) {
                GUIHandler.Instance.ShowWinPanel();
                SoundHandler.PlaySound(SoundHandler.Sounds.WinSoundOne);
            }
        }

        public void OnRightCardBoxChoosen() {
            GUIHandler.Instance.UpdateGUI();
            CardHandler.Instance.cardList.Remove(ActiveCard);
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundOne);
            ActiveCard.Destroy();
            
            StartCheckGame();
        }

        public void OnWrongCardBoxChoosen() {
            GUIHandler.Instance.ShowDialogPanel();
            CardHandler.Instance.cardList.Remove(ActiveCard);
            SoundHandler.PlaySound(SoundHandler.Sounds.ShuffleCards);
            ActiveCard.Destroy();

        }
    }
}
