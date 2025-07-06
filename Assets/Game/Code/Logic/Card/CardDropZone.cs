using UnityEngine;

namespace Game.Code.Logic.Card
{
    public class CardDropZone : MonoBehaviour
    {
        private readonly Vector2 _zonePosition = new(0, 1.5f);
        private readonly Vector2 _zoneSize = new(10f, 6f);
        
        public static CardDropZone Instance;

        private void Awake()
        {
            Instance = this;
            
            transform.position = _zonePosition;
        }

        public bool IsCardInZone(CardMovement card)
        {
            return card != null && IsCardInZone(card.transform.position);
        }

        private bool IsCardInZone(Vector2 cardPosition)
        {
            var minX = _zonePosition.x - _zoneSize.x / 2f;
            var maxX = _zonePosition.x + _zoneSize.x / 2f;
            var minY = _zonePosition.y - _zoneSize.y / 2f;
            var maxY = _zonePosition.y + _zoneSize.y / 2f;

            return cardPosition.x >= minX && cardPosition.x <= maxX &&
                   cardPosition.y >= minY && cardPosition.y <= maxY;
        }
    }
}
