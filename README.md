# Turn-Based Combat

A turn-based fight against an enemy, played in the console and written in C# (.NET 10). Each turn you get 3 action points to attack or drink a potion, then the enemy hits back, until one of you drops to 0 HP.

The goal was to practice the State pattern, events with `Action<T>`, abstract classes, interfaces and unit testing with NUnit.

## Requirements

- .NET 10 SDK

## Running

From the solution folder:

    dotnet run --project TurnBasedCombat.Console

You can also open `TurnBasedCombat.slnx` in your IDE (Visual Studio, Rider, VS Code) and run the `TurnBasedCombat.Console` project.

## Running the tests

    dotnet test

## Rules

- The combat opposes 1 hero (you) to 1 enemy.
- Each unit has 100 health points and gets 3 action points at the start of each of its turns.
- A unit at 0 health is dead: it can't act, be targeted or be healed anymore.
- Your turn ends when you pass, or when you can't afford any action with your remaining action points.
- The enemy uses Sword Strike until it runs out of action points.

## Actions

    Action          Cost    Effect
    Sword Strike    1 AP    Deals 10 damage to the enemy
    Heavy Strike    2 AP    Deals 25 damage to the enemy
    Potion          1 AP    Heals you for 20 health points
    Pass turn       -       Ends your turn

## Project structure

    TurnBasedCombat.Core          Game logic: units, capabilities and combat states
    TurnBasedCombat.Console       Console interface: reads the player's input and displays the combat
    TurnBasedCombat.Tests         NUnit tests for the units and the combat flow

## Design

- The combat phases (player turn, enemy turn, combat over) use the State pattern. Each phase is an object implementing `ICombatState` and decides which phase comes next, so the combat loop never checks whose turn it is.
- `Hero` and `Enemy` inherit from the abstract class `Unit`. Capabilities implement the `ICapability` interface, so the combat logic never needs to know which action is being used.
- Units never write to the console. They raise events (`DamageTaken`, `HealReceived`, `Killed`) and the console app subscribes to them to display what happens. The same goes for `Combat`, which raises `StateChanged` when a new phase starts.
- The game logic is covered by NUnit tests that run without the console.
