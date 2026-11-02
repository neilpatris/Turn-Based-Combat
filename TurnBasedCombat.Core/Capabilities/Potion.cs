// ===== NAMESPACE =====
namespace TurnBasedCombat.Core.Capabilities
{
    // ===== CLASS =====
    public class Potion : ICapability
    {
        // ===== FIELDS =====
        public const int Healing = 20;
        public string Name => "Potion";
        public int ActionPointCost => 1;
        
        // ===== FUNCTIONS =====
        public void Use(Unit caster, Unit target)
        {
            caster.Heal(Healing);
        }
    }
}
