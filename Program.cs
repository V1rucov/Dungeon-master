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
using System.IO;
using System.Diagnostics;
using Dungeon_master.cmnds;

namespace Dungeon_master
{
    class Program
    {
        static string Token = new StreamReader(@"C:\Users\rujni\Desktop\token.txt").ReadLine();
        static CommandsNextModule CNMmodule;
        static DiscordClient client { get; set; }
        static void Main(string[] args)
        {
            debug();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CharacterContext>());
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadLine();
        }
        [Conditional("debug")]
        static void debug() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[" + DateTime.Now + "] "+ " [Dungeon master v1.0] Debug mode -- ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("UNCOMMERCIAL USE ONLY");
        }
        static async Task MainAsync(string[] args)
        {
            client = new DiscordClient(new DiscordConfiguration() { Token = Token, TokenType = TokenType.Bot, UseInternalLogHandler = true, LogLevel = LogLevel.Debug});
            CNMmodule = client.UseCommandsNext(new CommandsNextConfiguration() { StringPrefix = "/"});
            client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = Timeout.InfiniteTimeSpan
            });

            CNMmodule.RegisterCommands<Commands>();
            CNMmodule.RegisterCommands<OwnerCommands>();
            CNMmodule.RegisterCommands<PartyCommands>();

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
