using System.Collections;
using UnityEngine;
using WiSJoy.DesignPattern;

namespace WiSdom.Core
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        public void LoadSceneWithTransition(string sceneName)
        {
            StartCoroutine(LoadSceneWithTransitionRoutine(sceneName));
        }

        private IEnumerator LoadSceneWithTransitionRoutine(string sceneName)
        {
            // Start the fade-out effect
            SceneTransition.I.FadeOut();
            yield return new WaitForSeconds(SceneTransition.I.fadeDuration);

            // Load the new scene asynchronously
            yield return StartCoroutine(LoadSceneAsync(sceneName));

            // Once the scene is loaded, start the fade-in effect
            SceneTransition.I.FadeIn();
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        public void UnloadScene(string sceneName)
        {
            StartCoroutine(UnloadSceneAsync(sceneName));
        }

        private IEnumerator UnloadSceneAsync(string sceneName)
        {
            AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }
    }
}
