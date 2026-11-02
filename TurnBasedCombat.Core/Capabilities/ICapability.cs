// ===== NAMESPACE =====
namespace TurnBasedCombat.Core.Capabilities
{
    // ===== INTERFACE =====
    public interface ICapability
    {
        string Name { get; }
        int ActionPointCost { get; }
        void Use(Unit caster, Unit target);
    }
}
