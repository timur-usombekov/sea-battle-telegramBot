using SeaBattleBot.Core.Domain.Contracts.Controllers;
using SeaBattleBot.Core.Enums;
using SeaBattleBot.Keyboards;
using Telegram.Bot;
using Telegram.Bot.Types;

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
					"Я бот который может играть в морской бой.\n" +
					"Для начала игры нажми кнопку ниже(для генерации случайного поля)\n" +
					"либо пришли мне своё распололжение кораблей по примеру ниже.\n" +
					"Пример поля:\n" +
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
					"Выбери способ создания своего поля, генерация случайного поля, либо собственное поле как указано ниже?\n" +
					"Пример поля:\n" +
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
						text: "Поле построено не верно",
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
						text: "Игра окончена, напишите /restart",
						cancellationToken: cancellationToken);
					}
					break;
				case GameStatus.Finished:
					await _botClient.SendTextMessageAsync(
					chatId: chatId,
					text: "Игра окончена, напишите /restart",
					cancellationToken: cancellationToken);
					break;
				default:
					break;
			}
		}
	}

}
