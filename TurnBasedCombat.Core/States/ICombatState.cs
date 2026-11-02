// ===== NAMESPACE =====
namespace TurnBasedCombat.Core.States
{
    // ===== INTERFACE =====
    public interface ICombatState
    {
        ICombatState Next(Unit hero, Unit enemy);
        void Enter(Unit hero, Unit enemy);
    }
}
