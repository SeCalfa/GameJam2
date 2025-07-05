using UnityEngine;
using System.Collections.Generic;
using Game.Code.Infrastructure;

namespace Game.Code.Logic.Card
{
    public class CardBend : MonoBehaviour
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform centralAnchor;
        [SerializeField] private float cardSpacing = 1.5f;
        [SerializeField] private float bendAngle = 30f;
        [SerializeField] private float arrangeSpeed = 5f;
        [Space]
        [Header("Testing")]
        [SerializeField] private KeyCode addCardKey = KeyCode.Space;

        private readonly List<GameObject> _activeCards = new();
        private Gameplay _gameplay;
        private bool _isArranging;

        public int CardsCountToFull => 6 - _activeCards.Count;

        public void Construct(Gameplay gameplay)
        {
            _gameplay = gameplay;
        }

        private void Awake()
        {
            InitializeCentralAnchor();
            // SpawnInitialCards();
            CreateDropZone();
        }

        private void Update()
        {
            if (_isArranging)
            {
                ArrangeCards();
            }
            
            if (Input.GetKeyDown(addCardKey))
            {
                AddCard();
            }
        }

        private void InitializeCentralAnchor()
        {
            if (centralAnchor == null)
            {
                var anchorObject = new GameObject("CentralAnchor");
                anchorObject.transform.SetParent(transform);
                anchorObject.transform.localPosition = new Vector3(0, -3, 0);
                centralAnchor = anchorObject.transform;
            }
        }

        private void SpawnInitialCards()
        {
            if (cardPrefab == null)
            {
                return;
            }

            for (var i = 0; i < 6; i++)
            {
                var newCard = Instantiate(cardPrefab, transform);
                newCard.name = $"Card_{i + 1}";

                SetupCard(newCard, i);
            }

            _isArranging = true;
        }

        private void SetupCard(GameObject newCard, int index)
        {
            var cardMovement = newCard.GetComponent<CardMovement>();

            if (cardMovement == null)
            {
                cardMovement = newCard.AddComponent<CardMovement>();
            }

            _activeCards.Add(newCard);
        }

        public void AddCards(List<CardEntity> newCards)
        {
            for (var i = 0; i < newCards.Count; i++)
            {
                var newCard = Instantiate(newCards[i].gameObject, transform);
                newCard.GetComponent<CardEntity>().Construct(_gameplay);
                newCard.name = $"Card_{i + 1}";

                SetupCard(newCard, i);
            }
            
            _isArranging = true;
        }

        private void AddCard(GameObject card = null)
        {
            if (_activeCards.Count >= 6) return;

            GameObject newCard;
            if (card != null)
            {
                newCard = card;
                newCard.transform.SetParent(transform);
                
                var cardMovement = newCard.GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.SetStartPosition(centralAnchor.position);
                }
            }
            else if (cardPrefab != null)
            {
                newCard = Instantiate(cardPrefab, transform);
                newCard.name = $"Card_{_activeCards.Count + 1}";
            }
            else
            {
                return;
            }

            var cardComponent = newCard.GetComponent<CardMovement>();

            if (cardComponent == null)
                cardComponent = newCard.AddComponent<CardMovement>();

            _activeCards.Add(newCard);
            _isArranging = true;
        }

        public void RemoveCard(GameObject card)
        {
            if (_activeCards.Contains(card))
            {
                _activeCards.Remove(card);
                Destroy(card);
                _isArranging = true;
            }
        }

        public void RemoveCardAt(int index)
        {
            if (index >= 0 && index < _activeCards.Count)
            {
                var cardToRemove = _activeCards[index];
                RemoveCard(cardToRemove);
            }
        }

        private void ArrangeCards()
        {
            int cardCount = _activeCards.Count;
            if (cardCount == 0)
            {
                _isArranging = false;
                return;
            }

            var totalWidth = (cardCount - 1) * cardSpacing;
            var startX = -totalWidth / 2f;

            var allInPosition = true;

            for (var i = 0; i < cardCount; i++)
            {
                if (_activeCards[i] == null) continue;

                var xOffset = startX + (i * cardSpacing);
                var yOffset = Mathf.Abs(xOffset) * 0.1f;
                var targetPosition = centralAnchor.position + new Vector3(xOffset, -yOffset, i * 0.01f);

                var angleOffset = 0f;
                if (cardCount > 1)
                {
                    var normalizedPosition = (float)i / (cardCount - 1);
                    angleOffset = Mathf.Lerp(bendAngle, -bendAngle, normalizedPosition);
                }
                var targetRotation = new Vector3(0, 0, angleOffset);
                var targetQuaternion = Quaternion.Euler(targetRotation);

                var cardTransform = _activeCards[i].transform;
                cardTransform.position = Vector3.Lerp(cardTransform.position, targetPosition, Time.deltaTime * arrangeSpeed);
                cardTransform.rotation = Quaternion.Lerp(cardTransform.rotation, targetQuaternion, Time.deltaTime * arrangeSpeed);

                var cardMovement = _activeCards[i].GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.SetStartPosition(targetPosition);
                    cardMovement.SetStartRotation(targetQuaternion);
                }

                if (Vector3.Distance(cardTransform.position, targetPosition) > 0.01f ||
                    Quaternion.Angle(cardTransform.rotation, targetQuaternion) > 0.1f)
                {
                    allInPosition = false;
                }
            }

            if (allInPosition)
                _isArranging = false;
        }

        public void UseCard(int cardIndex)
        {
            RemoveCardAt(cardIndex);
        }

        public int GetCardCount()
        {
            return _activeCards.Count;
        }

        public GameObject GetCard(int index)
        {
            return index >= 0 && index < _activeCards.Count ? _activeCards[index] : null;
        }

        public void OnCardDestroyed(GameObject destroyedCard)
        {
            if (_activeCards.Contains(destroyedCard))
            {
                _activeCards.Remove(destroyedCard);
                
                if (_activeCards.Count < 6)
                {
                    _isArranging = true;
                }
            }
        }

        public void OnCardPulledOut(GameObject card)
        {
            if (_activeCards.Contains(card))
            {
                _activeCards.Remove(card);
                _isArranging = true;
            }
        }

        public void ReturnCardToHand(GameObject card)
        {
            
            if (!_activeCards.Contains(card) && _activeCards.Count < 6)
            {
                card.transform.SetParent(transform);
                
                _activeCards.Add(card);
                
                _isArranging = true;
            }
        }

        public Vector2 GetHandPosition()
        {
            return centralAnchor != null ? centralAnchor.position : transform.position;
        }

        private void CreateDropZone()
        {
            var dropZoneObject = new GameObject("CardDropZone");
            dropZoneObject.AddComponent<CardDropZone>();
        }
    }
}