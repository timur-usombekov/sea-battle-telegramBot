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
					InlineKeyboardButton.WithCallbackData(text: "Случайно сгенерировать поле", callbackData: "random"),
				},
			});
			return inlineKeyboard;
		}
	}
}
