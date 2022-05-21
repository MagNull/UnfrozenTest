using System.Linq;
using Sources.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionPanel : MonoBehaviour
{
    [SerializeField]
    private Button _attackButton;
    private CharacterPresentersBank _characterBank;

    private void Start()
    {
        _characterBank = FindObjectOfType<CharacterPresentersBank>();
        foreach (Ally ally in _characterBank.Allies.Select(pr => pr.Model))
        {
            ally.Activated += () =>
            {
                _attackButton.onClick.AddListener(ally.PrepareAttack);
                gameObject.SetActive(true);
            };
            ally.Deactivated += () =>
            {
                _attackButton.onClick.RemoveListener(ally.PrepareAttack);
                gameObject.SetActive(false);
            };
            ally.Attacking += _ =>
            {
                _attackButton.onClick.RemoveListener(ally.PrepareAttack);
                gameObject.SetActive(false);
            };
        }
    }
}