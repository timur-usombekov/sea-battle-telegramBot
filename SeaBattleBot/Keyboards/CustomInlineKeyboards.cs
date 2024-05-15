using Telegram.Bot.Types.ReplyMarkups;

namespace SeaBattleBot.Keyboards
{
	public static class CustomInlineKeyboards
	{
		public static InlineKeyboardMarkup GetStartInlineKeyboardMarkup() 
		{ 
			InlineKeyboardMarkup inlineKeyboard = new(new[]
			{
				new []
				{
					InlineKeyboardButton.WithCallbackData(text: "Generate random field", callbackData: "random"),
				},
			});
			return inlineKeyboard;
		}
	}
}
