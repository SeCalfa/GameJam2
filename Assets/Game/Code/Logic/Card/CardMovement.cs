using UnityEngine;

namespace Game.Code.Logic.Card
{
    public class CardMovement : MonoBehaviour
    {
        private Vector2 _startPosition;
        private Quaternion _startRotation;
        private bool _returningToStart;
        private bool _isOnTable = false;
        private bool _isDragging = false;
        private CardBend _parentCardBend;

        private const float ReturnSpeed = 10f;
        private const float TableThreshold = 2f; 

        protected virtual void Awake()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
            _parentCardBend = GetComponentInParent<CardBend>();
        }

        protected virtual void Update()
        {
            if (_returningToStart)
            {
                transform.position = Vector2.Lerp(transform.position, _startPosition, Time.deltaTime * ReturnSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, _startRotation, Time.deltaTime * ReturnSpeed);

                if (Vector2.Distance(transform.position, _startPosition) < 0.01f &&
                    Quaternion.Angle(transform.rotation, _startRotation) < 0.1f)
                {
                    transform.position = _startPosition;
                    transform.rotation = _startRotation;
                    _returningToStart = false;
                }
            }
        }

        private void OnMouseDrag()
        {
            if (!_isDragging)
            {
                _isDragging = true;
                // Remove from hand when starting to drag
                if (_parentCardBend != null && !_isOnTable)
                {
                    _parentCardBend.OnCardPulledOut(gameObject);
                    _isOnTable = true;
                }
            }

            // Smoothly rotate to vertical (0 degrees) when dragging
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
            
            if (_isOnTable && _parentCardBend != null)
            {
                ReturnToHand();
            }
            else if (!_isOnTable)
            {
                _returningToStart = true;
            }
        }

        // private void OnMouseDown()
        // {
        //     DestroyCard();
        // }

        public void SetStartPosition(Vector2 newStartPosition)
        {
            _startPosition = newStartPosition;
        }

        public void SetStartRotation(Quaternion newStartRotation)
        {
            _startRotation = newStartRotation;
        }

        public void DestroyCard()
        {
            CardBend cardBend = GetComponentInParent<CardBend>();
            if (cardBend != null)
            {
                cardBend.OnCardDestroyed(gameObject);
            }

            Destroy(gameObject);
        }

        public void ReturnToHand()
        {
            if (_parentCardBend != null && _isOnTable)
            {
                Debug.Log($"CardMovement: Returning {gameObject.name} to hand");
                _parentCardBend.ReturnCardToHand(gameObject);
                _isOnTable = false;
                _returningToStart = false; // cardbend should hadle positioning instead of this script
            }
        }

        public bool IsOnTable()
        {
            return _isOnTable;
        }

    }
}