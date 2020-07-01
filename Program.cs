#define debug
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using System.Threading;
using System.Data.Entity;

namespace Dungeon_master
{
    class Program
    {
        static string Token = "NTM4NzQzNTY3ODA0MjY4NTQ0.XvMtyQ.g0ysTaA20ERuHtzk-jwAAMFR210";
        static CommandsNextModule CNMmodule;
        static DiscordClient client { get; set; }
        static void Main(string[] args)
        {
            #if debug
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("["+DateTime.Now+"]"+"[Dungeon master v1.0] Debug mode");
            #endif
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CharacterContext>());
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
