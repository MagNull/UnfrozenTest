using System;
using Sources.Runtime;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class CharacterPresenter : MonoBehaviour
{
    [field: SerializeReference] public Character Model { get; private set; }
    public event Action<CharacterPresenter> Clicked;

    [SerializeField]
    private int _startHealth;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private CharacterSide _characterSide;
    private MeshRenderer _renderer;
    private Slider _healthSlider;

    private CharacterAnimator _characterAnimator;

    public int OrderInLayer
    {
        get => _renderer.sortingOrder;
        set => _renderer.sortingOrder = value;
    }

    private void Awake()
    {
        var bank = FindObjectOfType<CharacterPresentersBank>();
        _renderer = GetComponentInChildren<MeshRenderer>();
        _characterAnimator = GetComponent<CharacterAnimator>();
        _healthSlider = GetComponentInChildren<Slider>();
        
        Model = _characterSide == CharacterSide.ALLY
            ? new Ally(_startHealth, _damage, bank)
            : new Enemy(_startHealth, _damage, bank);
        
        bank.Add(this);
    }

    private void Start()
    {
        _healthSlider.maxValue = _startHealth;
        _healthSlider.value = _startHealth;
    }
    
    private void OnDied()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Model.OnEnable();

        _characterAnimator.AttackAnimationEnded += Model.OnAttackAnimationEnded;
        _characterAnimator.DamagedAnimationEnded += CheckHealth;

        Model.Attacking += _ => _characterAnimator.OnAttacked();
        Model.Damaged += _characterAnimator.OnDamaged;
    }

    private void OnDisable()
    {
        Model.OnDisable();
        
        _characterAnimator.AttackAnimationEnded -= Model.OnAttackAnimationEnded;
        _characterAnimator.DamagedAnimationEnded -= CheckHealth;
        
        Model.Damaged -= _characterAnimator.OnDamaged;
    }

    private void CheckHealth()
    {
        UpdateHealthSlider();
        if(Model.GetCurrentHealth() <= 0)
            OnDied();
    }
    
    private void UpdateHealthSlider()
    {
        _healthSlider.value = Model.GetCurrentHealth();
    }

    private void OnMouseDown()
    {
        Clicked?.Invoke(this);
    }
}
