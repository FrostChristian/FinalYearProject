using System.Collections;
using UnityEngine;



namespace FinalYear {

    public class MainMenu : Menu<MainMenu> {
       [SerializeField] private float _playDelay = 0.01f;

        protected override void Awake() {
            Screen.orientation = ScreenOrientation.Portrait;
            Debug.LogWarning(Screen.orientation);

            base.Awake();
        }
     
        public void OnGameOnePressed() {
            LevelLoader.LoadLevel(2);
            FlagPainterMenu.Open();
        }

        public void OnGameTwoPressed() {
            LevelLoader.LoadLevel(3);
            BoxMatchMenu.Open();
        }

        public override void OnBackPressed() {
            Application.Quit();
        }
    }
}