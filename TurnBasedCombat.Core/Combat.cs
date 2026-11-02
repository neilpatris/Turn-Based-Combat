// ===== USING =====
using TurnBasedCombat.Core.States;

// ===== NAMESPACE =====
namespace TurnBasedCombat.Core
{
    // ===== CLASS =====
    public class Combat
    {
        // ===== FIELDS =====
        public ICombatState CurrentState => _currentState;

        public event Action<ICombatState>? StateChanged;

        private Unit _hero;
        private Unit _enemy;
        private ICombatState _currentState;

        // ===== FUNCTIONS =====
        public Combat(Unit hero, Unit enemy)
        {
            _hero = hero;
            _enemy = enemy;
            _currentState = new PlayerTurn();
            _currentState.Enter(_hero, _enemy);
        }

        public void EndTurn()
        {
            ICombatState nextState = _currentState.Next(_hero, _enemy);

            if (nextState == _currentState)
            {
                return;
            }

            _currentState = nextState;
            OnStateChanged();
            _currentState.Enter(_hero, _enemy);
        }

        private void OnStateChanged()
        {
            StateChanged?.Invoke(_currentState);
        }
    }
}
