using Game.Code.Infrastructure;
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

        public int GetCurrentAttack => _attackDamage - _debuffDamage;

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
            }
            
            DebuffDamageUse();
        }

        public void TakeCard(CardType cardType, int baseValue, int additionalValue)
        {
            if (cardType is CardType.Attack3)
            {
                TakeDamage(baseValue);
            }
            else if (cardType is CardType.Attack6)
            {
                TakeDamage(baseValue);
            }
            else if (cardType is CardType.Attack1014)
            {
                TakeDamage(_gameplay.CurrentStamina >= 8 ? additionalValue : baseValue);
            }
            else if (cardType is CardType.Debuff1)
            {
                _debuffDamage = baseValue;
            }
            else if (cardType is CardType.Debuff2)
            {
                _healDebuff = true;
            }
            else if (cardType is CardType.Debuff3)
            {
                _debuffDamage = baseValue;
            }
        }

        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
           
            if (_currentHealth <= 0)
            {
                Die();
            }
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
            _gameplay.RunRound(Round.Round2);
        }
    }
}