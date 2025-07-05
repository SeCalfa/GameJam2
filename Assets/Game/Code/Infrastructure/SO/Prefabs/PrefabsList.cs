using UnityEngine;

namespace Game.Code.Infrastructure.SO.Prefabs
{
    [CreateAssetMenu(fileName = "PrefabsList", menuName = "SO/PrefabsList")]
    public class PrefabsList : ScriptableObject
    {
        [SerializeField] private GameObject gameplayHUD;
        [SerializeField] private GameObject cardHand;

        [SerializeField] private GameObject cardDropZone;

        public GameObject GetGameplayHUD => gameplayHUD;
        public GameObject GetCardHand => cardHand;

        public GameObject GetcardDropZone => cardDropZone;
    }
}
