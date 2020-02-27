using System.Collections;
using UnityEngine;



namespace FinalYear {

    public class MainMenu : Menu<MainMenu> {
       [SerializeField] private float _playDelay = 0.01f;

        protected override void Awake() {
            base.Awake();
        }
     
        public void OnGameOnePressed() {
            Debug.Log("Playing Game ONE");
            LevelLoader.LoadLevel(2);
            FlagPainterMenu.Open();
        }

        public void OnGameTwoPressed() {
            Debug.Log("Playing Game TWO");
            LevelLoader.LoadLevel(3);
            BoxMatchMenu.Open();
        }

        public void OnGameThreePressed() {
            Debug.Log("Playing Game THREE");

        }

        public void OnGameFourPressed() {
            Debug.Log("Playing Game FOUR");

        }

        public override void OnBackPressed() {
            Debug.Log("QUIT GAME");
            Application.Quit();
        }
    }
}