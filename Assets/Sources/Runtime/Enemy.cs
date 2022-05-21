using UnityEngine;

namespace Sources.Runtime
{
    public class Enemy : Character
    {
        public Enemy(int healthValue, int damage, CharacterPresentersBank characterPresentersBank)
            : base(healthValue, damage, characterPresentersBank)
        {
        }

        public override void Activate()
        {
            base.Activate();
            var targets = CharacterPresentersBank.Allies;
            Attack(targets[Random.Range(0, targets.Count)].Model);
        }
    }
}