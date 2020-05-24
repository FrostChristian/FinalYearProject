using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FinalYear {

    public class LevelLoaderShort : MonoBehaviour {


        public void LoadLevelAsync(int levelIndex) { // start the coroutine to load asynchronously and generate a progress bar prefab
            StartCoroutine(LoadLevelAsyncRoutine(levelIndex));
        }

        private IEnumerator LoadLevelAsyncRoutine(int levelIndex) { // load a level asynchronously and update the load progress bar
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone) {
                yield return null;
            }

            yield return null;
            asyncLoad.allowSceneActivation = true;
        }
    }
}
