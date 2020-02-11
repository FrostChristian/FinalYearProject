/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlagPainter {

    public class Flag : MonoBehaviour { // PUT ON FLAG PREFAB // handles flag parts and checks if all parts are filled in

        #region Variables
        private static Flag _instance;
        public string _flagName = "ENTER FLAG NAME"; // for display in GUI
        [HideInInspector] public List<FlagMatch> FlagMatches = new List<FlagMatch>();
        public static bool isFlagCompletelyFilled;
        #endregion

        #region Unity Methods
        void Awake() {
            _instance = this;
            foreach (FlagMatch matchitem in GetComponentsInChildren<FlagMatch>()) {
                FlagMatches.Add(matchitem);
            }
            isFlagCompletelyFilled = false;
        }

        void Start() {
            StartCoroutine(TICK());
            StartCoroutine(GameHandler.Instance.HandleOnFlagComplete());
        }
        #endregion

        private void CheckIfFlagFilled() { // checks attached flag components for fill color
            int iter = 0;
            for (int i = 0; i < FlagMatches.Count; i++) {
                if (FlagMatches[i].isFilled == true) { // if any parts are filled ++iter
                    iter++;
                }
            }
            if (iter == FlagMatches.Count) { //if iter is same as all flag parts the flag is filled in
                isFlagCompletelyFilled = true;
                Debug.Log("All sprites are the right color!");
            } else {
                isFlagCompletelyFilled = false;
            }
        }

        private IEnumerator TICK() { // enum for performance
            while (true) { // always check loop!
                CheckIfFlagFilled();
                //Debug.Log("TICK FLAG");
                yield return new WaitForSeconds(.1f);
            }
        }

        public static Flag GetActiveFlag() {
            return _instance;
        }

    }


}
