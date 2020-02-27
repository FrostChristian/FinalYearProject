/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalYear.FlagPainter {

    public class GameHandler : MonoBehaviour {

        #region Variables
        private static GameHandler _instance;
        public static GameHandler Instance { get => _instance; set => _instance = value; }

        [SerializeField] private static Color _defaultColor = new Color(1f, 1f, 1f, 1f); // set white as default color
        public static Color DefaultColor { get => _defaultColor; }

        [SerializeField] private static Color _activeColor; // store active color here
        public static Color ActiveColor { get => _activeColor; set => _activeColor = value; }
        [Header("Score")]
        [SerializeField] private int _score = 0;
        public int Score { get => _score; }

        private bool _isTouchInputActive; // checks if user currently provides input

        [Header("Sound")]
        public GameAudioClip[] gameAudioClips; // stores all audio clips; assigned in inspector!

        [Header("Particle FX")]
        public GameObject paintEffectPrefabPS;
        [Space]
        public GameObject bucketEffectPrefabPS;
        public Transform bucketEffectSpawn; // position for PS instanciation
        #endregion

        #region Unity Methods
        private void Awake() {
            if (_instance != null) { // singleton Gamehandler
                Destroy(this);
            } else {
                _instance = this;
            }
            ResetActiveColor(); // set active to default color

        }
        private void Start() {

            if (SceneManager.GetActiveScene().name == "FlagPainter") {
                FlagPainterMenu.Open();
                Debug.Log("GameManager Awake(): Entered Game through FlagPainter");
            }
        }

        private void Update() {
            HandleUserInput(); // listen for input
        }
        #endregion Unity Methods

        /// ---------------------------------- Colors ---------------------------------- ///
        public void UpdadeActiveColor(Color color) {
            _activeColor = color;
            InstantiateParticleSystem(color, bucketEffectPrefabPS, bucketEffectSpawn, bucketEffectSpawn.position, -90f); // spawn particles on bucket
        }

        public void ResetActiveColor() {
            _activeColor = _defaultColor;
        }

        public static void SetSpriteRendererColor(SpriteRenderer rend, Color color) {
            rend.color = color;
        }

        public Color ModifyColorRGB(Color color, float modifier) {
            float tempR = color.r + modifier; // add modifier to color and store in temp var
            float tempG = color.g + modifier;
            float tempB = color.b + modifier;

            Mathf.Clamp(tempR, 0f, 1f); // clamp to 0 1
            Mathf.Clamp(tempG, 0f, 1f);
            Mathf.Clamp(tempB, 0f, 1f);

            return new Color(tempR, tempG, tempB);// use temp vars for new color for PS
        }

        public static bool CompareColors(Color color, Color targetColor) { // compares two colors and returns true if match
            if (!Mathf.Approximately(color.r, targetColor.r)) {
                return false;
            }
            if (!Mathf.Approximately(color.g, targetColor.g)) {
                return false;
            }
            if (!Mathf.Approximately(color.b, targetColor.b)) {
                return false;
            }
            if (!Mathf.Approximately(color.a, targetColor.a)) {
                return false;
            }
            return true;
        }
        /// ---------------------------------- +Colors ---------------------------------- ///
        /// ---------------------------------- User Input ---------------------------------- ///
        private void HandleUserInput() {

            if (Input.GetKeyDown(KeyCode.Escape)) {
                GUIHandler.OnQuitClick_Static();
            }

            if (Input.GetAxisRaw("Fire1") != 0) { // check for User input on "Fire1" Input
                if (!_isTouchInputActive) { // make sure this will only be called once
                    _isTouchInputActive = true;
                    //call click event here
                    OnUserClick();
                }
            } else {
                _isTouchInputActive = false;
            }

            if (Input.GetAxisRaw("Fire1") > 0) {
                //call hold event here
                Debug.Log("IM HOLDING");
            }
        }

        private void OnUserClick() {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Drawing"));  // Input.GetTouch(0).position == for the app
            if (hit.collider != null) { // if we hit something
                SetSpriteRendererColor(hit.collider.gameObject.GetComponent<SpriteRenderer>(), _activeColor); // get the renderer and change its color
                SoundHandler.PlayRandomSound("ColorFill"); // Play sound
                InstantiateParticleSystem(_activeColor, paintEffectPrefabPS, hit.collider.gameObject.transform.parent, hit.point, 0f);
            }
        }
        /// ---------------------------------- +User Input ---------------------------------- ///

        public IEnumerator HandleOnFlagComplete() { // WInning!
            yield return new WaitForSeconds(1f);
            while (!Flag.isFlagCompletelyFilled) { // always check loop!
                //Debug.Log("TICK GH");
                yield return new WaitForSeconds(.1f);
            }
            AddScore(50);

            FlagHandler.Instance.NextFlag(); // spawn next flag
            SoundHandler.PlaySound(SoundHandler.Sounds.WinSoundOne); //Play Sound
        }

        public void AddScore(int score) {
            _score += score;
        }

        private void InstantiateParticleSystem(Color color, GameObject prefab, Transform spwanTransform, Vector3 position, float xRota) {
            var go = Instantiate(prefab, position, Quaternion.Euler(xRota, 0f, 0f), spwanTransform);
            foreach (ParticleSystem ptS in go.GetComponentsInChildren<ParticleSystem>()) {
                ParticleSystem.MainModule main = ptS.main; // temp var for the PS.main
                main.startColor = ModifyColorRGB(color, 0.2f); // change Color here and modify it so that its a bit different to active color
            };
            Destroy(go, 1.5f);
        }

    }
}
