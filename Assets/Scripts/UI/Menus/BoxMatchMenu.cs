using FinalYear.FlagPainter;
using System.Collections;
using UnityEngine;



namespace FinalYear {

    public class BoxMatchMenu : Menu<BoxMatchMenu> {
       [SerializeField] private float _playDelay = 0.01f;

        public override void OnBackPressed() {
            Debug.Log("Back");
            LevelLoader.LoadLevel(1);
            MainMenu.Open();
        }
    }
}