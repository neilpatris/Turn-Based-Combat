// ===== USING =====
using TurnBasedCombat.Core;
using TurnBasedCombat.Core.Capabilities;
using TurnBasedCombat.Core.States;

// ===== NAMESPACE =====
namespace TurnBasedCombat.ConsoleApp
{
    // ===== CLASS =====
    public static class Program
    {
        // ===== FIELDS =====
        private static ICapability[] _capabilities =
        {
            new SwordStrike(),
            new HeavyStrike(),
            new Potion()
        };

        // ===== FUNCTIONS =====
        public static void Main()
        {
            Unit player = new Hero();
            Unit enemy = new Enemy();

            player.DamageTaken += HandlePlayerDamageTaken;
            player.HealReceived += HandlePlayerHealReceived;
            player.Killed += HandlePlayerKilled;
            enemy.DamageTaken += HandleEnemyDamageTaken;
            enemy.Killed += HandleEnemyKilled;

            Combat combat = new Combat(player, enemy);
            combat.StateChanged += HandleCombatStateChanged;

            DisplayMessage.DisplayWelcome();
            HandleCombatStateChanged(combat.CurrentState);

            while (combat.CurrentState is not CombatOver)
            {
                PlayPlayerTurn(player, enemy);
                combat.EndTurn();
                combat.EndTurn();
            }

            combat.StateChanged -= HandleCombatStateChanged;
            player.DamageTaken -= HandlePlayerDamageTaken;
            player.HealReceived -= HandlePlayerHealReceived;
            player.Killed -= HandlePlayerKilled;
            enemy.DamageTaken -= HandleEnemyDamageTaken;
            enemy.Killed -= HandleEnemyKilled;
        }

        private static void PlayPlayerTurn(Unit player, Unit enemy)
        {
            int lowestCost = _capabilities.Min(capability => capability.ActionPointCost);
            int passTurnChoice = _capabilities.Length + 1;

            while (player.HasEnoughActionPoints(lowestCost) && !enemy.IsDead)
            {
                DisplayMessage.DisplayStatus(player, enemy);
                DisplayMessage.DisplayActions(_capabilities);

                string? input = Console.ReadLine();

                if (input is null)
                {
                    return;
                }

                string trimmedInput = input.Trim();

                if (!int.TryParse(trimmedInput, out int choice) || choice < 1 || choice > passTurnChoice)
                {
                    DisplayMessage.DisplayInvalidChoice(passTurnChoice);
                    continue;
                }

                if (choice == passTurnChoice)
                {
                    DisplayMessage.DisplayTurnPassed();
                    return;
                }

                ICapability capability = _capabilities[choice - 1];

                if (!player.HasEnoughActionPoints(capability.ActionPointCost))
                {
                    DisplayMessage.DisplayNotEnoughActionPoints(capability, player.CurrentActionPoints);
                    continue;
                }

                DisplayMessage.DisplayCapabilityUsed(capability);
                player.UseCapability(capability, enemy);
            }
        }

        // ===== EVENT HANDLERS =====
        private static void HandleCombatStateChanged(ICombatState state)
        {
            switch (state)
            {
                case PlayerTurn:
                    DisplayMessage.DisplayPlayerTurn();
                    break;
                case EnemyTurn:
                    DisplayMessage.DisplayEnemyTurn();
                    break;
                case CombatOver:
                    DisplayMessage.DisplayCombatOver();
                    break;
            }
        }

        private static void HandlePlayerDamageTaken(int currentHealth)
        {
            DisplayMessage.DisplayPlayerDamageTaken(currentHealth);
        }

        private static void HandlePlayerHealReceived(int currentHealth)
        {
            DisplayMessage.DisplayPlayerHealReceived(currentHealth);
        }

        private static void HandlePlayerKilled()
        {
            DisplayMessage.DisplayDefeat();
        }

        private static void HandleEnemyDamageTaken(int currentHealth)
        {
            DisplayMessage.DisplayEnemyDamageTaken(currentHealth);
        }

        private static void HandleEnemyKilled()
        {
            DisplayMessage.DisplayVictory();
        }
    }
}
