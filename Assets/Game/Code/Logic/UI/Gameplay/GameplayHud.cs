using UnityEngine;
using UnityEngine.UI;

namespace Game.Code.Logic.UI.Gameplay
{
    public class GameplayHud : MonoBehaviour
    {
        [SerializeField] private Image[] staminaLogos;

        public void ShowStamina(int stamina)
        {
            for (var s = 0; s < staminaLogos.Length - stamina; s++)
            {
                staminaLogos[s + stamina].enabled = false;
            }
        }

        public void ResetStamina()
        {
            foreach (var stamina in staminaLogos)
            {
                stamina.enabled = true;
            }
        }
    }
}
