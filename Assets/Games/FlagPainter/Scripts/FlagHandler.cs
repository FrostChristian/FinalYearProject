/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FinalYear.FlagPainter {

    public class FlagHandler : MonoBehaviour {

        #region Variables
        private static FlagHandler _instance;
        public static FlagHandler Instance { get => _instance; set => _instance = value; }

        [SerializeField] private List<GameObject> _Flags = new List<GameObject>(); //list of all flags
        [Space]
        [SerializeField] private int _currentFlagIndex = 0; // current flag index from _Flags
        [SerializeField] private static GameObject _currentFlag = default; //current flag prefab
        [Space]
        [SerializeField] private GameObject _flagSpawn = default; //pos for instantiation
        public Sprite incorrectSprite;
        public Sprite correctSprite;
        #endregion

        #region Unity Methods
        void Awake() {
            if (_instance != null) { // singleton 
                Destroy(this);
            } else {
                _instance = this;
            }
            //SpawnFlag(_currentFlagIndex);
        }
        #endregion

        public void SpawnFlag(int index) {
            if (_currentFlag != null) {
                Destroy(_currentFlag);
            }
            _currentFlag = Instantiate(_Flags[index], _flagSpawn.transform);
            GUIHandler.UpdateGUI_Static();
        }

        public void ClampFlagIndex() { //wrap around list
            if (_Flags.Count == 0) {
                return;
            }

            if (_currentFlagIndex >= _Flags.Count) {
                _currentFlagIndex = 0;
            }

            if (_currentFlagIndex < 0) {
                _currentFlagIndex = _Flags.Count - 1;
            }
        }

        public void NextFlag() {
            _currentFlagIndex++;
            ClampFlagIndex();
            SpawnFlag(_currentFlagIndex);
        }

        public void PreviousFlag() {
            _currentFlagIndex--;
            ClampFlagIndex();
            SpawnFlag(_currentFlagIndex);
        }

        public static GameObject GetCurrentlyActiveFlagGameObject() {
            return _currentFlag;
        }
    }
}
