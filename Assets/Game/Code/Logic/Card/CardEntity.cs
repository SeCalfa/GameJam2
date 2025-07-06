using Game.Code.Infrastructure;
using Game.Code.Logic.Enemies;
using Game.Code.Logic.Music;
using UnityEngine;

namespace Game.Code.Logic.Card
{
    public class CardEntity : CardMovement
    {
        [SerializeField] private CardType cardType;
        [SerializeField] private int cardValueBase;
        [SerializeField] private int cardValueAdditional;
        [SerializeField][Range(1, 3)] private int staminaCost = 1;
        
        private Gameplay _gameplay;
        
        public static int BuffValue;

        public CardType GetCardType => cardType;
        public int GetCardValue => cardValueBase;
        public int GetCardValueAdditional => cardValueAdditional;
        public int GetStaminaCost => staminaCost;
        
        public void Construct(Gameplay gameplay)
        {
            _gameplay = gameplay;
        }
        
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override bool UseCard()
        {
            if (_gameplay.CurrentStamina >= staminaCost)
            {
                var add = BuffValue > 0 ? 1 : 0;
                var enemy = _gameplay.GetContainer.GetGameObjectByName<Enemy>(Constants.Enemy);
                
                UseBuff();

                if (cardType is CardType.Defence5 or CardType.Defence8 or CardType.Defence12
                    or CardType.Buff1 or CardType.Buff2 or CardType.Buff3 or CardType.Debuff3)
                {
                    _gameplay.TakeCard(cardType, cardValueBase + add, cardValueAdditional + add);

                    if (cardType is CardType.Debuff3)
                    {
                        Enemy.DebuffDamage2 = 2;
                    }
                }
                else
                {
                    var isDead = enemy.TakeCard(cardType, cardValueBase + add, cardValueAdditional + add);
                    _gameplay.UpdateEnemyAttack();
                    if (isDead)
                    {
                        return true;
                    }
                }

                _gameplay.UpdateEnemyHp();
                
                _gameplay.CurrentStamina -= staminaCost;
                _gameplay.UpdateStamina();
                _gameplay.UpdateEnemyAttack();
                _gameplay.UpdatePlayerArmor();
                
                
                PlaySoundEffect.PlayCardDrop();

                return true;
            }

            return false;
        }

        private void UseBuff()
        {
            BuffValue -= 1;
            if (BuffValue < 0)
            {
                BuffValue = 0;
            }
        }
    }
}