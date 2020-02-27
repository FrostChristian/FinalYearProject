/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;
using UnityEngine.UI;

namespace FinalYear.FlagPainter {

    public class ColorButton : MonoBehaviour {

        #region Variables
        public Color myColor = new Color(0f, 0f, 0f, 1f);
        private Button thisButton;
        #endregion

        #region Unity Methods
        private void Awake() {
            thisButton = GetComponent<Button>();
        }

        void Start() {
            thisButton.GetComponent<Image>().color = myColor;
            thisButton.onClick.AddListener(OnThisButtonClick);
        }
        #endregion

        void OnThisButtonClick() {
            GameHandler.Instance.UpdadeActiveColor(myColor);
            GUIHandler.Instance.paintBucketImage.color = myColor;
            //if (GameHandler.CompareColors(myColor, Color.black)) { // useless function atm
            //    Debug.Log("Im black");
            //    GUIHandler.Instance.paintBucketHighlightImage.color = Color.white;
            //}
            SoundHandler.PlayRandomSound("ColorFill");
        }
    }
}
