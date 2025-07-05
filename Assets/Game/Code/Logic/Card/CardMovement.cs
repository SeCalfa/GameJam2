using UnityEngine;

namespace Game.Code.Logic.Card
{
    public class CardMovement : MonoBehaviour
    {
        private Vector2 _startPosition;
        private bool _returningToStart;
        
        private const float ReturnSpeed = 10f;

        protected virtual void Awake()
        {
            _startPosition = transform.position;
        }

        protected virtual void Update()
        {
            if (_returningToStart)
            {
                transform.position = Vector2.Lerp(transform.position, _startPosition, Time.deltaTime * ReturnSpeed);
                
                if (Vector2.Distance(transform.position, _startPosition) < 0.01f)
                {
                    transform.position = _startPosition;
                    _returningToStart = false;
                }
            }
        }

        private void OnMouseDrag()
        {
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
            _returningToStart = true;
        }
    }
}