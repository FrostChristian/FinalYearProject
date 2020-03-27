/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalYear.BoxMatch {

    public enum MatchCategory { Non, Male, Female, Any } // setup Match Categorys

    public class GameHandler : MonoBehaviour {

        #region Variables
        private static GameHandler _instance;
        public static GameHandler Instance { get => _instance; set => _instance = value; }

        [SerializeField] private static Card activeCard = default;
        public static Card ActiveCard { get => activeCard; set => activeCard = value; }

        private static CardBox _choosenCardBox = default;
        public static CardBox ChoosenCardBox { get => _choosenCardBox; set => _choosenCardBox = value; }

        private static int _score;
        public static int Score { get => _score; }
        public int scorePerCard = 50;

        public static int cardLimit = 10;

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

            _score = 0;

            if (SceneManager.GetActiveScene().name == "BoxMatch") {
                BoxMatchMenu.Open();
                Debug.Log("GameManager Awake(): Entered Game through BoxMatch");
            }
        }

        private void Start() {
            GUIHandler.Instance.ShowIntroPanel();
            SoundHandler.PlayBackgroundSound(SoundHandler.Sounds.Background);
        }

        private void Update() {
            if (CardHandler.Instance.cardList.Count == 0 && !_intro) {
                GUIHandler.Instance.ShowWinPanel();
                SoundHandler.PlaySound(SoundHandler.Sounds.WinSoundOne);
            }
        }
        #endregion

        private void AddScore(int score) {
            _score += score;
        }

        public static void ResetScore() {
            _score = 0;
        }

        public void OnRightCardBoxChoosen() {
            AddScore(scorePerCard);
            GUIHandler.Instance.UpdateGUI();
            CardHandler.Instance.cardList.Remove(ActiveCard);
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundOne);
            ActiveCard.Destroy();
        }

        public void OnWrongCardBoxChoosen() {
            GUIHandler.Instance.ShowDialogPanel();
            SoundHandler.PlaySound(SoundHandler.Sounds.ShuffleCards);
        }
    }
}
