using UnityEngine;

namespace Game.Code.Infrastructure.SO.Prefabs
{
    [CreateAssetMenu(fileName = "PrefabsList", menuName = "SO/PrefabsList")]
    public class PrefabsList : ScriptableObject
    {
        [SerializeField] private GameObject gameplayHUD;

        public GameObject GetGameplayHUD => gameplayHUD;
    }
}
