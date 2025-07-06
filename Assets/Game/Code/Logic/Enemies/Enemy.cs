using UnityEngine;

namespace Game.Code.Logic.Enemies
{
    public enum DebuffType
    {
        None,
        ReduceDamage1,      
        ReduceDamage2Shield, 
        HealOnAttack        
    }

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
        
        private DebuffType _currentDebuff = DebuffType.None;
        private int playerShield;

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

        public int Attack()
        {
            return _attackDamage;
        }

        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
           
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void DealDamageToPlayer(int baseDamage)
        {
            int finalDamage = baseDamage;
            
            // Apply debuffs
            switch (_currentDebuff)
            {
                case DebuffType.ReduceDamage1:
                    finalDamage = Mathf.Max(0, baseDamage - 1);
                    break;
                    
                case DebuffType.ReduceDamage2Shield:
                    finalDamage = Mathf.Max(0, baseDamage - 2);
                    playerShield += 2;
                    break;
                    
                case DebuffType.HealOnAttack:
                    HealPlayer(baseDamage);
                    ClearDebuff();
                    return;
            }
            
            // Apply damage to player (implement player damage system here)
            Debug.Log($"Dealing {finalDamage} damage to player");
            
            ClearDebuff();
        }

        private void ClearDebuff()
        {
            _currentDebuff = DebuffType.None;
        }

        private void HealPlayer(int healAmount)
        {
            // Implement player healing logic here
            Debug.Log($"Player healed for {healAmount}");
        }

        private void Die()
        {
            Debug.Log("Enemy died");
        }
    }
}