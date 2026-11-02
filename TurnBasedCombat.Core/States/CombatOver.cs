// ===== NAMESPACE =====
namespace TurnBasedCombat.Core.States
{
    // ===== CLASS =====
    public class CombatOver : ICombatState
    {
        // ===== FUNCTIONS =====
        public ICombatState Next(Unit hero, Unit enemy)
        {
            return this;
        }

        public void Enter(Unit hero, Unit enemy)
        {
        }
    }
}
