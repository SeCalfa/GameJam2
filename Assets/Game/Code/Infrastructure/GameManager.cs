using Game.Code.Infrastructure.GameObjectsLocator;
using Game.Code.Infrastructure.SO.Prefabs;
using Game.Code.Infrastructure.SO.Rounds;
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
            _container = new Container();
            _gameplay = new Gameplay(_container, prefabsList, enemyPoint, round1, round2, round3);

            _gameplay.RunRound(Round.Round1);
        }
    }
}