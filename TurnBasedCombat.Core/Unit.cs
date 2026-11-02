// ===== USING =====
using TurnBasedCombat.Core.Capabilities;

// ===== NAMESPACE =====
namespace TurnBasedCombat.Core
{
    // ===== CLASS =====
    public abstract class Unit
    {
        // ===== FIELDS =====
        public const int MaxHealth = 100;
        public const int MinHealth = 0;
        public const int MaxActionPoints = 3;

        public string Name => _name;
        public bool IsDead => _currentHealth <= MinHealth;
        public int CurrentHealth => _currentHealth;
        public int CurrentActionPoints => _currentActionPoints;

        public event Action<int>? DamageTaken;
        public event Action<int>? HealReceived;
        public event Action? Killed;

        private readonly string _name;
        private int _currentHealth;
        private int _currentActionPoints;

        // ===== FUNCTIONS =====
        protected Unit(string name)
        {
            _name = name;
            _currentHealth = MaxHealth;
            _currentActionPoints = MaxActionPoints;
        }

        public void TakeDamage(int amount)
        {
            if (IsDead)
            {
                return;
            }

            _currentHealth = Math.Max(MinHealth, _currentHealth - amount);
            OnDamageTaken();

            if (IsDead)
            {
                OnKilled();
            }
        }

        public void Heal(int amount)
        {
            if (IsDead)
            {
                return;
            }

            _currentHealth = Math.Min(MaxHealth, _currentHealth + amount);
            OnHealReceived();
        }

        public bool HasEnoughActionPoints(int amount)
        {
            return _currentActionPoints >= amount;
        }

        public void UseActionPoints(int amount)
        {
            _currentActionPoints = Math.Max(0, _currentActionPoints - amount);
        }

        public void RestoreActionPoints()
        {
            _currentActionPoints = MaxActionPoints;
        }

        public void UseCapability(ICapability capability, Unit target)
        {
            if (IsDead || target.IsDead)
            {
                return;
            }

            if (!HasEnoughActionPoints(capability.ActionPointCost))
            {
                return;
            }

            UseActionPoints(capability.ActionPointCost);
            capability.Use(this, target);
        }

        private void OnDamageTaken()
        {
            DamageTaken?.Invoke(_currentHealth);
        }

        private void OnHealReceived()
        {
            HealReceived?.Invoke(_currentHealth);
        }

        private void OnKilled()
        {
            Killed?.Invoke();
        }
    }
}
