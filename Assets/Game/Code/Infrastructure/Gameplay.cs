using Game.Code.Infrastructure.GameObjectsLocator;
using Game.Code.Infrastructure.SO.Prefabs;
using Game.Code.Infrastructure.SO.Rounds;
using Game.Code.Logic.UI.Gameplay;
using UnityEngine;

namespace Game.Code.Infrastructure
{
    public class Gameplay
    {
        private readonly Container _container;
        private readonly PrefabsList _prefabsList;
        private readonly Transform _enemyPoint;
        private readonly RoundData _round1;
        private readonly RoundData _round2;
        private readonly RoundData _round3;

        private RoundData _currentRound;
        private int _currentStamina;

        public Gameplay(
            Container container,
            PrefabsList prefabsList,
            Transform enemyPoint,
            RoundData round1, RoundData round2, RoundData round3)
        {
            _container = container;
            _prefabsList = prefabsList;
            _enemyPoint = enemyPoint;
            _round1 = round1;
            _round2 = round2;
            _round3 = round3;
        }

        public void RunRound(Round round)
        {
            _currentRound = round switch
            {
                Round.Round1 => _round1,
                Round.Round2 => _round2,
                _ => _round3
            };
            
            // Spawn enemy
            var enemy = _container.CreateGameObject(Constants.Enemy, _currentRound.GetEnemy);
            enemy.transform.position = _enemyPoint.position;
            
            // Spawn card table
            // ---

            // Spawn gameplay HUD
            SpawnGameplayHUD();
            
            // First turn start
            TurnStart(1);
        }

        public void TurnStart(int staminaToAdd)
        {
            _currentStamina += staminaToAdd;
            
            var gameplayHud = _container.GetGameObjectByName<GameplayHud>(Constants.GameplayHUD);
            gameplayHud.ShowStamina(_currentStamina);
        }
        
        public void TurnEnd()
        {
            
        }
        
        private void SpawnGameplayHUD()
        {
            _container.CreateGameObject(Constants.GameplayHUD, _prefabsList.GetGameplayHUD);
        }
    }
}
