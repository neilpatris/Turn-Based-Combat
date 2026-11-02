// ===== NAMESPACE =====
namespace TurnBasedCombat.Core.Capabilities
{
    // ===== CLASS =====
    public class SwordStrike : ICapability
    {
        // ===== FIELDS =====
        public const int Damage = 10;
        public string Name => "Sword Strike";
        public int ActionPointCost => 1;
        
        // ===== FUNCTIONS =====
        public void Use(Unit caster, Unit target)
        {
            target.TakeDamage(Damage);
        }
    }
}
