using UnityEngine;

namespace Game.Code.Logic.UI.MainMenu
{
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] private ChangeScene sceneTransition;

        public void OnStartButtonClicked()
        {
            Debug.Log("Start button clicked");
            if (sceneTransition == null)
            {
                sceneTransition = FindFirstObjectByType<ChangeScene>();
            }
            
            if (sceneTransition != null)
            {
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
