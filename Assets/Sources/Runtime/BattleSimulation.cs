using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Runtime
{
    public class BattleSimulation : MonoBehaviour
    {
        [SerializeField]
        private List<CharacterPresenter> _characters = new();
        private CombatBlackout _combatBlackout;
        private CharacterPresenter _activeCharacter;
        private Round _round;

        class Round
        {
            public event Action RoundEnded;
            private readonly List<CharacterPresenter> _turnOrder;

            public Round(List<CharacterPresenter> turnOrder)
            {
                _turnOrder = turnOrder;
                foreach (var characterPresenter in _turnOrder)
                    characterPresenter.Model.Died += () => _turnOrder.Remove(characterPresenter);
            }

            public CharacterPresenter GetNextTurn()
            {
                if (_turnOrder.Count == 0)
                {
                    RoundEnded?.Invoke();
                    return null;
                }
                
                var nextTurn = _turnOrder.Last();
                _turnOrder.Remove(nextTurn);
                if (_turnOrder.Count == 0)
                    RoundEnded?.Invoke();

                return nextTurn;
            }
        }

        public void SkipTurn()
        {
            _activeCharacter.Model.Deactivate();
        }

        private void NextTurn()
        {
            if(_activeCharacter)
                _activeCharacter.Model.Deactivated -= NextTurn;
            
            _activeCharacter = _round.GetNextTurn();
            if (_activeCharacter == null)
            {
                RandomizeBattleTurns();
                _activeCharacter = _round.GetNextTurn();
            }

            _activeCharacter.Model.Deactivated += NextTurn;
            
            _activeCharacter.Model.Activate();
        }

        private void Awake()
        {
            _characters = FindObjectsOfType<CharacterPresenter>().ToList();
        }

        private void Start()
        {
            foreach (var characterPresenter in _characters)
                characterPresenter.Model.Died += () => _characters.Remove(characterPresenter);
            
            RandomizeBattleTurns();
            NextTurn();
        }

        private void RandomizeBattleTurns()
        {
            var randomTurns = _characters.OrderBy(_ => Random.value).ToList();
            _round = new Round(randomTurns);
            _round.RoundEnded += OnRoundEnded;
        }

        private void OnRoundEnded()
        {
            _round.RoundEnded -= OnRoundEnded;
            RandomizeBattleTurns();
        }
    }
}