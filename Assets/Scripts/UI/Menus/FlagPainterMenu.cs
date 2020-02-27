using FinalYear.FlagPainter;
using System.Collections;
using UnityEngine;



namespace FinalYear {

    public class FlagPainterMenu : Menu<FlagPainterMenu> {

       [SerializeField] private float _playDelay = 0.01f;
        public Canvas myCanvas;

        private void Start() {
            myCanvas = GetComponent<Canvas>();
            myCanvas.worldCamera = Camera.main;
        }


        public override void OnBackPressed() {
            Debug.Log("Back");
            SoundHandler.PlaySound(SoundHandler.Sounds.ButtonPressSoundTwo);
            LevelLoader.LoadLevel(1);
            MainMenu.Open();
        }
    }
}