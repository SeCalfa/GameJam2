using System.Collections.Generic;
using Game.Code.Infrastructure.GameObjectsLocator;
using Game.Code.Infrastructure.SO.Prefabs;
using Game.Code.Infrastructure.SO.Rounds;
using Game.Code.Logic.Card;
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
        public int CurrentStamina { get; set; }
        
        private List<CardEntity> _availableCards = new();

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
            var enemy = _container.CreateGameObject<Transform>(Constants.Enemy, _currentRound.GetEnemy);
            enemy.position = _enemyPoint.position;
            
            // Spawn card hand
            SpawnCardHand();

            // Spawn gameplay HUD
            SpawnGameplayHUD();
            
            // First turn start
            TurnStart(1);
        }

        public void TurnStart(int staminaToAdd)
        {
            var gameplayHud = _container.GetGameObjectByName<GameplayHud>(Constants.GameplayHUD);

            CurrentStamina += staminaToAdd;

            gameplayHud.ToggleStamina(true);
            gameplayHud.ShowStamina(CurrentStamina);
            
            var cardBend = _container.GetGameObjectByName<CardBend>(Constants.CardHand);
            var newRandomCards = TakeRandomCards(cardBend.CardsCountToFull);
            cardBend.AddCards(newRandomCards);
        }

        private void TurnEnd()
        {
            var gameplayHud = _container.GetGameObjectByName<GameplayHud>(Constants.GameplayHUD);
            gameplayHud.ToggleStamina(false);
        }

        public void UpdateStamina()
        {
            var gameplayHud = _container.GetGameObjectByName<GameplayHud>(Constants.GameplayHUD);
            gameplayHud.ShowStamina(CurrentStamina);
        }

        private void SpawnCardHand()
        {
            var cardBend = _container.CreateGameObject<CardBend>(Constants.CardHand, _prefabsList.GetCardHand);
            cardBend.Construct(this);
            _availableCards = new List<CardEntity>(_currentRound.GetCards);

            var newRandomCards = TakeRandomCards(cardBend.CardsCountToFull);
            cardBend.AddCards(newRandomCards);
        }

        private void SpawnGameplayHUD()
        {
            var gameplayHud = _container.CreateGameObject<GameplayHud>(Constants.GameplayHUD, _prefabsList.GetGameplayHUD);
            gameplayHud.OnTurnEnd += TurnEnd;
        }

        private List<CardEntity> TakeRandomCards(int count)
        {
            var newCards = new List<CardEntity>();

            for (var i = 0; i < count; i++)
            {
                var randomCardIndex = Random.Range(0, _availableCards.Count);
                var randomCard = _availableCards[randomCardIndex];
                
                newCards.Add(randomCard);
                _availableCards.RemoveAt(randomCardIndex);

                if (_availableCards.Count == 0)
                {
                    _availableCards = new List<CardEntity>(_currentRound.GetCards);
                }
            }
            
            return newCards;
        }
    }
}
