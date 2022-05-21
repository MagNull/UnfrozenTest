using System;
using UnityEngine;

namespace Source.Slime_Components
{
    [Serializable]
    public class Health
    {
        public event Action Died;
        
        [SerializeField]
        private int _value;

        public int Value => _value;

        public Health(int value)
        {
            _value = value;
        }

        public void TakeDamage(int damage)
        {
            if(Value <= 0)
                return;
            
            _value = Value - damage;
            if(Value <= 0)
                Died?.Invoke();
        }
    }
}