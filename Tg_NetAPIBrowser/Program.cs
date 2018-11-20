using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Tg_NetAPIBrowser.Resources;
using Tg_NetAPIBrowser.Resources.MSDN;

namespace Tg_NetAPIBrowser
{
    class Program
    {
        static ParserWorker<string[]> parser;

        private static readonly TelegramBotClient bot = new TelegramBotClient("737319644:AAGAGZAxGtypy0kPMtdQO247xKOg6bYw6A0");

        static void Main(string[] args)
        {
            parser = new ParserWorker<string[]>(new MSDNParser());

            parser.OnNewData += Parser_OnNewData;

            bot.OnMessage += Bot_OnMessage;
            bot.SetWebhookAsync("");

            var me = bot.GetMeAsync().Result;
            Console.Title = me.Username;

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        public static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
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
                        await bot.SendTextMessageAsync(msg.Chat.Id, 
                            "/start - запустить бота \n" +
                            "/help - вывести эту справку \n" +
                            "/donate - вывести ссылку на донат");
                        break;
                    case "/donate":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Вставить сюда ссылку на донат");
                        break;
                    default:
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Hi, " + msg.From.FirstName + "!\nYou search: " + msg.Text);

                        #region Console Output

                        Console.WriteLine(
                            msg.From.LanguageCode + " | " + 
                            msg.Date + " : " + 
                            msg.From.FirstName + " " + 
                            msg.From.LastName + " (" + 
                            msg.From.Username + ") search: " + 
                            msg.Text);

                        #endregion

                        //Основная логика
                        parser.Settings = new MSDNSettings();
                        parser.Worker(msg.Text, msg.Chat.Id.ToString());

                        break;
                }
            }
            else
            {
                await bot.SendTextMessageAsync(msg.Chat.Id, "Sory, " + msg.From.FirstName + ", but this is not text.");
            }

        }

        private static async void Parser_OnNewData(object arg1, string[] arg2, string arg3)
        {
            try
            {
                await bot.SendTextMessageAsync(arg3, arg2[0]);
            }
            catch
            {
                await bot.SendTextMessageAsync(arg3, "Error, this is not look like namespace or class or method, try again.");
            }
        }
    }
}
