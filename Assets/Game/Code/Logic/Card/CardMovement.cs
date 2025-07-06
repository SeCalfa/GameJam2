using System;
using UnityEngine;

namespace Game.Code.Logic.Card
{
    public class CardMovement : MonoBehaviour
    {
        private Vector2 _startPosition;
        private Quaternion _startRotation;
        private Vector3 _startScale;
        private bool _returningToStart;
        private bool _isOnTable;
        private bool _isDragging;
        private CardBend _parentCardBend;

        private const float ReturnSpeed = 10f;
        private const float DragScale = 1.4f;

        protected virtual void Awake()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            _startScale = transform.localScale;
            _parentCardBend = GetComponentInParent<CardBend>();
        }

        protected virtual void Update()
        {
            if (_returningToStart)
            {
                transform.position = Vector2.Lerp(transform.position, _startPosition, Time.deltaTime * ReturnSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, _startRotation, Time.deltaTime * ReturnSpeed);
                transform.localScale = Vector3.Lerp(transform.localScale, _startScale, Time.deltaTime * ReturnSpeed);

                if (Vector2.Distance(transform.position, _startPosition) < 0.01f &&
                    Quaternion.Angle(transform.rotation, _startRotation) < 0.1f)
                {
                    transform.position = _startPosition;
                    transform.rotation = _startRotation;
                    transform.localScale = _startScale;
                    _returningToStart = false;
                }
            }
        }

        private void OnMouseDrag()
        {
            if (!_isDragging)
            {
                _isDragging = true;
                transform.localScale = _startScale * DragScale;
                
                if (_parentCardBend != null && !_isOnTable)
                {
                    _parentCardBend.OnCardPulledOut(gameObject);
                    _isOnTable = true;
                }
            }
            if (_isOnTable)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * ReturnSpeed);
            }

            Vector2 mousePosition = Input.mousePosition;

            if (Camera.main != null)
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                transform.position = Vector2.Lerp(transform.position, new Vector2(worldPosition.x, worldPosition.y), Time.deltaTime * ReturnSpeed);
            }

            _returningToStart = false;
        }

        private void OnMouseUp()
        {
            _isDragging = false;
            
            // Return card to normal scale when releasing
            transform.localScale = _startScale;

            if (_isOnTable && _parentCardBend != null)
            {
                var cardUsed = CardDropZone.Instance.IsCardInZone(this) && UseCard();
                if (cardUsed)
                {
                    DestroyCard();
                    return;
                }

                ReturnToHand();
            }
            else if (!_isOnTable)
            {
                _returningToStart = true;
            }
        }

        public void SetStartPosition(Vector2 newStartPosition)
        {
            _startPosition = newStartPosition;
        }

        public void SetStartRotation(Quaternion newStartRotation)
        {
            _startRotation = newStartRotation;
        }

        public void SetStartScale(Vector3 newStartScale)
        {
            _startScale = newStartScale;
        }

        public void DestroyCard()
        {
            var cardBend = GetComponentInParent<CardBend>();
            if (cardBend != null)
            {
                cardBend.OnCardDestroyed(gameObject);
            }

            Destroy(gameObject);
        }

        private void ReturnToHand()
        {
            if (_parentCardBend != null && _isOnTable)
            {
                _parentCardBend.ReturnCardToHand(gameObject);
                _isOnTable = false;
                _returningToStart = false; // cardbend should hadle positioning instead of this script
            }
        }

        protected virtual bool UseCard()
        {
            return false;
        }
    }
}