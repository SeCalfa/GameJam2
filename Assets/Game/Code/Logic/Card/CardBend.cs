using UnityEngine;
using System;
using System.Collections.Generic;
using Game.Code.Logic.Card;
using Game.Code.Infrastructure;

namespace Game.Code.Logic.Card
{
    public class CardBend : CardMovement
    {
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform centralAnchor;
        [SerializeField] private float cardSpacing = 1.5f;
        [SerializeField] private float bendAngle = 30f;
        [SerializeField] private float arrangeSpeed = 5f;
        [Space]
        [Header("Testing")]
        [SerializeField] private KeyCode addCardKey = KeyCode.Space;

        [SerializeField]

        private List<GameObject> activeCards = new List<GameObject>();
        private bool isArranging = false;

        protected override void Awake()
        {
           // // Debug.Log("CardBend: Awake called");
            base.Awake();
            InitializeCentralAnchor();
        }

        private void Start()
        {
          //  // Debug.Log("CardBend: Start called");
            SpawnInitialCards();
        }

        protected override void Update()
        {
            base.Update();

            if (isArranging)
            {
                ArrangeCards();
            }
            
            if (Input.GetKeyDown(addCardKey))
            {
              //  // Debug.Log("CardBend: Add card key pressed");
                AddCard();
            }
        }

        private void InitializeCentralAnchor()
        {
            if (centralAnchor == null)
            {
               // // Debug.Log("CardBend: Creating central anchor");
                GameObject anchorObject = new GameObject("CentralAnchor");
                anchorObject.transform.SetParent(transform);
                anchorObject.transform.localPosition = new Vector3(0, -3, 0);
                centralAnchor = anchorObject.transform;
            }
            else
            {
                // // // Debug.Log("CardBend: Central anchor already exists");
            }
        }

        private void SpawnInitialCards()
        {
           // // // Debug.Log("CardBend: SpawnInitialCards called");

            if (cardPrefab == null)
            {
                // // Debug.LogError("CardBend: Card prefab is not assigned! Please assign a card prefab in the inspector.");
                return;
            }

           // // // Debug.Log($"CardBend: Using prefab: {cardPrefab.name}");

            for (int i = 0; i < 6; i++)
            {
              //  // Debug.Log($"CardBend: Creating card {i + 1}");
                GameObject newCard = Instantiate(cardPrefab, transform);
                newCard.name = $"Card_{i + 1}";

                SetupCard(newCard, i);
            }

           // // Debug.Log($"CardBend: Total cards spawned: {activeCards.Count}");
            isArranging = true;
        }

        private void SetupCard(GameObject newCard, int index)
        {
            CardMovement cardMovement = newCard.GetComponent<CardMovement>();

            if (cardMovement == null)
            {
               // // Debug.Log($"CardBend: Adding CardMovement to card {index + 1}");
                cardMovement = newCard.AddComponent<CardMovement>();
            }

            activeCards.Add(newCard);
            //// Debug.Log($"CardBend: Card {index + 1} added to activeCards list");
        }

        public void AddCard(GameObject card = null)
        {
            if (activeCards.Count >= 6) return;

            GameObject newCard;
            if (card != null)
            {
                newCard = card;
                newCard.transform.SetParent(transform);
                
                // Reset card state when adding to hand
                CardMovement cardMovement = newCard.GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    // Reset any "on table" status
                    cardMovement.SetStartPosition(centralAnchor.position);
                }
            }
            else if (cardPrefab != null)
            {
                newCard = Instantiate(cardPrefab, transform);
                newCard.name = $"Card_{activeCards.Count + 1}";
            }
            else
            {
                //// Debug.LogError("CardBend: No card prefab assigned! Cannot add new card.");
                return;
            }

            CardMovement cardComponent = newCard.GetComponent<CardMovement>();

            if (cardComponent == null)
                cardComponent = newCard.AddComponent<CardMovement>();

            activeCards.Add(newCard);
            //// Debug.Log($"CardBend: Card added to hand. Total cards: {activeCards.Count}");
            isArranging = true;
        }

        public void RemoveCard(GameObject card)
        {
            if (activeCards.Contains(card))
            {
                activeCards.Remove(card);
                Destroy(card);
                isArranging = true;
            }
        }

        public void RemoveCardAt(int index)
        {
            if (index >= 0 && index < activeCards.Count)
            {
                GameObject cardToRemove = activeCards[index];
                RemoveCard(cardToRemove);
            }
        }

        private void ArrangeCards()
        {
            int cardCount = activeCards.Count;
            if (cardCount == 0)
            {
                //// Debug.Log("CardBend: No cards to arrange");
                isArranging = false;
                return;
            }

            //// Debug.Log($"CardBend: Arranging {cardCount} cards");

            float totalWidth = (cardCount - 1) * cardSpacing;
            float startX = -totalWidth / 2f;

            bool allInPosition = true;

            for (int i = 0; i < cardCount; i++)
            {
                if (activeCards[i] == null) continue;

                float xOffset = startX + (i * cardSpacing);
                float yOffset = Mathf.Abs(xOffset) * 0.1f;
                Vector3 targetPosition = centralAnchor.position + new Vector3(xOffset, -yOffset, i * 0.01f);

                float angleOffset = 0f;
                if (cardCount > 1)
                {
                    float normalizedPosition = (float)i / (cardCount - 1);
                    angleOffset = Mathf.Lerp(bendAngle, -bendAngle, normalizedPosition);
                }
                Vector3 targetRotation = new Vector3(0, 0, angleOffset);
                Quaternion targetQuaternion = Quaternion.Euler(targetRotation);

                Transform cardTransform = activeCards[i].transform;
                cardTransform.position = Vector3.Lerp(cardTransform.position, targetPosition, Time.deltaTime * arrangeSpeed);
                cardTransform.rotation = Quaternion.Lerp(cardTransform.rotation, targetQuaternion, Time.deltaTime * arrangeSpeed);

                CardMovement cardMovement = activeCards[i].GetComponent<CardMovement>();
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
                isArranging = false;
        }


        public void UseCard(int cardIndex)
        {
            RemoveCardAt(cardIndex);
        }

        public int GetCardCount()
        {
            return activeCards.Count;
        }

        public GameObject GetCard(int index)
        {
            return index >= 0 && index < activeCards.Count ? activeCards[index] : null;
        }

        public void OnCardDestroyed(GameObject destroyedCard)
        {
            // Debug.Log($"CardBend: Card destroyed - {destroyedCard.name}");
            
            if (activeCards.Contains(destroyedCard))
            {
                activeCards.Remove(destroyedCard);
                // Debug.Log($"CardBend: Removed card from list. Remaining cards: {activeCards.Count}");
                
                if (activeCards.Count < 6)
                {
                    // Debug.Log("CardBend: Starting rearrangement");
                    isArranging = true;
                }
            }
        }

        public void OnCardPulledOut(GameObject card)
        {
            // Debug.Log($"CardBend: Card pulled out - {card.name}");
            
            if (activeCards.Contains(card))
            {
                activeCards.Remove(card);
                // Debug.Log($"CardBend: Removed card from hand. Remaining cards: {activeCards.Count}");
                isArranging = true;
            }
        }

        public void ReturnCardToHand(GameObject card)
        {
            // Debug.Log($"CardBend: Card returned to hand - {card.name}");
            
            if (!activeCards.Contains(card) && activeCards.Count < 6)
            {
                // Set proper parent
                card.transform.SetParent(transform);
                
                // Add to activeCards list
                activeCards.Add(card);
                // Debug.Log($"CardBend: Card added back to hand. Total cards: {activeCards.Count}");
                
                // Trigger rearrangement
                isArranging = true;
            }
            else if (activeCards.Contains(card))
            {
                // Debug.Log($"CardBend: Card {card.name} is already in hand");
            }
            else
            {
                // Debug.Log($"CardBend: Cannot return card - hand is full ({activeCards.Count}/6)");
            }
        }

        public Vector2 GetHandPosition()
        {
            return centralAnchor != null ? centralAnchor.position : transform.position;
        }
    }
}