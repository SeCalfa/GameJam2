using Game.Code.Infrastructure.GameObjectsLocator;
using Game.Code.Infrastructure.SO;
using UnityEngine;

namespace Game.Code.Infrastructure
{
    public class LevelManager : MonoBehaviour
    {
        private PrefabsList _prefabsList;
        private Container _container;
        
        private void Awake()
        {
            _prefabsList = GameManager.Instance.GetPrefabsList;
            _container = GameManager.Instance.Container;
            
            SpawnTestPrefab();
        }

        private void SpawnTestPrefab()
        {
            _container.CreateGameObject(Constants.TestName, _prefabsList.GetTest);
        }
    }
}