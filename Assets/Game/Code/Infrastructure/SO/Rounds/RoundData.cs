using Game.Code.Logic.Card;
using UnityEngine;

namespace Game.Code.Infrastructure.SO.Rounds
{
    [CreateAssetMenu(fileName = "RoundData", menuName = "SO/RoundData")]
    public class RoundData : ScriptableObject
    {
        [SerializeField] private GameObject enemy;
        [SerializeField] private CardEntity[] cards;
        
        public GameObject GetEnemy => enemy;
        public CardEntity[] GetCards => cards;
    }
}
