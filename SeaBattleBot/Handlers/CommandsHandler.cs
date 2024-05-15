using SeaBattleBot.Core.Domain.Contracts.Controllers;
using SeaBattleBot.Core.Enums;
using SeaBattleBot.Keyboards;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SeaBattleBot.Handlers
{
	public class CommandsHandler
	{
		private readonly ITelegramBotClient _botClient;
		private readonly IGameController _gameController;

		public CommandsHandler(ITelegramBotClient botClient, IGameController gameController)
		{
			_botClient = botClient;
			_gameController = gameController;
		}

		public async Task HandleCallbackQueryUpdateAsync(CallbackQuery callback, CancellationToken cancellationToken)
		{
			if (callback.Message is not { } callbackMessage)
				return;

			if (await _gameController.GetGameStatus(callbackMessage.Chat.Id) == GameStatus.NotStarted)
			{
				await _gameController.CreateField(callbackMessage.Chat.Id);
				Message sentMessage = await _botClient.SendTextMessageAsync(
				chatId: callbackMessage.Chat.Id,
				text: await _gameController.GetFields(callbackMessage.Chat.Id),
				cancellationToken: cancellationToken);
				return;
			}
		}

		public async Task<bool> HandleCommandAsync(string command, long chatId, CancellationToken cancellationToken)
		{
			switch (command)
			{
				case "/start":
					await _gameController.StartNewGame(chatId);
					await _botClient.SendTextMessageAsync(
					chatId: chatId,
					text:
                    "I'm a bot that can play with you in **Sea Battle**.\n" +
                    "To start the game, click the button below to generate a random field\n" +
                    "or send me your ship layout from the example below.\n" +
                    "Example field:\n" +
					"🌊🌊🚢🚢🚢🚢🌊🌊🚢🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊\r\n" +
					"🚢🚢🌊🌊🌊🌊🌊🌊🚢🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🌊🌊🚢🌊\r\n" +
					"🌊🌊🌊🚢🚢🌊🌊🌊🚢🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🚢🌊🌊🌊\r\n" +
					"🌊🚢🚢🚢🌊🌊🌊🌊🌊🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🚢🌊🌊🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊\r\n" +
					"🌊🚢🌊🌊🌊🌊🚢🚢🌊🌊",
					replyMarkup: CustomInlineKeyboards.GetStartInlineKeyboardMarkup(),
					cancellationToken: cancellationToken);
					return true;
				case "/restart":
					await _gameController.StartNewGame(chatId);
					await _botClient.SendTextMessageAsync(
					chatId: chatId,
					text:
                    "Choose how you want your field to be created, random field generation, or your own field as below?\n" +
					"Example field:\n" +
					"🌊🌊🚢🚢🚢🚢🌊🌊🚢🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊\r\n" +
					"🚢🚢🌊🌊🌊🌊🌊🌊🚢🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🌊🌊🚢🌊\r\n" +
					"🌊🌊🌊🚢🚢🌊🌊🌊🚢🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🚢🌊🌊🌊\r\n" +
					"🌊🚢🚢🚢🌊🌊🌊🌊🌊🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🚢🌊🌊🌊\r\n" +
					"🌊🌊🌊🌊🌊🌊🌊🌊🌊🌊\r\n" +
					"🌊🚢🌊🌊🌊🌊🚢🚢🌊🌊",
					replyMarkup: CustomInlineKeyboards.GetStartInlineKeyboardMarkup(),
					cancellationToken: cancellationToken);
					return true;
                case "/rules":
                    await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text:
                    "Welcome to Sea Battle!\n" +
					"Each player has a 10x10 grid for their fleet:\n" +
					"1 Battleship (4 cells)\n" +
                    "2 Cruisers (3 cells each)\n3 Destroyers (2 cells each)\n" +
                    "4 Submarines (1 cell each). Ships can’t overlap or be adjacent.\n" +
                    "You make turns firing shots by calling out a grid coordinate.\n" +
					"A hit grants an additional turn. When all cells of a ship are hit, the ship is sunk.\n" +
					"The game continues until one player sinks all of the opponent’s ships. The first to sink all ships wins.",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
                    return true;
                default:
					return false;
			}
		}

		public async Task HandleGameStatusAsync(GameStatus gameStatus, string messageText, long chatId, CancellationToken cancellationToken)
		{
			switch (gameStatus)
			{
				case GameStatus.NotStarted:
					if (!await _gameController.CreateField(chatId, messageText))
					{
						await _botClient.SendTextMessageAsync(
						chatId: chatId,
						text: "Field is not valid",
						cancellationToken: cancellationToken);
						return;
					}
					await _botClient.SendTextMessageAsync(
					chatId: chatId,
					text: await _gameController.GetFields(chatId),
					cancellationToken: cancellationToken);
					break;
				case GameStatus.InProgress:
					var res = await _gameController.PlayerMakeMove(chatId, messageText);
					if (!res.IsSuccess)
					{
						await _botClient.SendTextMessageAsync(
						chatId: chatId,
						text: res.ErrorMessage ?? "Something went wrong",
						cancellationToken: cancellationToken);
						return;
					}
					await _botClient.SendTextMessageAsync(
					chatId: chatId,
					text: await _gameController.GetFields(chatId),
					cancellationToken: cancellationToken);
					if (await _gameController.GetGameStatus(chatId) == GameStatus.Finished)
					{
						await _botClient.SendTextMessageAsync(
						chatId: chatId,
						text: "Game ended, write /restart",
						cancellationToken: cancellationToken);
					}
					break;
				case GameStatus.Finished:
					await _botClient.SendTextMessageAsync(
					chatId: chatId,
					text: "Game ended, write /restart",
					cancellationToken: cancellationToken);
					break;
				default:
					break;
			}
		}
	}

}
