using Freezer.Core;
using System;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Tg_NetAPIBrowser
{
    class Program
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("737319644:AAGAGZAxGtypy0kPMtdQO247xKOg6bYw6A0");

        static void Main(string[] args)
        {
            bot.OnMessage += Bot_OnMessage;
            bot.SetWebhookAsync("");

            var me = bot.GetMeAsync().Result;
            Console.Title = me.Username;

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Message msg = e.Message;

            if (msg == null) return;

            if (msg.Type == MessageType.Text)
            {
                //Тут добавляем команды
                switch (msg.Text)
                {
                    case "/start":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Hello, " + msg.From.FirstName + "!");
                        break;
                    case "/help":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Вставить сюда информацию о том, как пользоваться ботом");
                        break;
                    case "/donate":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Вставить сюда ссылку на донат");
                        break;
                    default:
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Hi, " + msg.From.FirstName + "!\nYou search: " + msg.Text);

                        var screenshotJob = ScreenshotJobBuilder.Create("https://docs.microsoft.com/ru-ru/dotnet/api/" + msg.Text + "?view=netframework-4.7.2")
                        .SetBrowserSize(1366, 768)
                        .SetCaptureZone(CaptureZone.FullPage)
                        .SetTrigger(new WindowLoadTrigger());
                        
                        System.IO.File.WriteAllBytes(@"C:\BOT\image.png", screenshotJob.Freeze());

                        using (FileStream stream = System.IO.File.Open("C:\\BOT\\image.png", FileMode.Open))
                        {
                            InputOnlineFile iof = new InputOnlineFile(stream);
                            await bot.SendPhotoAsync(msg.Chat.Id, iof, msg.Text);
                        }

                        System.IO.File.Delete(@"C:\BOT\image.png");
                        break;
                }
            }
            else
            {
                await bot.SendTextMessageAsync(msg.Chat.Id, "Sory, " + msg.From.FirstName + ", but this is not text.");
            }
        }
    }
}