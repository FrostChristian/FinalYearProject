/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using UnityEngine;

namespace FinalYear.BoxMatch {

    [System.Serializable]
    public class GameAudioClip {
        public SoundHandler.Sounds sound;
        public AudioClip audioClip;
        /* 
        [Range(0f, 1f)]
        public float volume;
        */
    }
}
