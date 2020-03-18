/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalYear.BoxMatch {

    public class GameHandler : MonoBehaviour {

        #region Variables
        private static GameHandler _instance;
        public static GameHandler Instance { get => _instance; set => _instance = value; }

        [SerializeField] private static Card activeCard = default;
        public static Card ActiveCard { get => activeCard; set => activeCard = value; }

        public List<Card> cardList = new List<Card>();
        //[HideInInspector] 
        public List<Card> _tempCardList; // holds cards so actual cardList wont get messed up

        private static CardBox _choosenCardBox = default;
        public static CardBox ChoosenCardBox { get => _choosenCardBox; set => _choosenCardBox = value; }

        private static int _score;
        public static int Score { get => _score; }
        public int scorePerCard = 50;

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
            CardSpawner.Instance.SpawnCards();
            GUIHandler.Instance.UpdateGUI();

        }

        private void Update() {
            if (_tempCardList.Count == 0) {
                GUIHandler.Instance.ShowWinPanel();
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
            _tempCardList.Remove(ActiveCard);
            ActiveCard.Destroy();
        }

        public void OnWrongCardBoxChoosen() {
            GUIHandler.Instance.ShowDialogPanel();
        }
    }
}
