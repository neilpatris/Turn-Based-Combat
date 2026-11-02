// ===== USING =====
using TurnBasedCombat.Core.Capabilities;

// ===== NAMESPACE =====
namespace TurnBasedCombat.Core.States
{
    // ===== CLASS =====
    public class EnemyTurn : ICombatState
    {
        // ===== FIELDS =====
        private readonly ICapability _swordStrike = new SwordStrike();

        // ===== FUNCTIONS =====
        public ICombatState Next(Unit hero, Unit enemy)
        {
            if (hero.IsDead || enemy.IsDead)
            {
                return new CombatOver();
            }

            return new PlayerTurn();
        }

        public void Enter(Unit hero, Unit enemy)
        {
            enemy.RestoreActionPoints();

            while (enemy.HasEnoughActionPoints(_swordStrike.ActionPointCost) && !hero.IsDead)
            {
                enemy.UseCapability(_swordStrike, hero);
            }
        }
    }
}
