/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;

namespace FinalYear.BoxMatch {

    public class SoundHandler : MonoBehaviour { // handles play Sound, sound timers

        #region Variables
        private static GameObject oneShotGameObject;
        private static AudioSource oneShotAudioSource;
        public static bool _isMuted = false;
        #endregion

        public enum Sounds { // all possible sounds,  assign in inspector
            ButtonPressSoundOne,
            ButtonPressSoundTwo,
            ShuffleCards,
            WinSoundOne,
            Background
        }

        public static void ToggleAudio() {
            if (oneShotAudioSource.volume <= 0f) {
                oneShotAudioSource.volume = 1f;
                _isMuted = false;
                Debug.LogWarning("GUIHandler.cs OnMuteClicked() SOUND ON!");

            } else {
                oneShotAudioSource.volume = 0f;
                _isMuted = true;
                Debug.LogWarning("GUIHandler.cs OnMuteClicked() SOUND MUTED!");

            }
        }

        public static void PlaySound(Sounds sound) {
            if (oneShotGameObject == null) {
                oneShotGameObject = new GameObject("SoundSource");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }

        public static void PlayBackgroundSound(Sounds sound) {
            if (oneShotGameObject == null) {
                oneShotGameObject = new GameObject("SoundSource");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }

            oneShotAudioSource.clip = GetAudioClip(sound);
            oneShotAudioSource.loop = true;
            oneShotAudioSource.Play();
        }

        public static void PlayRandomSound(string containsString) {
            if (oneShotGameObject == null) {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.PlayOneShot(GetRandomAudioClip(containsString));
        }

        private static AudioClip GetAudioClip(Sounds sound) { // find requested audio clip
            foreach (GameAudioClip gameAudioClip in GameHandler.Instance.gameAudioClips) {
                if (gameAudioClip.sound == sound) {
                    return gameAudioClip.audioClip;
                }
            }
            return null;
        }

        private static AudioClip GetRandomAudioClip(string soundName) { // find random audio clip by string
            var tempList = new List<AudioClip>(); // hold all found sounds

            foreach (GameAudioClip gameAudioClip in GameHandler.Instance.gameAudioClips) {
                if (gameAudioClip.sound.ToString().Contains(soundName)) { // if gameaudiclip contains soundname
                    tempList.Add(gameAudioClip.audioClip); // add to temp list         
                }
            }

            if (tempList.Count != 0) { // if temp list is not empty
                return tempList[UnityEngine.Random.Range(0, tempList.Count)]; // get random value from list
            }
            return null;
        }
    }
}
