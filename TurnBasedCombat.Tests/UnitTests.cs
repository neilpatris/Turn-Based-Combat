// ===== USING =====
using TurnBasedCombat.Core;
using TurnBasedCombat.Core.Capabilities;

// ===== NAMESPACE =====
namespace TurnBasedCombat.Tests
{
    // ===== CLASS =====
    [TestFixture]
    public class UnitTests
    {
        // ===== FIELDS =====
        private Unit _hero = null!;
        private Unit _enemy = null!;
        private ICapability _swordStrike = null!;
        private ICapability _heavyStrike = null!;
        private ICapability _potion = null!;

        // ===== FUNCTIONS =====
        [SetUp]
        public void Setup()
        {
            _hero = new Hero();
            _enemy = new Enemy();
            _swordStrike = new SwordStrike();
            _heavyStrike = new HeavyStrike();
            _potion = new Potion();
        }

        [Test]
        public void IsDead_ReturnsTrueAtZeroHealth()
        {
            _hero.TakeDamage(Unit.MaxHealth);

            Assert.That(_hero.IsDead, Is.True);
        }

        [Test]
        public void TakeDamage_RemovesHealth()
        {
            _hero.TakeDamage(30);

            Assert.That(_hero.CurrentHealth, Is.EqualTo(Unit.MaxHealth - 30));
        }

        [Test]
        public void TakeDamage_StopsAtZero()
        {
            _hero.TakeDamage(Unit.MaxHealth + 100);

            Assert.That(_hero.CurrentHealth, Is.EqualTo(Unit.MinHealth));
        }

        [Test]
        public void HasEnoughActionPoints_ReturnsTrueWhenEnough()
        {
            _hero.UseActionPoints(Unit.MaxActionPoints - 1);

            Assert.That(_hero.HasEnoughActionPoints(1), Is.True);
        }

        [Test]
        public void HasEnoughActionPoints_ReturnsFalseWhenNotEnough()
        {
            _hero.UseActionPoints(Unit.MaxActionPoints);

            Assert.That(_hero.HasEnoughActionPoints(1), Is.False);
        }

        [Test]
        public void UseActionPoints_RemovesActionPoints()
        {
            _hero.UseActionPoints(Unit.MaxActionPoints - 1);

            Assert.That(_hero.CurrentActionPoints, Is.EqualTo(1));
        }

        [Test]
        public void UseActionPoints_StopsAtZero()
        {
            _hero.UseActionPoints(Unit.MaxActionPoints + 1);

            Assert.That(_hero.CurrentActionPoints, Is.EqualTo(0));
        }

        [Test]
        public void RestoreActionPoints_ResetsToMax()
        {
            _hero.UseActionPoints(Unit.MaxActionPoints);
            _hero.RestoreActionPoints();

            Assert.That(_hero.CurrentActionPoints, Is.EqualTo(Unit.MaxActionPoints));
        }

        [Test]
        public void Heal_DoesNotExceedMaxHealth()
        {
            _hero.TakeDamage(50);
            _hero.Heal(Unit.MaxHealth);

            Assert.That(_hero.CurrentHealth, Is.EqualTo(Unit.MaxHealth));
        }

        [Test]
        public void Heal_DoesNotReviveDeadUnit()
        {
            _hero.TakeDamage(Unit.MaxHealth + 100);
            _hero.Heal(Unit.MaxHealth + 100);

            Assert.That(_hero.IsDead, Is.True);
        }

        [Test]
        public void TakeDamage_RaisesDamageTakenWithRemainingHealth()
        {
            int remainingHealth = 0;
            _hero.DamageTaken += currentHealth => remainingHealth = currentHealth;

            _hero.TakeDamage(30);

            Assert.That(remainingHealth, Is.EqualTo(Unit.MaxHealth - 30));
        }

        [Test]
        public void TakeDamage_RaisesKilledWhenDamageEqualsRemainingHealth()
        {
            bool isKilled = false;
            _hero.Killed += () => isKilled = true;

            _hero.TakeDamage(Unit.MaxHealth);

            Assert.That(isKilled, Is.True);
        }

        [Test]
        public void TakeDamage_RaisesKilledOnlyOnce()
        {
            int killedCount = 0;
            _hero.Killed += () => killedCount++;

            _hero.TakeDamage(Unit.MaxHealth / 2);
            _hero.TakeDamage(Unit.MaxHealth);
            _hero.TakeDamage(Unit.MaxHealth / 2);

            Assert.That(killedCount, Is.EqualTo(1));
        }

        [Test]
        public void UseCapability_SpendsActionPointsAndDamagesTarget()
        {
            _hero.UseCapability(_heavyStrike, _enemy);

            Assert.That(_enemy.CurrentHealth, Is.EqualTo(Unit.MaxHealth - HeavyStrike.Damage));
            Assert.That(_hero.CurrentActionPoints, Is.EqualTo(Unit.MaxActionPoints - _heavyStrike.ActionPointCost));
        }

        [Test]
        public void UseCapability_PotionHealsCaster()
        {
            _hero.TakeDamage(50);
            _hero.UseCapability(_potion, _enemy);

            Assert.That(_hero.CurrentHealth, Is.EqualTo(Unit.MaxHealth - 50 + Potion.Healing));
            Assert.That(_enemy.CurrentHealth, Is.EqualTo(Unit.MaxHealth));
        }

        [Test]
        public void UseCapability_DoesNothingWhenNotEnoughActionPoints()
        {
            int spentActionPoints = Unit.MaxActionPoints - 1;

            _hero.UseActionPoints(spentActionPoints);
            _hero.UseCapability(_heavyStrike, _enemy);

            Assert.That(_enemy.CurrentHealth, Is.EqualTo(Unit.MaxHealth));
            Assert.That(_hero.CurrentActionPoints, Is.EqualTo(Unit.MaxActionPoints - spentActionPoints));
        }

        [Test]
        public void UseCapability_KeepsActionPointsWhenTargetIsDead()
        {
            _enemy.TakeDamage(Unit.MaxHealth);
            _hero.UseCapability(_swordStrike, _enemy);

            Assert.That(_hero.CurrentActionPoints, Is.EqualTo(Unit.MaxActionPoints));
        }
    }
}
