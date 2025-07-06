using Game.Code.Infrastructure.GameObjectsLocator;
using Game.Code.Infrastructure.SO.Prefabs;
using Game.Code.Infrastructure.SO.Rounds;
using Game.Code.Logic.UI;
using UnityEngine;

namespace Game.Code.Infrastructure
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform enemyPoint;
        [Space]
        [SerializeField] private PrefabsList prefabsList;
        [Space]
        [SerializeField] private RoundData round1;
        [SerializeField] private RoundData round2;
        [SerializeField] private RoundData round3;

        private Gameplay _gameplay;
        private Container _container;
        private bool _isFirstInit = true;
        
        public static GameManager Instance { get; private set; }
        public PrefabsList PrefabsList => prefabsList;
        public Container Container => _container;

        private void Awake()
        {
            if (Instance)
            {
                return;
            }
            
            Instance = this;
            
            Init();
            
            DontDestroyOnLoad(gameObject);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (scene.name == "Game" && !_isFirstInit)
            {
                if (enemyPoint == null)
                {
                    GameObject enemyPointObj = GameObject.Find("EnemyPoint");
                    enemyPoint = enemyPointObj.transform;
                }
                
                Init();
            }
            
            if (_isFirstInit)
            {
                _isFirstInit = false;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                ChangeScene changeScene = FindFirstObjectByType<ChangeScene>();
                changeScene.TransitionToScene("EndGame");
            }
        }

        private void Init()
        {
            _container = new Container();
            _gameplay = new Gameplay(_container, prefabsList, enemyPoint, round1, round2, round3);

            _gameplay.RunRound(Round.Round1);
        }
    }
}