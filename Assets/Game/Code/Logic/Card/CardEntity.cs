using Game.Code.Infrastructure;
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
                _gameplay.CurrentStamina -= staminaCost;
                _gameplay.UpdateStamina();
                return true;
            }

            return false;
        }
    }
}