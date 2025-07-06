using System.Collections;
using UnityEngine;

namespace Game.Code.Logic.UI
{
    public class ChangeScene : MonoBehaviour
    {
        [SerializeField] private CanvasGroup curtainCanvasGroup;
        [SerializeField] private float fadeSpeed = 2f;
    
        [SerializeField] private bool fadeInOnStart = true;

        void Awake()
        {
            if (curtainCanvasGroup != null)
            {
                if (fadeInOnStart) // fade in if set true
                {
                    curtainCanvasGroup.alpha = 1f;
                    curtainCanvasGroup.blocksRaycasts = true;
                    StartCoroutine(FadeIn());
                }
                else
                {
                    curtainCanvasGroup.alpha = 0f;
                    curtainCanvasGroup.blocksRaycasts = false;
                }
            }
        }

        public void TransitionToScene(string sceneName)
        {
            StartCoroutine(TransitionCoroutine(sceneName));
        }

        private IEnumerator TransitionCoroutine(string sceneName)
        {
            // Fade out (darken screen)
            yield return StartCoroutine(FadeOut());
        
            // Load new scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        private IEnumerator FadeOut()
        {
            curtainCanvasGroup.blocksRaycasts = true;
        
            while (curtainCanvasGroup.alpha < 1f)
            {
                curtainCanvasGroup.alpha += fadeSpeed * Time.deltaTime;
                yield return null;
            }
            curtainCanvasGroup.alpha = 1f;
        }

        private IEnumerator FadeIn()
        {
            while (curtainCanvasGroup.alpha > 0f)
            {
                curtainCanvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
            curtainCanvasGroup.alpha = 0f;
            curtainCanvasGroup.blocksRaycasts = false;
        }
    }
}
