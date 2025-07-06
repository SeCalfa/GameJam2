using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Code.Logic.Music;

namespace Game.Code.Logic.UI.Gameplay
{
    public class GameplayHud : MonoBehaviour
    {
        [SerializeField] private Button endTurnButton;
        [SerializeField] private TextMeshProUGUI enemyHpText;
        [SerializeField] private TextMeshProUGUI enemyAttackText;
        [SerializeField] private TextMeshProUGUI playerHpText;
        [SerializeField] private TextMeshProUGUI playerShieldText;
        [SerializeField] private Image[] staminaLogos;

        public event Action OnTurnEnd;

        public void ShowPlayerHp(int hp)
        {
            if (playerHpText != null)
                playerHpText.text = hp.ToString();
        }

        public void ShowPlayerShield(int shield)
        {
            if (playerShieldText != null)
                playerShieldText.text = shield.ToString();
        }
        
        public void ShowEnemyHp(int hp)
        {
            if (enemyHpText != null)
                enemyHpText.text = hp.ToString();
        }

        public void ShowEnemyAttack(int attack)
        {
            if (enemyAttackText != null)
                enemyAttackText.text = attack.ToString();
        }

        public void ShowStamina(int stamina)
        {
            if (staminaLogos != null)
            {
                for (var s = 0; s < staminaLogos.Length - stamina; s++)
                {
                    if (s + stamina < staminaLogos.Length && staminaLogos[s + stamina] != null)
                        staminaLogos[s + stamina].enabled = false;
                }
            }
        }

        public void ToggleStamina(bool toggle)
        {
            if (staminaLogos != null)
            {
                foreach (var stamina in staminaLogos)
                {
                    if (stamina != null)
                        stamina.enabled = toggle;
                }
            }
        }

        public void TurnEnd()
        {
            PlaySoundEffect.PlayEndTurn();
            OnTurnEnd?.Invoke();
        }
    }
}
