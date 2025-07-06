using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.Code.Logic.UI.Gameplay
{
    public class GameplayHud : MonoBehaviour
    {
        [SerializeField] private Button endTurnButton;
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Image[] staminaLogos;

        public event Action OnTurnEnd;

        public void ShowHp(int hp)
        {
            hpText.text = hp.ToString();
        }

        public void ShowStamina(int stamina)
        {
            for (var s = 0; s < staminaLogos.Length - stamina; s++)
            {
                staminaLogos[s + stamina].enabled = false;
            }
        }

        public void ToggleStamina(bool toggle)
        {
            foreach (var stamina in staminaLogos)
            {
                stamina.enabled = toggle;
            }
        }

        public void TurnEnd()
        {
            OnTurnEnd?.Invoke();
        }
    }
}
