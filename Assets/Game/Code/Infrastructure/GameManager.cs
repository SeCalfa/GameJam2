using Game.Code.Infrastructure.GameObjectsLocator;
using Game.Code.Infrastructure.SO.Prefabs;
using Game.Code.Infrastructure.SO.Rounds;
using UnityEngine;

namespace Game.Code.Infrastructure
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PrefabsList prefabsList;
        [Space]
        [SerializeField] private RoundData round1;
        [SerializeField] private RoundData round2;
        [SerializeField] private RoundData round3;

        public PrefabsList GetPrefabsList => prefabsList;

        private Gameplay _gameplay;
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
            _gameplay = new Gameplay(round1, round2, round3);
            Container = new Container();
            
            _gameplay.RunRound(Round.Round1);
        }
    }
}