# SeaBattleBot

SeaBattleBot is a Telegram bot for playing the classic game of Battleship. The bot is written in C#.

## Features

- **Gameplay**: The bot allows users to play a game of Battleship against an AI opponent.
- **Field Creation**: Users can create their own battlefield or let the bot generate a random one.
- **Move Handling**: The bot handles player moves, checks if they're valid, and updates the game state accordingly.

## Core Components

- **Controllers**: The application uses various controllers to manage different aspects of the game, such as the game state, the field, and the ships.
- **Services**: Services are used to interact with the repositories.
- **Repositories**: The repositories interact with the database using Entity Framework Core.
- **AI**: The bot uses an special algorithm to make its moves during the game.
