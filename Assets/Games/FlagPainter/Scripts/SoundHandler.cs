/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace FinalYear.FlagPainter {

    public class SoundHandler : MonoBehaviour { // handles play Sound, sound timers

        #region Variables
        private static GameObject oneShotGameObject;
        private static AudioSource oneShotAudioSource;
        #endregion

        public enum Sounds { // all possible sounds,  assign in inspector
            ButtonPressSoundOne,
            ButtonPressSoundTwo,
            ButtonPressSoundThree,
            ColorFillSoundOne,
            ColorFillSoundTwo,
            ColorFillSoundThree,
            WinSoundOne,
            EraserSoundOne,
        }

        public static void PlaySound(Sounds sound) {
                if (oneShotGameObject == null) {
                    oneShotGameObject = new GameObject("SoundSource");
                    oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
                }
                oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
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
            Debug.LogError("Sound " + sound + " not found!");
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

            Debug.LogError("Sound " + soundName + " not found!");
            return null;
        }
       
    }
}
