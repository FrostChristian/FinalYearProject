using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalYear.BoxMatch {
    public class DialogCardAnimation : MonoBehaviour {

        private static DialogCardAnimation _instance;
        public static DialogCardAnimation Instance { get => _instance; set => _instance = value; }

        [Header("Animation")]
        private string[] animationClips = { "LeftBox", "RightBox" };
        public RuntimeAnimatorController animController; // assign in inspector

        public event EventHandler OnCardAnimFinished;

        private void Awake() {
            if (Instance != null) {
                Destroy(this);
            } else {
                Instance = this;
            }
        }

        private void Start() {
            GUIHandler.Instance.OnDialogPanelOpen += AnimateCard;
        }

        public void AnimateCard(object sender, EventArgs e) {
            //Duplicate card in position twitch and put in list
            for (int i = 0; i < animationClips.Length; i++) {
            var tempDialog = Instantiate(GUIHandler.Instance.dialogCard, gameObject.transform);
            Animator anim = tempDialog.AddComponent<Animator>();
            anim.runtimeAnimatorController = animController;
            anim.SetTrigger(animationClips[i]);
            Destroy(tempDialog, 2f);
            }

            GUIHandler.Instance.dialogCard.SetActive(false);
        }
    }
}
