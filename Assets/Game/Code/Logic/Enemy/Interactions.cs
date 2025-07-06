using System;
using UnityEngine;

namespace Game.Code.Logic.Enemy
{
    public enum DebuffType
    {
        None,
        ReduceDamage1,      
        ReduceDamage2Shield, 
        HealOnAttack        
    }

    public class Interactions : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        
        private int enemy1MaxHealth = 40;
        private int enemy1AttackDamage = 2;
        private int enemy2MaxHealth = 60;
        private int enemy2AttackDamage = 3;
        
        private int enemy3MaxHealth = 90;
        private int enemy3AttackDamage = 4;
        
        private int maxHealth;
        private int currentHealth;
        private int attackDamage;
        
        private DebuffType currentDebuff = DebuffType.None;
        private int playerShield = 0;

        void Start()
        {
            SetEnemyStats();
            currentHealth = maxHealth;
        }

        void Update()
        {

        }

        private void SetEnemyStats()
        {
            switch (enemyType)
            {
                case EnemyType.First:
                    maxHealth = enemy1MaxHealth;
                    attackDamage = enemy1AttackDamage;
                    break;
                case EnemyType.Second:
                    maxHealth = enemy2MaxHealth;
                    attackDamage = enemy2AttackDamage;
                    break;
                case EnemyType.Third:
                    maxHealth = enemy3MaxHealth;
                    attackDamage = enemy3AttackDamage;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DealDamage(int damage, bool toPlayer)
        {
            if (toPlayer)
            {
                DealDamageToPlayer(damage);
            }
            else
            {
                TakeDamage(damage);
            }
        }

        private void DealDamageToPlayer(int baseDamage)
        {
            int finalDamage = baseDamage;
            
            // Apply debuffs
            switch (currentDebuff)
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

        private void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
           
            if (currentHealth <= 0)
            {
                Die(); // change round here
            }
        }

        public void ApplyDebuff(DebuffType debuffType)
        {
            currentDebuff = debuffType;
        }

        private void ClearDebuff()
        {
            currentDebuff = DebuffType.None;
        }

        private void HealPlayer(int healAmount)
        {
            // Implement player healing logic here
            Debug.Log($"Player healed for {healAmount}");
        }

        private void Die()
        {
            Debug.Log("Enemy died");
            // Implement death logic here / change round
        }

        public void AttackPlayer()
        {
            DealDamageToPlayer(attackDamage);
        }
    }
}