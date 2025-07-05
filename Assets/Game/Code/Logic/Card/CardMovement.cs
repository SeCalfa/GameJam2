using Game.Code.Infrastructure.GameObjectsLocator;
using Game.Code.Infrastructure.SO;
using UnityEngine;

// a card that can be dragged across the screen, it can be dragged with the mouse, if released, it returns to the starting position.

namespace Game.Code.Logic.Card
{
    public class Card : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private Vector2 startPosition;
        private bool returningToStart = false;
        private float returnSpeed = 10f;
        void Start()
        {
            startPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void LateUpdate()
        {
            if (returningToStart)
            {
            transform.position = Vector2.Lerp(transform.position, startPosition, Time.deltaTime * returnSpeed);
            if (Vector2.Distance(transform.position, startPosition) < 0.01f)
            {
                transform.position = startPosition;
                returningToStart = false;
            }
            }
        }

        private void OnMouseDrag()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, new Vector2(worldPosition.x, worldPosition.y), Time.deltaTime * returnSpeed);
            returningToStart = false;
        }

        private void OnMouseUp()
        {
            returningToStart = true;
        }

        


    }
}