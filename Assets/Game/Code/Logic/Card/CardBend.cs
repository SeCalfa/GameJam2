using UnityEngine;
using System.Collections.Generic;


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

        [SerializeField]

        private List<GameObject> activeCards = new List<GameObject>();
        private bool isArranging = false;

        private void Awake()
        {
            // base.Awake();
            InitializeCentralAnchor();
        }

        private void Start()
        {
            SpawnInitialCards();
            CreateDropZone();
        }

        private void Update()
        {
            // base.Update();

            if (isArranging)
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
                GameObject anchorObject = new GameObject("CentralAnchor");
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


            for (int i = 0; i < 6; i++)
            {
                GameObject newCard = Instantiate(cardPrefab, transform);
                newCard.name = $"Card_{i + 1}";

                SetupCard(newCard, i);
            }

            isArranging = true;
        }

        private void SetupCard(GameObject newCard, int index)
        {
            CardMovement cardMovement = newCard.GetComponent<CardMovement>();

            if (cardMovement == null)
            {
                cardMovement = newCard.AddComponent<CardMovement>();
            }

            activeCards.Add(newCard);
        }

        public void AddCard(GameObject card = null)
        {
            if (activeCards.Count >= 6) return;

            GameObject newCard;
            if (card != null)
            {
                newCard = card;
                newCard.transform.SetParent(transform);
                
                CardMovement cardMovement = newCard.GetComponent<CardMovement>();
                if (cardMovement != null)
                {
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
                return;
            }

            CardMovement cardComponent = newCard.GetComponent<CardMovement>();

            if (cardComponent == null)
                cardComponent = newCard.AddComponent<CardMovement>();

            activeCards.Add(newCard);
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
                isArranging = false;
                return;
            }


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
            
            if (activeCards.Contains(destroyedCard))
            {
                activeCards.Remove(destroyedCard);
                
                if (activeCards.Count < 6)
                {
                    isArranging = true;
                }
            }
        }

        public void OnCardPulledOut(GameObject card)
        {
            
            if (activeCards.Contains(card))
            {
                activeCards.Remove(card);
                isArranging = true;
            }
        }

        public void ReturnCardToHand(GameObject card)
        {
            
            if (!activeCards.Contains(card) && activeCards.Count < 6)
            {
                card.transform.SetParent(transform);
                
                activeCards.Add(card);
                
                isArranging = true;
            }
            
            
        }

        public Vector2 GetHandPosition()
        {
            return centralAnchor != null ? centralAnchor.position : transform.position;
        }

        private void CreateDropZone()
        {
            GameObject dropZoneObject = new GameObject("CardDropZone");
            dropZoneObject.AddComponent<CardDropZone>();
        }

        
    }
}