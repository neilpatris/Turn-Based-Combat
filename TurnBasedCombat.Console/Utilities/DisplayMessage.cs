// ===== USING =====
using TurnBasedCombat.Core;
using TurnBasedCombat.Core.Capabilities;

// ===== NAMESPACE =====
namespace TurnBasedCombat.ConsoleApp
{
    // ===== CLASS =====
    public static class DisplayMessage
    {
        // ===== FIELDS =====
        private const int SeparatorLength = 50;

        // ===== FUNCTIONS =====
        public static void DisplayWelcome()
        {
            Console.WriteLine("Welcome to Turn-Based Combat!");
            Console.WriteLine("Defeat the enemy before it defeats you.");
            Console.WriteLine($"Each turn, you get {Unit.MaxActionPoints} action points to spend on your actions.");
        }

        public static void DisplayPlayerTurn()
        {
            DisplayTitle("YOUR TURN");
        }

        public static void DisplayEnemyTurn()
        {
            DisplayTitle("ENEMY TURN");
        }

        public static void DisplayCombatOver()
        {
            DisplayTitle("COMBAT OVER");
        }

        public static void DisplayStatus(Unit player, Unit enemy)
        {
            string playerActionPoints = $"{player.CurrentActionPoints}/{Unit.MaxActionPoints}";

            Console.WriteLine();
            Console.WriteLine($"{"UNIT",-10}{"HEALTH",-12}ACTION POINTS");
            Console.WriteLine(new string('-', SeparatorLength));
            Console.WriteLine($"{player.Name,-10}{FormatHealth(player.CurrentHealth),-12}{playerActionPoints}");
            Console.WriteLine($"{enemy.Name,-10}{FormatHealth(enemy.CurrentHealth),-12}-");
            Console.WriteLine();
        }

        public static void DisplayActions(ICapability[] capabilities)
        {
            Console.WriteLine("Please select an action:");

            for (int i = 0; i < capabilities.Length; i++)
            {
                Console.WriteLine($"\t{i + 1}. {capabilities[i].Name,-20}{capabilities[i].ActionPointCost} AP");
            }

            Console.WriteLine($"\t{capabilities.Length + 1}. Pass turn");
        }

        public static void DisplayCapabilityUsed(ICapability capability)
        {
            Console.WriteLine($"You use {capability.Name}.");
        }

        public static void DisplayTurnPassed()
        {
            Console.WriteLine("You pass your turn.");
        }

        // Combat messages
        public static void DisplayPlayerDamageTaken(int currentHealth)
        {
            Console.WriteLine($"The enemy hits you! Your health: {FormatHealth(currentHealth)}");
        }

        public static void DisplayPlayerHealReceived(int currentHealth)
        {
            Console.WriteLine($"You recover health. Your health: {FormatHealth(currentHealth)}");
        }

        public static void DisplayEnemyDamageTaken(int currentHealth)
        {
            Console.WriteLine($"The enemy takes damage. Enemy health: {FormatHealth(currentHealth)}");
        }

        public static void DisplayVictory()
        {
            Console.WriteLine("Victory! You have defeated the enemy.");
        }

        public static void DisplayDefeat()
        {
            Console.WriteLine("Defeat! You have been killed by the enemy.");
        }

        public static void DisplayInvalidChoice(int maxChoice)
        {
            Console.WriteLine($"Invalid choice. Please enter a number between 1 and {maxChoice}.");
        }

        public static void DisplayNotEnoughActionPoints(ICapability capability, int currentActionPoints)
        {
            Console.WriteLine($"Not enough action points for {capability.Name}.");
            Console.WriteLine($"It costs {capability.ActionPointCost} AP and you have {currentActionPoints} AP left.");
        }

        private static void DisplayTitle(string title)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', SeparatorLength));
            Console.WriteLine($" {title}");
            Console.WriteLine(new string('=', SeparatorLength));
        }

        private static string FormatHealth(int health)
        {
            return $"{health}/{Unit.MaxHealth}";
        }
    }
}
