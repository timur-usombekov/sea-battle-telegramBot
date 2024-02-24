using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Microsoft.Extensions.DependencyInjection;
using SeaBattleBot.Core.Domain.Contracts.Controllers;
using SeaBattleBot.Extensions;
using SeaBattleBot.Handlers;

var botClient = new TelegramBotClient(MySecrets.GetBotKey());

using CancellationTokenSource cts = new();
ReceiverOptions receiverOptions = new()
{
	AllowedUpdates = Array.Empty<UpdateType>()
};

var services = new ServiceCollection();
ConfigureServices.ConfigureControllerWithRepositories(services);
var provider = services.BuildServiceProvider();

IGameController gameController = provider.GetRequiredService<IGameController>();
CommandsHandler commandsHandler = new(botClient, gameController);

botClient.StartReceiving(
	updateHandler: HandleUpdateAsync,
	pollingErrorHandler: HandlePollingErrorAsync,
	receiverOptions: receiverOptions,
	cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();
Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();
cts.Cancel();
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
	if (update.Message is not { } message)
	{
		if (update.CallbackQuery is not { } callback)
			return;

		await commandsHandler.HandleCallbackQueryUpdateAsync(callback, cancellationToken);
		return;
	}
		
	if (message.Text is not { } messageText)
		return;

	var chatId = message.Chat.Id;
	Console.WriteLine($"Received a {messageText} message in chat {chatId}.");
	if (await commandsHandler.HandleCommandAsync(messageText, chatId, cancellationToken))
		return;
	
	var status = await gameController.GetGameStatus(chatId);
	await commandsHandler.HandleGameStatusAsync(status, messageText, chatId, cancellationToken);
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
	var ErrorMessage = exception switch
	{
		ApiRequestException apiRequestException
			=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
		_ => exception.ToString()
	};

	Console.WriteLine(ErrorMessage);
	return Task.CompletedTask;
}
