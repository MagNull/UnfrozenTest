using System;
using System.Collections.Generic;
using Source.Slime_Components;
using UnityEngine;

namespace Sources.Runtime
{
    [Serializable]
    public abstract class Character
    {
        public event Action Activated;
        public event Action Deactivated;
        
        public event Action<Character> Attacking;
        public event Action Attacked;
        
        public event Action Died;
        
        public event Action Damaged;

        protected readonly CharacterPresentersBank CharacterPresentersBank;
        [SerializeField]
        private Health _health;
        private readonly int _damage;

        public int GetCurrentHealth() => _health.Value;

        protected Character(int healthValue, int damage, CharacterPresentersBank characterPresentersBank)
        {
            _health = new Health(healthValue);
            _damage = damage;
            CharacterPresentersBank = characterPresentersBank;
        }

        public virtual void Activate()
        {
            Activated?.Invoke();
        }

        public void OnAttackAnimationEnded()
        {
            Attacked?.Invoke();
            Deactivate();
        }

        public virtual void Deactivate()
        {
            Deactivated?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            Damaged?.Invoke();
        }

        protected void Attack(Character target)
        {
            Attacking?.Invoke(target);
            target.TakeDamage(_damage);
        }

        public void OnEnable()
        {
            _health.Died += OnDied;
        }

        public void OnDisable()
        {
            _health.Died -= OnDied;
        }

        private void OnDied()
        {
            Died?.Invoke();
        }
    }
}