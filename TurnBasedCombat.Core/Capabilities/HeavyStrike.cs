// ===== NAMESPACE =====
namespace TurnBasedCombat.Core.Capabilities
{
    // ===== CLASS =====
    public class HeavyStrike : ICapability
    {
        // ===== FIELDS =====
        public const int Damage = 25;
        public string Name => "Heavy Strike";
        public int ActionPointCost => 2;
        
        // ===== FUNCTIONS =====
        public void Use(Unit caster, Unit target)
        {
            target.TakeDamage(Damage);
        }
    }
}
