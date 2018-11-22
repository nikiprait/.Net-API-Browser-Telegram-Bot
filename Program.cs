using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Tg_NetAPIBrowser.Resources;
using Tg_NetAPIBrowser.Resources.MSDN;

namespace Tg_NetAPIBrowser
{
    class Program
    {
        static ParserWorker<string[]> parser;

        private static readonly TelegramBotClient bot = new TelegramBotClient("737319644:AAGAGZAxGtypy0kPMtdQO247xKOg6bYw6A0");

        static string Search = "";

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

        public static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            Message msg = e.Message;

            if (msg == null) return;

            if (msg.Type == MessageType.Text)
            {
                var language = new ReplyKeyboardMarkup
                {
                    Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new KeyboardButton("RU"),
                                                    new KeyboardButton("ENG"),
                                                    new KeyboardButton("PL"),
                                                    new KeyboardButton("DE")
                                                },
                                            },
                    ResizeKeyboard = true
                };
                
                switch (msg.Text)
                {
                    case "/start":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Hello, " + msg.From.FirstName + "!");
                        break;

                    #region Language Settings

                    case "RU":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Ищу информацию на русском языке!");
                        parser.Settings = new MSDNSettings
                        { BaseUrl = "https://docs.microsoft.com/ru-ru/dotnet/api" };
                        parser.Worker(Search, msg.Chat.Id.ToString());
                        break;
                    case "ENG":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Search information in english language!");
                        parser.Settings = new MSDNSettings
                        { BaseUrl = "https://docs.microsoft.com/en-us/dotnet/api" };
                        parser.Worker(Search, msg.Chat.Id.ToString());
                        break;
                    case "PL":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Szukam informacje w języku polskim!");
                        parser.Settings = new MSDNSettings
                        { BaseUrl = "https://docs.microsoft.com/pl-pl/dotnet/api" };
                        parser.Worker(Search, msg.Chat.Id.ToString());
                        break;
                    case "DE":
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Ich suche Informationen auf Deutsch!");
                        parser.Settings = new MSDNSettings
                        { BaseUrl = "https://docs.microsoft.com/de-de/dotnet/api" };
                        parser.Worker(Search, msg.Chat.Id.ToString());
                        break;

                    #endregion

                    default:
                        await bot.SendTextMessageAsync(msg.Chat.Id, "Hi, " + msg.From.FirstName + "!\nYou search: " + msg.Text, ParseMode.Default, false, false, 0, language);
                        Search = msg.Text;

                        #region Console Output

                        Console.WriteLine(
                            msg.From.LanguageCode + " | " +
                            msg.Date + " : " +
                            msg.From.FirstName + " " +
                            msg.From.LastName + " (" +
                            msg.From.Username + ") search: " +
                            msg.Text);

                        #endregion

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
