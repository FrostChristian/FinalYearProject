/*
* Copyright (c) Christian Frost
* christian.dennis.frost@gmail.com
*/

using System.Collections;
using UnityEngine;

public class RotationFade : MonoBehaviour {

    private void OnEnable() {
        StartCoroutine(FadeCanvas(GetComponent<CanvasGroup>(), 0f, 1f, 0.3f));
    }

    void Update() {
        transform.Rotate(Vector3.forward * 20 * Time.deltaTime);
    }

    public static IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration) {
        var startTime = Time.time;
        var endTime = Time.time + duration;
        var elapsedTime = 0f;


        canvas.alpha = startAlpha;// set the canvas to the start alpha 

        while (Time.time <= endTime) {// loop repeatedly until the previously calculated end time
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            if (startAlpha > endAlpha) // if we are fading out/down 
            {
                canvas.alpha = startAlpha - percentage; // calculate the new alpha
            } else // if we are fading in/up
              {
                canvas.alpha = startAlpha + percentage; // calculate the new alpha
            }

            yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
        }
        canvas.alpha = endAlpha; // force the alpha to the end alpha before finishing 
    }
}
