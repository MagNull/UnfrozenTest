using System;
using Spine.Unity;
using UnityEngine;

namespace Sources.Runtime
{
    public class CharacterAnimator : MonoBehaviour
    {
        public event Action AttackAnimationEnded;
        public event Action DamagedAnimationEnded;
        private const string Attack = "PickaxeCharge";
        private const string Idle = "Idle";
        private const string Damage = "Damage";
        private SkeletonAnimation _animation;

        private void Awake()
        {
            _animation = GetComponentInChildren<SkeletonAnimation>();
        }

        public void OnAttacked()
        {
            var attack = _animation.state.SetAnimation(0, Attack, false);
            attack.End += _ => AttackAnimationEnded?.Invoke();
            _animation.state.AddAnimation(0, Idle, true, attack.delay);
        }

        public void OnDamaged()
        {
            var damaged = _animation.state.SetAnimation(0, Damage, false);
            damaged.End += _ => DamagedAnimationEnded?.Invoke();
            _animation.state.AddAnimation(0, Idle, true, damaged.delay);
        }
    }
}