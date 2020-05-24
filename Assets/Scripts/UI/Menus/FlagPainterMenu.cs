using FinalYear.FlagPainter;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalYear {

    public class FlagPainterMenu : Menu<FlagPainterMenu> {

       [SerializeField] private float _playDelay = 0.01f;
        public Canvas myCanvas;

        private void Start() {
            myCanvas = GetComponent<Canvas>();
            myCanvas.worldCamera = Camera.main;
        }


        public override void OnBackPressed() {
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundTwo);
            //LevelLoader.LoadLevel(1);
            SceneManager.LoadScene(1);
            Screen.orientation = ScreenOrientation.Portrait;
            MainMenu.Open();
        }
    }
}