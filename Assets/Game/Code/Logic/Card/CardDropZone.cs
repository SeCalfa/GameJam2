using UnityEngine;
using System.Collections.Generic;

namespace Game.Code.Logic.Card
{
    public class CardDropZone : MonoBehaviour
    {
        private Vector2 zonePosition = new Vector2(0, 2.5f);
        private Vector2 zoneSize = new Vector2(11.5f, 5f);

        private void Start()
        {
            transform.position = zonePosition;
        }

        public bool IsCardInZone(Vector2 cardPosition)
        {
            float minX = zonePosition.x - zoneSize.x / 2f;
            float maxX = zonePosition.x + zoneSize.x / 2f;
            float minY = zonePosition.y - zoneSize.y / 2f;
            float maxY = zonePosition.y + zoneSize.y / 2f;

            return cardPosition.x >= minX && cardPosition.x <= maxX &&
                   cardPosition.y >= minY && cardPosition.y <= maxY;
        }

        public bool IsCardInZone(CardMovement card)
        {
            if (card == null) return false;
            return IsCardInZone(card.transform.position);
        }

        public bool OnCardReleased(CardMovement card)
        {
            if (IsCardInZone(card))
            {
                card.DestroyCard();
                return true; 
            }
            return false; 
        }
    }
}
