using UnityEngine;

namespace Game.Code.Logic.UI.MainMenu
{
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] private ChangeScene sceneTransition;

        public void OnStartButtonClicked()
        {
            if (sceneTransition == null)
            {
                Debug.Log("No ChangeScene component assigned, searching in scene");
                sceneTransition = FindFirstObjectByType<ChangeScene>();
            }
            
            if (sceneTransition != null)
            {
                Debug.Log("Start button clicked, transitioning to Game scene");
                sceneTransition.TransitionToScene("Game");
            }
            else
            {
                Debug.LogWarning("No ChangeScene component found, loading scene directly");
                UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
            }
        }

        public void OnExitButtonClicked()
        {
            Debug.Log("Exit button clicked");
            Application.Quit();
        }
    }
}
