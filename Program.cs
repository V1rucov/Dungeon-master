using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Threading;

namespace Dungeon_master
{
    class Program
    {
        static string Token = "";
        static CommandsNextModule CNMmodule;
        static DiscordClient client { get; set; }
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadLine();
        }
        static async Task MainAsync(string[] args)
        {
            client = new DiscordClient(new DiscordConfiguration() { Token = Token, TokenType = TokenType.Bot, UseInternalLogHandler = true, LogLevel = LogLevel.Debug });
            CNMmodule = client.UseCommandsNext(new CommandsNextConfiguration() { EnableMentionPrefix = true });
            CNMmodule.RegisterCommands<Commands>();

            client.MessageCreated += async e =>
            {
                if (e.Message.MentionEveryone)
                {
                    await e.Message.RespondAsync("Нельзя использовать эвриван");
                }
                else if (e.Message.Content == "тут?")
                {
                    await e.Message.RespondAsync("хули ты хотел?");
                }
            };
            await client.ConnectAsync();
        }
    }
}
