using UnityEngine;

namespace Game.Code.Infrastructure.SO.Prefabs
{
    [CreateAssetMenu(fileName = "PrefabsList", menuName = "SO/PrefabsList")]
    public class PrefabsList : ScriptableObject
    {
        [SerializeField] private GameObject gameplayHUD;
        [SerializeField] private GameObject cardHand;


        public GameObject GetGameplayHUD => gameplayHUD;
        public GameObject GetCardHand => cardHand;

    }
}
