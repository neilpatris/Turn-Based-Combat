// ===== USING =====
using TurnBasedCombat.Core;
using TurnBasedCombat.Core.Capabilities;
using TurnBasedCombat.Core.States;

// ===== NAMESPACE =====
namespace TurnBasedCombat.Tests
{
    // ===== CLASS =====
    [TestFixture]
    public class CombatTests
    {
        // ===== FIELDS =====
        private Unit _hero = null!;
        private Unit _enemy = null!;
        private Combat _combat = null!;

        // ===== FUNCTIONS =====
        [SetUp]
        public void Setup()
        {
            _hero = new Hero();
            _enemy = new Enemy();
            _combat = new Combat(_hero, _enemy);
        }

        [Test]
        public void EndTurn_SwitchesToEnemyTurn()
        {
            _combat.EndTurn();

            Assert.That(_combat.CurrentState, Is.TypeOf<EnemyTurn>());
        }

        [Test]
        public void EndTurn_EndsCombatWhenEnemyIsDead()
        {
            _enemy.TakeDamage(Unit.MaxHealth);
            _combat.EndTurn();

            Assert.That(_combat.CurrentState, Is.TypeOf<CombatOver>());
        }

        [Test]
        public void EndTurn_EndsCombatWhenHeroIsKilledByEnemy()
        {
            _hero.TakeDamage(Unit.MaxHealth - SwordStrike.Damage);
            _combat.EndTurn();
            _combat.EndTurn();

            Assert.That(_hero.IsDead, Is.True);
            Assert.That(_combat.CurrentState, Is.TypeOf<CombatOver>());
        }

        [Test]
        public void EndTurn_RestoresHeroActionPoints()
        {
            _hero.UseActionPoints(Unit.MaxActionPoints);
            _combat.EndTurn();
            _combat.EndTurn();

            Assert.That(_hero.CurrentActionPoints, Is.EqualTo(Unit.MaxActionPoints));
        }

        [Test]
        public void NewCombat_RestoresHeroActionPoints()
        {
            _hero.UseActionPoints(Unit.MaxActionPoints);
            _combat = new Combat(_hero, _enemy);

            Assert.That(_hero.CurrentActionPoints, Is.EqualTo(Unit.MaxActionPoints));
        }

        [Test]
        public void EndTurn_EnemyUsesSwordStrikeUntilNoActionPoints()
        {
            _combat.EndTurn();

            Assert.That(_hero.CurrentHealth, Is.EqualTo(Unit.MaxHealth - (Unit.MaxActionPoints * SwordStrike.Damage)));
        }

        [Test]
        public void EndTurn_RaisesStateChangedWithNewState()
        {
            ICombatState? newState = null;
            _combat.StateChanged += state => newState = state;

            _combat.EndTurn();

            Assert.That(newState, Is.TypeOf<EnemyTurn>());
        }

        [Test]
        public void EndTurn_DoesNotRaiseStateChangedWhenCombatIsAlreadyOver()
        {
            _enemy.TakeDamage(Unit.MaxHealth);
            _combat.EndTurn();

            int stateChangedCount = 0;
            _combat.StateChanged += state => stateChangedCount++;

            _combat.EndTurn();

            Assert.That(stateChangedCount, Is.EqualTo(0));
        }
    }
}
