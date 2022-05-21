using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Runtime
{
    public class CharacterPresentersBank : MonoBehaviour
    {
        private readonly List<CharacterPresenter> _allies = new();
        private readonly List<CharacterPresenter> _enemies = new();

        public IReadOnlyList<CharacterPresenter> Allies => _allies;

        public IReadOnlyList<CharacterPresenter> Enemies => _enemies;

        public IReadOnlyList<CharacterPresenter> AllCharacters => _allies.Concat(_enemies).ToList();


        public void Add(CharacterPresenter characterPresenter)
        {
            if (characterPresenter.Model is Ally)
            {
                _allies.Add(characterPresenter);
                characterPresenter.Model.Died += () => _allies.Remove(characterPresenter);
            }
            else
            {
                _enemies.Add(characterPresenter);
                characterPresenter.Model.Died += () => _enemies.Remove(characterPresenter);
            }
        }

        public CharacterPresenter GetPresenterByModel(Character character)
        {
            List<CharacterPresenter> targetList = character is Ally ? _allies : _enemies;
            
            return targetList.FirstOrDefault(ch => ch.Model == character);
        }
    }
}