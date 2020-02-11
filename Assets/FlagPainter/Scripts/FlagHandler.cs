﻿/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;

namespace FlagPainter {

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
        #endregion

        #region Unity Methods
        void Awake() {
            if (_instance != null) { // singleton 
                Destroy(this);
            } else {
                _instance = this;
            }
            SpawnFlag(_currentFlagIndex);
        }
        #endregion

        private void SpawnFlag(int index) {
            if (_currentFlag != null) {
                Destroy(_currentFlag);        
            }
            _currentFlag = Instantiate(_Flags[index], _flagSpawn.transform);
        }

        public void ClampFlagIndex() { //wrap around list
            if (_Flags.Count == 0) {
                Debug.LogWarning("ClampIndex: missing difficulty setup!");
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
            Debug.Log("Increment Flag Index");
            _currentFlagIndex++;
            ClampFlagIndex();
            SpawnFlag(_currentFlagIndex);
        }

        public void PreviousFlag() {
            Debug.Log("Decrement Flag Index");
            _currentFlagIndex--;
            ClampFlagIndex();
            SpawnFlag(_currentFlagIndex);
        }

        public static GameObject GetCurrentlyActiveFlagGameObject() {
            return _currentFlag;
        }
    }
}
