using Game.Code.Infrastructure;
using Game.Code.Logic.Enemies;
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
                var enemy = _gameplay.GetContainer.GetGameObjectByName<Enemy>(Constants.Enemy);
                if (cardType is CardType.Defence5 or CardType.Defence8 or CardType.Defence12)
                {
                    _gameplay.TakeCard(cardType, cardValueBase, cardValueAdditional);
                }
                else
                {
                    enemy.TakeCard(cardType, cardValueBase, cardValueAdditional);
                }

                _gameplay.UpdateEnemyHp();
                
                _gameplay.CurrentStamina -= staminaCost;
                _gameplay.UpdateStamina();
                
                return true;
            }

            return false;
        }
    }
}