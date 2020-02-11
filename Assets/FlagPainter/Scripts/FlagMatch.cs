/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections;
using UnityEngine;

namespace FlagPainter {

    //[System.Serializable]
    public class FlagMatch : MonoBehaviour { // PUT ON FLAG SPRITE; holds target color of sprite and checks if it is correctly filled

        #region Variables
        public Color targetColor;
        public bool isFilled;
        [SerializeField] private SpriteRenderer rend;
        #endregion

        #region Unity Methods
        void Start() {
            rend = GetComponent<SpriteRenderer>();
            StartCoroutine(TICK());
        }
        #endregion

        private void CheckIfColorFilled() { // cheks attached sprite for fill color
            if (!Mathf.Approximately(rend.color.r, targetColor.r)) {
                isFilled = false;
                return;
            }
            if (!Mathf.Approximately(rend.color.g, targetColor.g)) {
                isFilled = false;
                return;
            }
            if (!Mathf.Approximately(rend.color.b, targetColor.b)) {
                isFilled = false;
                return;
            }
            if (!Mathf.Approximately(rend.color.a, targetColor.a)) {
                isFilled = false;
                return;
            }
            Debug.Log("Color of " + rend.gameObject.name + " is filled! \n" + "Color F: " + rend.color + "\n" + "Color T: " + targetColor);
            isFilled = true;
        }

        private IEnumerator TICK() { // enum for performance
            while (true) { // always check loop!
                CheckIfColorFilled();
                //Debug.Log("TICK PART");
                yield return new WaitForSeconds(.1f);
            }
        }

    }
}
