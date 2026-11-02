// ===== NAMESPACE =====
namespace TurnBasedCombat.Core.States
{
    // ===== CLASS =====
    public class PlayerTurn : ICombatState
    {
        // ===== FUNCTIONS =====
        public ICombatState Next(Unit hero, Unit enemy)
        {
            if (hero.IsDead || enemy.IsDead)
            {
                return new CombatOver();
            }

            return new EnemyTurn();
        }

        public void Enter(Unit hero, Unit enemy)
        {
            hero.RestoreActionPoints();
        }
    }
}
