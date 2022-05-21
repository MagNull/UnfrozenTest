using System;
using UnityEngine;

namespace Sources.Runtime
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AttackBlackout : MonoBehaviour
    {
        private CharacterPresentersBank _bank;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _bank = FindObjectOfType<CharacterPresentersBank>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            foreach (var characterPresenter in _bank.AllCharacters)
            {
                characterPresenter.Model.Attacking +=
                    target => HighlightFighters(characterPresenter.Model, target);
                characterPresenter.Model.Deactivated += () => gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
        }

        private void HighlightFighters(Character attacker, Character target)
        {
            var attackerPresenter = _bank.GetPresenterByModel(attacker);
            var targetPresenter = _bank.GetPresenterByModel(target);
            var originOrder = attackerPresenter.OrderInLayer;
            gameObject.SetActive(true);

            attackerPresenter.OrderInLayer = targetPresenter.OrderInLayer = _spriteRenderer.sortingOrder + 1;
            attacker.Attacked += () =>
                attackerPresenter.OrderInLayer = targetPresenter.OrderInLayer = originOrder;
        }
    }
}