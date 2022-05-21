using DG.Tweening;
using Sources.Runtime;
using UnityEngine;

public class FightMover : MonoBehaviour
{
    [SerializeField]
    private float _moveDuration = 1;
    [SerializeField]
    private Transform _allyPosition;
    [SerializeField]
    private Transform _enemyPosition;
    private CharacterPresentersBank _bank;

    private void Awake()
    {
        _bank = FindObjectOfType<CharacterPresentersBank>();
        foreach (var characterPresenter in _bank.AllCharacters)
        {
            characterPresenter.Model.Attacking +=
                target => MeetFighters(characterPresenter.Model, target);
        }
    }

    private void MeetFighters(Character attacker, Character target)
    {
        var attackerPresenter = _bank.GetPresenterByModel(attacker);
        var targetPresenter = _bank.GetPresenterByModel(target);

        var attackerOriginPos = attackerPresenter.transform.position;
        var targetOriginPos = targetPresenter.transform.position;

        attackerPresenter.transform.DOMove(attacker is Ally ? _allyPosition.position : _enemyPosition.position, _moveDuration);
        targetPresenter.transform.DOMove(target is Enemy ? _enemyPosition.position : _allyPosition.position,
            _moveDuration);
        

        attackerPresenter.Model.Attacked += () =>
        {
            attackerPresenter.transform.position = attackerOriginPos;
            targetPresenter.transform.position = targetOriginPos;
        };
    }
}