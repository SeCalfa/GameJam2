using Game.Code.Infrastructure;
using Game.Code.Logic.UI;
using UnityEngine;

namespace Game.Code.Logic.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;

        private const int Enemy1MaxHealth = 40;
        private const int Enemy1AttackDamage = 2;
        
        private const int Enemy2MaxHealth = 60;
        private const int Enemy2AttackDamage = 3;

        private const int Enemy3MaxHealth = 90;
        private const int Enemy3AttackDamage = 4;

        private int _attackDamage, _maxHealth, _currentHealth;
        private Gameplay _gameplay;
        
        private static int _debuffDamage;
        private static bool _healDebuff;
        
        public int GetCurrentHealth => _currentHealth;

        public void Construct(Gameplay gameplay)
        {
            _gameplay = gameplay;
        }

        void Awake()
        {
            Init();
        }

        private void Init()
        {
            _attackDamage = enemyType switch
            {
                EnemyType.First => Enemy1AttackDamage,
                EnemyType.Second => Enemy2AttackDamage,
                _ => Enemy3AttackDamage
            };

            _maxHealth = enemyType switch
            {
                EnemyType.First => Enemy1MaxHealth,
                EnemyType.Second => Enemy2MaxHealth,
                _ => Enemy3MaxHealth
            };

            _currentHealth = _maxHealth;
        }

        public void DoAction(ref int currentHealth, ref int currentArmor)
        {
            if (_healDebuff)
            {
                currentHealth += _attackDamage - _debuffDamage;
                if (currentHealth > 30)
                {
                    currentHealth = 30;
                }
                
                _healDebuff = false;
                return;
            }
            
            currentArmor -= _attackDamage - _debuffDamage;
            if (currentArmor < 0)
            {
                currentHealth += currentArmor;
                currentArmor = 0;

                if (currentHealth <= 0)
                {
                    ChangeScene.Instance.TransitionToScene("EndGame");
                }
            }
            
            DebuffDamageUse();
        }

        public bool TakeCard(CardType cardType, int baseValue, int additionalValue)
        {
            if (cardType is CardType.Attack3)
            {
                return TakeDamage(baseValue);
            }

            if (cardType is CardType.Attack6)
            {
                return TakeDamage(baseValue);
            }

            if (cardType is CardType.Attack1014)
            {
                return TakeDamage(_gameplay.CurrentStamina >= 8 ? additionalValue : baseValue);
            }

            if (cardType is CardType.Debuff1)
            {
                _debuffDamage = baseValue;
                return false;
            }

            if (cardType is CardType.Debuff2)
            {
                _healDebuff = true;
                return false;
            }

            if (cardType is CardType.Debuff3)
            {
                _debuffDamage = baseValue;
                return false;
            }

            return false;
        }

        private bool TakeDamage(int damage)
        {
            _currentHealth -= damage;
           
            if (_currentHealth <= 0)
            {
                Die();
                return true;
            }
            
            return false;
        }

        private void DebuffDamageUse()
        {
            _debuffDamage -= 1;
            if (_debuffDamage < 0)
            {
                _debuffDamage = 0;
            }
        }

        private void Die()
        {
            _gameplay.FightFinish();
            
            if (enemyType is EnemyType.First)
            {
                _gameplay.RunRound(Round.Round2);
            }
            else if (enemyType is EnemyType.Second)
            {
                _gameplay.RunRound(Round.Round3);
            }
            else if (enemyType is EnemyType.Third)
            {
                ChangeScene.Instance.TransitionToScene("WinGame");
            }
        }
    }
}