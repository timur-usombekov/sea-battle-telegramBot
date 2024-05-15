# SeaBattleBot

SeaBattleBot is a Telegram bot for playing the classic game of Battleship. The bot is written in C# and uses .NET Core for creating web services.

## Features

- **Gameplay**: The bot allows users to play a game of Battleship against an AI opponent.
- **Field Creation**: Users can create their own battlefield or let the bot generate a random one.
- **Game Status**: Users can check the current status of the game at any time.
- **Move Handling**: The bot handles player moves, checks if they're valid, and updates the game state accordingly.

## Core Components

- **Controllers**: The application uses various controllers to manage different aspects of the game, such as the game state, the field, and the ships.
- **Services**: Services are used to handle business logic and interact with the repositories.
- **Repositories**: The repositories interact with the database using Entity Framework Core.
- **AI**: The bot uses an AI to make its moves during the game.
