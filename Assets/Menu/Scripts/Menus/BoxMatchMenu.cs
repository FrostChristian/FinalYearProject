using FinalYear.FlagPainter;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalYear {

    public class BoxMatchMenu : Menu<BoxMatchMenu> {
       [SerializeField] private float _playDelay = 0.01f;

        public override void OnBackPressed() {
            //LevelLoader.LoadLevel(1);
            SceneManager.LoadScene(1);
            Screen.orientation = ScreenOrientation.Portrait;
            MainMenu.Open();
        }
    }
}