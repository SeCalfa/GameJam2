using Game.Code.Infrastructure.GameObjectsLocator;
using Game.Code.Infrastructure.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Code.Infrastructure
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PrefabsList prefabsList;

        public PrefabsList GetPrefabsList => prefabsList;

        public Container Container { get; private set; }
        
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                return;
            }
            
            Instance = this;
            
            Init();
            
            DontDestroyOnLoad(gameObject);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void Init()
        {
            Container = new Container();
        }
    }
}