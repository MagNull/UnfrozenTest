namespace Sources.Runtime
{
    public class Ally : Character
    {
        public Ally(int healthValue, int damage, CharacterPresentersBank characterPresentersBank)
            : base(healthValue, damage, characterPresentersBank)
        {
        }

        public void PrepareAttack()
        {
            foreach (var enemy in CharacterPresentersBank.Enemies)
            {
                enemy.Clicked -= Attack;
                enemy.Clicked += Attack;
            }
        }

        public override void Deactivate()
        {
            foreach (var enemy in CharacterPresentersBank.Enemies)
            {
                enemy.Clicked -= Attack;
            }
            base.Deactivate();
        }

        private void Attack(CharacterPresenter characterPresenter)
        {
            foreach (var enemy in CharacterPresentersBank.Enemies)
            {
                enemy.Clicked -= Attack;
            }
            Attack(characterPresenter.Model);
        }
    }
}