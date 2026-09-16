using System.Text;
using CrawlerService;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotService
{
    public class TelegramBot
    {
        private readonly List<Course> _courses;
        public TelegramBot(List<Course> courses)
        {
            _courses = courses;
            _telegramBot = new TelegramBotClient("YOUR TOKEN");

        }
        private TelegramBotClient _telegramBot;

        public async Task RunBot()
        {
            try
            {
                Console.WriteLine("Bot Connected");
                int offset = 0;
                while (true)
                {
                    Update[] updates = await _telegramBot.GetUpdatesAsync(offset);

                    foreach (var update in updates)
                    {
                        offset = update.Id + 1;
                        if (update.Message == null || update.Message.Text == "")
                            continue;

                        string message = update.Message.Text!.ToLower();
                        var from = update.Message.From;
                        var chatId = update.Message.Chat.Id;

                        if (update.Message.Text == "/start")
                        {
                            await SendInlineKeyboardAsync(chatId);
                        }

                        if (message.Contains("/courselist"))
                        {
                            foreach (var item in _courses)
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                stringBuilder.AppendLine("✨آکادمی برنامه نویسان برگزار میکند ✨");
                                stringBuilder.AppendLine(item.Title);
                                stringBuilder.AppendLine($"مدرس : {item.MasterName}");
                                stringBuilder.AppendLine($"طول دوره: {item.Sections} ({item.HowLongIsCourse})");
                                stringBuilder.AppendLine($"شروع دوره: {item.DayOfWeek} {item.StartDate}");
                                stringBuilder.AppendLine("به صورت حضوری و آنلاین");
                                stringBuilder.AppendLine("شعبه اصلی");
                                stringBuilder.AppendLine("");
                                stringBuilder.AppendLine("");
                                stringBuilder.AppendLine("برای مشاهده جزییات بیشتر روی لینک زیر کلیک کنید");
                                stringBuilder.AppendLine("");
                                stringBuilder.AppendLine(item.ShortLink?.Trim().ToLower());
                                stringBuilder.AppendLine("");
                                stringBuilder.AppendLine("");
                                stringBuilder.AppendLine("☎️ 021-91303737 -- 021-88454816");
                                stringBuilder.AppendLine("");
                                stringBuilder.AppendLine("🆔 @AcademyBarnamenevisan");
                                await _telegramBot.SendTextMessageAsync(chatId, stringBuilder.ToString());
                            }
                        }
                    }
                }

            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cant Connect check your internet, press any key to close...!");
                Console.ResetColor();
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public async Task SendInlineKeyboardAsync(long chatId)
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("لیست دوره ها", "CourselistCommand"),
                InlineKeyboardButton.WithCallbackData("گزینه 2", "Option2"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("گزینه 3", "Option3"),
                InlineKeyboardButton.WithCallbackData("گزینه 4", "Option4"),
            }
        });

            await _telegramBot.SendTextMessageAsync(chatId, "لطفا یک گزینه را انتخاب کنید:", replyMarkup: keyboard);
        }
    }
}
